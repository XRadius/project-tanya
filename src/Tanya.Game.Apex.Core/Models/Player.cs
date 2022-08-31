using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Core.Extensions;
using Tanya.Core.Models;
using Tanya.Core.Types;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;
using Tanya.Game.Apex.Core.Utilities;

namespace Tanya.Game.Apex.Core.Models
{
    public class Player : IUpdatable
    {
        private readonly Access<byte> _bleedoutState;
        private readonly Access<byte> _duckState;
        private readonly Access<byte> _glowEnable;
        private readonly Access<byte> _glowThroughWalls;
        private readonly LastVisibleTime _lastVisibleTime;
        private readonly Access<byte> _lifeState;
        private readonly Access<Vector> _localOrigin;
        private readonly Access<ulong> _name;
        private readonly Access<byte> _teamNum;
        private readonly Access<Vector> _vecPunchWeaponAngle;
        private readonly Access<Vector> _viewAngle;

        #region Constructors

        public Player(IDriver driver, IOffsets offsets, ulong address)
        {
            _bleedoutState = driver.Access(address + offsets.PlayerBleedoutState, ByteType.Instance);
            _duckState = driver.Access(address + offsets.PlayerDuckState, ByteType.Instance);
            _glowEnable = driver.Access(address + offsets.PlayerGlowEnable, ByteType.Instance);
            _glowThroughWalls = driver.Access(address + offsets.PlayerGlowThroughWall, ByteType.Instance);
            _lastVisibleTime = new LastVisibleTime(driver.Access(address + offsets.EntityLastVisibleTime, SingleType.Instance));
            _lifeState = driver.Access(address + offsets.PlayerLifeState, ByteType.Instance);
            _localOrigin = driver.Access(address + offsets.EntityLocalOrigin, VectorType.Instance);
            _name = driver.Access(address + offsets.PlayerName, UInt64Type.Instance);
            _teamNum = driver.Access(address + offsets.PlayerTeamNum, ByteType.Instance, 1000);
            _vecPunchWeaponAngle = driver.Access(address + offsets.PlayerVecPunchWeaponAngle, VectorType.Instance);
            _viewAngle = driver.Access(address + offsets.PlayerViewAngle, VectorType.Instance);
        }

        #endregion

        #region Methods

        public bool IsSameTeam(Player otherPlayer)
        {
            return TeamNum == otherPlayer.TeamNum;
        }

        public bool IsValid()
        {
            return GlowEnable != 0 && GlowEnable != 255 && LifeState == 0 && LocalOrigin != Vector.Origin && Name != 0;
        }

        #endregion

        #region Properties

        [JsonPropertyName("bleedoutState")]
        public byte BleedoutState
        {
            get => _bleedoutState.Get();
            set => _bleedoutState.Set(value);
        }

        [JsonPropertyName("duckState")]
        public byte DuckState
        {
            get => _duckState.Get();
            set => _duckState.Set(value);
        }

        [JsonPropertyName("glowEnable")]
        public byte GlowEnable
        {
            get => _glowEnable.Get();
            set => _glowEnable.Set(value);
        }

        [JsonPropertyName("glowThroughWalls")]
        public byte GlowThroughWalls
        {
            get => _glowThroughWalls.Get();
            set => _glowThroughWalls.Set(value);
        }

        [JsonPropertyName("lifeState")]
        public byte LifeState
        {
            get => _lifeState.Get();
            set => _lifeState.Set(value);
        }

        [JsonPropertyName("localOrigin")]
        public Vector LocalOrigin
        {
            get => _localOrigin.Get();
            set => _localOrigin.Set(value);
        }

        [JsonPropertyName("name")]
        public ulong Name
        {
            get => _name.Get();
            set => _name.Set(value);
        }

        [JsonPropertyName("teamNum")]
        public byte TeamNum
        {
            get => _teamNum.Get();
            set => _teamNum.Set(value);
        }

        [JsonPropertyName("vecPunchWeaponAngle")]
        public Vector VecPunchWeaponAngle
        {
            get => _vecPunchWeaponAngle.Get();
            set => _vecPunchWeaponAngle.Set(value);
        }

        [JsonPropertyName("viewAngle")]
        public Vector ViewAngle
        {
            get => _viewAngle.Get();
            set => _viewAngle.Set(value);
        }

        [JsonPropertyName("visible")]
        public bool Visible => _lastVisibleTime.Visible;

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            _bleedoutState.Update(frameTime);
            _duckState.Update(frameTime);
            _glowEnable.Update(frameTime);
            _glowThroughWalls.Update(frameTime);
            _lastVisibleTime.Update(frameTime);
            _lifeState.Update(frameTime);
            _localOrigin.Update(frameTime);
            _name.Update(frameTime);
            _teamNum.Update(frameTime);
            _vecPunchWeaponAngle.Update(frameTime);
            _viewAngle.Update(frameTime);
        }

        #endregion
    }
}