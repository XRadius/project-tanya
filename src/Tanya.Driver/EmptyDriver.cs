using Tanya.Driver.Interfaces;

namespace Tanya.Driver
{
    public class EmptyDriver : IDriver
    {
        public static EmptyDriver Instance = new();

        #region Constructors

        private EmptyDriver()
        {
        }

        #endregion

        #region Implementation of IDriver

        public bool Read(ulong address, byte[] buffer, int count)
        {
            return true;
        }

        public bool Write(ulong address, byte[] buffer, int count)
        {
            return true;
        }

        #endregion
    }
}