using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class EntityList : IUpdatable
    {
        private readonly ulong _address;
        private readonly IDriver _driver;
        private readonly LevelName _levelName;
        private readonly Limiter _limiter;
        private readonly IOffsets _offsets;
        private readonly ConcurrentDictionary<ulong, Entity> _value;

        #region Constructors

        public EntityList(IDriver driver, IOffsets offsets, LevelName levelName, ulong address)
        {
            _address = address + offsets.CoreEntityList;
            _driver = driver;
            _offsets = offsets;
            _levelName = levelName;
            _limiter = new Limiter(Constants.EntityListInterval);
            _value = new ConcurrentDictionary<ulong, Entity>();
        }

        #endregion

        #region Methods

        private void Synchronize()
        {
            var bufferSize = _levelName.Value == "mp_rr_canyonlands_staging"
                ? Constants.EntityListSizeFull
                : Constants.EntityListSizePlayer;
            var buffer = Reuse.GetBuffer(bufferSize);
            var knownSet = new HashSet<ulong>();

            if (_driver.Read(_address, buffer, bufferSize))
            {
                for (var i = 0; i < bufferSize; i += 32)
                {
                    var address = Unsafe.ReadUnaligned<ulong>(ref buffer[i]);
                    if (address == 0) continue;
                    knownSet.Add(address);
                }

                foreach (var address in knownSet)
                {
                    if (_value.ContainsKey(address)) continue;
                    _value.TryAdd(address, new Entity(_driver, _offsets, address));
                }

                foreach (var (address, _) in _value)
                {
                    if (knownSet.Contains(address)) continue;
                    _value.TryRemove(address, out _);
                }
            }
        }

        #endregion

        #region Properties

        [JsonPropertyName("value")]
        public IReadOnlyDictionary<ulong, Entity> Value => _value;

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            if (_limiter.Update(frameTime))
            {
                Synchronize();
            }

            foreach (var (_, entity) in _value)
            {
                entity.Update(frameTime);
            }
        }

        #endregion
    }
}