using System.Collections.Concurrent;
using Tanya.Driver.Interfaces;
using Tanya.Game.Apex.Core.Interfaces;

namespace Tanya.Game.Apex.Core.Models
{
    public class SignifierList : IUpdatable
    {
        private readonly IDriver _driver;
        private readonly ConcurrentDictionary<ulong, Signifier> _value;

        #region Constructors

        public SignifierList(IDriver driver)
        {
            _driver = driver;
            _value = new ConcurrentDictionary<ulong, Signifier>();
        }

        #endregion

        #region Methods

        public Signifier GetOrAdd(ulong address)
        {
            return _value.GetOrAdd(address, _ =>
            {
                var signifier = new Signifier(_driver, address);
                signifier.Update(DateTime.UtcNow);
                return signifier;
            });
        }

        #endregion

        #region Implementation of IUpdatable

        public void Update(DateTime frameTime)
        {
            foreach (var (_, signifier) in _value)
            {
                signifier.Update(frameTime);
            }
        }

        #endregion
    }
}