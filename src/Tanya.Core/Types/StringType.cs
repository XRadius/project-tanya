using System.Text;
using Tanya.Core.Interfaces;

namespace Tanya.Core.Types
{
    public class StringType : IType<string>
    {
        private readonly int _count;
        private readonly Encoding _encoding;

        #region Constructors

        public StringType(int count, Encoding encoding)
        {
            _count = count;
            _encoding = encoding;
        }

        #endregion

        #region Implementation of IType<string>

        public string Get(byte[] buffer)
        {
            var value = _encoding.GetString(buffer, 0, _count);
            var terminator = value.IndexOf('\0');
            return terminator != -1 ? value[..terminator] : value;
        }

        public void Set(byte[] buffer, string value)
        {
            var count = _encoding.GetBytes(value, buffer);
            for (var i = _count - 1; i >= count; i--) buffer[i] = 0;
        }

        public int Size()
        {
            return _count;
        }

        #endregion
    }
}