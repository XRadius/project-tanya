using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using Tanya.Core;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public abstract class EntityListFilter<T> : IUpdatable where T : IUpdatable
    {
        private readonly IDriver _driver;
        private readonly EntityList _entities;
        private readonly Limiter _limiter;
        private readonly SignifierList _signifiers;
        private readonly string _target;
        private readonly ConcurrentDictionary<ulong, T> _value;

        #region Abstracts

        protected abstract T Create(IDriver driver, ulong address);

        #endregion

        #region Constructors

        protected EntityListFilter(IDriver driver, EntityList entities, SignifierList signifiers, string target)
        {
            _driver = driver;
            _entities = entities;
            _limiter = new Limiter(Constants.EntityListInterval);
            _signifiers = signifiers;
            _target = target;
            _value = new ConcurrentDictionary<ulong, T>();
        }

        #endregion

        #region Methods

        private void Synchronize()
        {
            var knownSet = new HashSet<ulong>();

            foreach (var (address, entity) in _entities.Value)
            {
                var signifier = _signifiers.GetOrAdd(entity.SignifierName);
                if (signifier.Value != _target) continue;
                knownSet.Add(address);
            }

            foreach (var address in knownSet)
            {
                if (_value.ContainsKey(address)) continue;
                _value.TryAdd(address, Create(_driver, address));
            }

            foreach (var (address, _) in _value)
            {
                if (knownSet.Contains(address)) continue;
                _value.TryRemove(address, out _);
            }
        }

        #endregion

        #region Properties

        [JsonPropertyName("value")]
        public IReadOnlyDictionary<ulong, T> Value => _value;

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            if (_limiter.Update(frameTime))
            {
                Synchronize();
            }

            foreach (var (_, player) in _value)
            {
                player.Update(frameTime);
            }
        }

        #endregion
    }
}