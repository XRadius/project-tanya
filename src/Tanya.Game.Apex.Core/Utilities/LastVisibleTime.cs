using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Utilities
{
    public class LastVisibleTime : IUpdatable
    {
        private readonly Access<float> _value;
        private long _previousTicks;
        private float _previousValue;

        #region Constructors

        public LastVisibleTime(Access<float> value)
        {
            _value = value;
        }

        #endregion

        #region Properties

        [JsonPropertyName("isVisible")]
        public bool Visible { get; private set; }

        #endregion

        #region Implementation of IUpdatable

        [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
        public void Update(DateTime frameTime)
        {
            if (_value.Update(frameTime))
            {
                var currentValue = _value.Get();

                if (currentValue != _previousValue)
                {
                    _previousTicks = frameTime.Ticks;
                    _previousValue = currentValue;
                    Visible = true;
                }
                else if (Visible && _previousTicks + Constants.VisibilityTicks < frameTime.Ticks)
                {
                    Visible = false;
                }
            }
        }

        #endregion
    }
}