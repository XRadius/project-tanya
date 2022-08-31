using Tanya.Core.Interfaces;
using Tanya.Driver.Interfaces;

namespace Tanya.Core
{
    public class Access<T>
    {
        private readonly ulong _address;
        private readonly IDriver _driver;
        private readonly Limiter? _limiter;
        private readonly IType<T> _type;
        private T? _value;

        #region Constructors

        public Access(IDriver driver, ulong address, IType<T> type)
        {
            _address = address;
            _driver = driver;
            _type = type;
        }

        public Access(IDriver driver, ulong address, IType<T> type, uint interval)
        {
            _address = address;
            _driver = driver;
            _limiter = new Limiter(interval);
            _type = type;
        }

        #endregion

        #region Methods

        public T Get()
        {
            return _value ?? throw new InvalidDataException();
        }

        public void Set(T value)
        {
            if (_value != null && _value.Equals(value)) return;
            var bufferSize = _type.Size();
            var buffer = Reuse.GetBuffer(bufferSize);
            _type.Set(buffer, value);
            _driver.Write(_address, buffer, bufferSize);
            _value = value;
        }

        public bool Update(DateTime frameTime)
        {
            if (_limiter != null && !_limiter.Update(frameTime)) return false;
            var bufferSize = _type.Size();
            var buffer = Reuse.GetBuffer(bufferSize);
            _driver.Read(_address, buffer, bufferSize);
            _value = _type.Get(buffer);
            return true;
        }

        #endregion
    }
}