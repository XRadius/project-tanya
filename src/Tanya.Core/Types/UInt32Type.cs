using System.Runtime.CompilerServices;
using Tanya.Core.Interfaces;

namespace Tanya.Core.Types
{
    public class UInt32Type : IType<uint>
    {
        public static readonly UInt32Type Instance = new();

        #region Constructors

        private UInt32Type()
        {
        }

        #endregion

        #region Implementation of IType<uint>

        public uint Get(byte[] buffer)
        {
            return Unsafe.ReadUnaligned<uint>(ref buffer[0]);
        }

        public void Set(byte[] buffer, uint value)
        {
            Unsafe.As<byte, uint>(ref buffer[0]) = value;
        }

        public int Size()
        {
            return sizeof(uint);
        }

        #endregion
    }
}