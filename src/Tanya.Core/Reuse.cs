namespace Tanya.Core
{
    public static class Reuse
    {
        private static byte[] _buffer;
        private static int _bufferSize;

        #region Constructors

        static Reuse()
        {
            _bufferSize = 1024;
            _buffer = new byte[_bufferSize];
        }

        #endregion

        #region Statics

        public static byte[] GetBuffer(int count)
        {
            if (_bufferSize >= count) return _buffer;
            _bufferSize = count;
            _buffer = new byte[_bufferSize];
            return _buffer;
        }

        #endregion
    }
}