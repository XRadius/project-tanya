using System.Text;
using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Core.Extensions;
using Tanya.Core.Types;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class Signifier : IUpdatable
    {
        private readonly Access<string> _value;

        #region Constructors

        public Signifier(IDriver driver, ulong address)
        {
            _value = driver.Access(address, new StringType(32, Encoding.ASCII), 60000);
        }

        #endregion

        #region Properties

        [JsonPropertyName("name")]
        public string Value
        {
            get => _value.Get();
            set => _value.Set(value);
        }

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            _value.Update(frameTime);
        }

        #endregion
    }
}