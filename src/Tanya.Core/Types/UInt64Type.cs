using System.Runtime.CompilerServices;
using Tanya.Core.Interfaces;

namespace Tanya.Core.Types
{
    public class UInt64Type : IType<ulong>
    {
        public static readonly UInt64Type Instance = new();

        #region Constructors

        private UInt64Type()
        {
        }

        #endregion

        #region Implementation of IType<ulong>

        public ulong Get(byte[] buffer)
        {
            return Unsafe.ReadUnaligned<ulong>(ref buffer[0]);
        }

        public void Set(byte[] buffer, ulong value)
        {
            Unsafe.As<byte, ulong>(ref buffer[0]) = value;
        }

        public int Size()
        {
            return sizeof(ulong);
        }

        #endregion
    }
}