using System.Text.Json;
using System.Text.Json.Serialization;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Models
{
    [JsonConverter(typeof(IniOffsetsSerializer))]
    public class IniOffsets : IOffsets
    {
        private readonly Ini _ini;

        #region Constructors

        private IniOffsets(Ini ini)
        {
            _ini = ini;
        }

        public static IniOffsets Create(string value)
        {
            return new IniOffsets(Ini.Create(value));
        }

        #endregion

        #region Overrides of object

        public override string ToString()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(this, options);
        }

        #endregion

        #region Implementation of IOffsets Properties

        public uint ButtonInAttack => _ini.Get("Buttons", "in_attack");
        public uint ButtonInSpeed => _ini.Get("Buttons", "in_speed");
        public uint ButtonInZoom => _ini.Get("Buttons", "in_zoom");
        public uint CoreEntityList => _ini.Get("Miscellaneous", "cl_entitylist");
        public uint CoreLevelName => _ini.Get("Miscellaneous", "LevelName");
        public uint CoreLocalPlayer => _ini.Get("Miscellaneous", "LocalPlayer");
        public uint EntityLastVisibleTime => _ini.Get("Miscellaneous", "CPlayer!lastVisibleTime");
        public uint EntityLocalOrigin => _ini.Get("DataMap.CBaseViewModel", "m_localOrigin");
        public uint EntitySignifierName => _ini.Get("RecvTable.DT_BaseEntity", "m_iSignifierName");
        public uint PlayerBleedoutState => _ini.Get("RecvTable.DT_Player", "m_bleedoutState");
        public uint PlayerDuckState => _ini.Get("RecvTable.DT_Player", "m_duckState");
        public uint PlayerGlowEnable => _ini.Get("RecvTable.DT_HighlightSettings", "m_highlightServerContextID") + 0x8;
        public uint PlayerGlowThroughWall => _ini.Get("RecvTable.DT_HighlightSettings", "m_highlightServerContextID") + 0x10;
        public uint PlayerLifeState => _ini.Get("RecvTable.DT_Player", "m_lifeState");
        public uint PlayerName => _ini.Get("RecvTable.DT_BaseEntity", "m_iName");
        public uint PlayerTeamNum => _ini.Get("RecvTable.DT_BaseEntity", "m_iTeamNum");
        public uint PlayerVecPunchWeaponAngle => _ini.Get("DataMap.C_Player", "m_currentFrameLocalPlayer.m_vecPunchWeapon_Angle");
        public uint PlayerViewAngle => _ini.Get("DataMap.C_Player", "m_ammoPoolCapacity") - 0x14;

        #endregion
    }
}