using System.Text.Json.Serialization;
using Tanya.Core.Models;
using Tanya.Game.Apex.Core.Models;
using Tanya.Game.Apex.Feature.Aim.Interfaces;

namespace Tanya.Game.Apex.Feature.Aim.Models
{
    public class PlayerTarget : ITarget
    {
        private readonly Player _player;

        #region Constructors

        public PlayerTarget(Player player)
        {
            _player = player;
        }

        #endregion

        #region Implementation of ITarget

        public bool IsSameTeam(Player localPlayer)
        {
            return _player.IsSameTeam(localPlayer);
        }

        public bool IsValid(Player localPlayer)
        {
            return _player.IsValid() && _player != localPlayer;
        }

        #endregion

        #region Implementation of ITarget Properties

        [JsonPropertyName("bleedoutState")]
        public byte BleedoutState => _player.BleedoutState;

        [JsonPropertyName("duckState")]
        public byte DuckState => _player.DuckState;

        [JsonPropertyName("localOrigin")]
        public Vector LocalOrigin => _player.LocalOrigin;

        [JsonPropertyName("visible")]
        public bool Visible => _player.Visible;

        #endregion
    }
}