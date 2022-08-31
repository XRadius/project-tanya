using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Tanya.Core.Extensions;

namespace Tanya.Game.Apex
{
    public class Config
    {
        private readonly IConfigurationSection _config;

        #region Constructors

        public Config(IConfiguration configuration)
        {
            _config = configuration.GetSection(typeof(Config).Namespace);
        }

        #endregion

        #region Properties

        [JsonPropertyName("framesPerSecond")]
        public int FramesPerSecond => _config.GetProperty<int>();

        [JsonPropertyName("offsetsCheck")]
        public int OffsetsCheck => _config.GetProperty<int>();

        [JsonPropertyName("offsetsRefresh")]
        public int OffsetsRefresh => _config.GetProperty<int>();

        [JsonPropertyName("offsetsUrl")]
        public string OffsetsUrl => _config.GetProperty<string>();

        [JsonPropertyName("processRefresh")]
        public int ProcessRefresh => _config.GetProperty<int>();

        #endregion
    }
}