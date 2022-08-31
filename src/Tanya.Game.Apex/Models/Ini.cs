using System.Globalization;
using System.Text.RegularExpressions;

namespace Tanya.Game.Apex.Models
{
    public class Ini
    {
        private readonly Dictionary<string, Dictionary<string, string>> _values;

        #region Constructors

        private Ini()
        {
            _values = new Dictionary<string, Dictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);
        }

        public static Ini Create(string text)
        {
            var ini = new Ini();
            ini.Load(text);
            return ini;
        }

        #endregion

        #region Methods

        public uint Get(string name, string key)
        {
            if (!_values.TryGetValue(name, out var section))
            {
                throw new InvalidDataException();
            }

            if (!section.TryGetValue(key, out var value))
            {
                throw new InvalidDataException();
            }

            if (!uint.TryParse(Regex.Replace(value, "^0x", string.Empty), NumberStyles.HexNumber, null, out var offset))
            {
                throw new InvalidDataException();
            }

            return offset;
        }

        private void Load(string text)
        {
            var currentSection = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            _values[string.Empty] = currentSection;

            foreach (var line in text.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                if (line.StartsWith(";"))
                {
                    continue;
                }

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    _values[line[1..^1]] = currentSection;
                    continue;
                }

                if (currentSection != null)
                {
                    var pair = line.Split('=', 2);
                    if (currentSection.ContainsKey(pair[0])) continue;
                    currentSection[pair[0]] = pair.Length == 2 ? pair[1] : string.Empty;
                }
            }
        }

        #endregion
    }
}