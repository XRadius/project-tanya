using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Core.Extensions;
using Tanya.Core.Types;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class ButtonList : IUpdatable
    {
        private readonly Access<byte> _inAttack;
        private readonly Access<byte> _inSpeed;
        private readonly Access<byte> _inZoom;

        #region Constructors

        public ButtonList(IDriver driver, IOffsets offsets, ulong address)
        {
            _inAttack = driver.Access(address + offsets.ButtonInAttack, ByteType.Instance);
            _inSpeed = driver.Access(address + offsets.ButtonInSpeed, ByteType.Instance);
            _inZoom = driver.Access(address + offsets.ButtonInZoom, ByteType.Instance);
        }

        #endregion

        #region Properties

        [JsonPropertyName("inAttack")]
        public byte InAttack
        {
            get => _inAttack.Get();
            set => _inAttack.Set(value);
        }

        [JsonPropertyName("inSpeed")]
        public byte InSpeed
        {
            get => _inSpeed.Get();
            set => _inSpeed.Set(value);
        }

        [JsonPropertyName("inZoom")]
        public byte InZoom
        {
            get => _inZoom.Get();
            set => _inZoom.Set(value);
        }

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            _inAttack.Update(frameTime);
            _inSpeed.Update(frameTime);
            _inZoom.Update(frameTime);
        }

        #endregion
    }
}