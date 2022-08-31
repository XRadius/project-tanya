using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Models
{
    public class IniOffsetsSerializer : JsonConverter<IniOffsets>
    {
        private readonly IEnumerable<PropertyInfo> _properties;

        #region Constructors

        public IniOffsetsSerializer()
        {
            _properties = typeof(IOffsets).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        #endregion

        #region Methods

        private void Serialize(Utf8JsonWriter writer, IniOffsets offset)
        {
            foreach (var property in _properties)
            {
                var result = property.GetValue(offset);
                if (result is not uint value) throw new NotSupportedException();
                writer.WriteString(property.Name[..1].ToLower() + property.Name[1..], $"0x{value:X}");
            }
        }

        #endregion

        #region Overrides of JsonConverter<IniOffsets>

        public override IniOffsets Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, IniOffsets offset, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            Serialize(writer, offset);
            writer.WriteEndObject();
        }

        #endregion
    }
}