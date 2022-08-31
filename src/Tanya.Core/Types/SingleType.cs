using System.Runtime.CompilerServices;
using Tanya.Core.Interfaces;

namespace Tanya.Core.Types
{
    public class SingleType : IType<float>
    {
        public static readonly SingleType Instance = new();

        #region Constructors

        private SingleType()
        {
        }

        #endregion

        #region Implementation of IType<float>

        public float Get(byte[] buffer)
        {
            return Unsafe.ReadUnaligned<float>(ref buffer[0]);
        }

        public void Set(byte[] buffer, float value)
        {
            Unsafe.As<byte, float>(ref buffer[0]) = value;
        }

        public int Size()
        {
            return sizeof(float);
        }

        #endregion
    }
}