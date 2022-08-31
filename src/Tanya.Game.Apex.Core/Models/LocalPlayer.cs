using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Core.Extensions;
using Tanya.Core.Types;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class LocalPlayer : IUpdatable
    {
        private readonly Access<ulong> _value;

        #region Constructors

        public LocalPlayer(IDriver driver, IOffsets offsets, ulong address)
        {
            _value = driver.Access(address + offsets.CoreLocalPlayer, UInt64Type.Instance, 1000);
        }

        #endregion

        #region Properties

        [JsonPropertyName("value")]
        public ulong Value => _value.Get();

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            _value.Update(frameTime);
        }

        #endregion
    }
}