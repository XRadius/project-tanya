using System.Text.Json.Serialization;
using Tanya.Core.Models;
using Tanya.Game.Apex.Core.Models;
using Tanya.Game.Apex.Feature.Aim.Interfaces;

namespace Tanya.Game.Apex.Feature.Aim.Models
{
    public class NpcTarget : ITarget
    {
        private readonly Npc _npc;

        #region Constructors

        public NpcTarget(Npc npc)
        {
            _npc = npc;
        }

        #endregion

        #region Implementation of ITarget

        public bool IsSameTeam(Player localPlayer)
        {
            return false;
        }

        public bool IsValid(Player localPlayer)
        {
            return true;
        }

        #endregion

        #region Implementation of ITarget Properties

        [JsonPropertyName("bleedoutState")]
        public byte BleedoutState => 0;

        [JsonPropertyName("duckState")]
        public byte DuckState => 0;

        [JsonPropertyName("localOrigin")]
        public Vector LocalOrigin => _npc.LocalOrigin;

        [JsonPropertyName("visible")]
        public bool Visible => _npc.Visible;

        #endregion
    }
}