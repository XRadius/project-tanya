using System.Runtime.CompilerServices;
using Tanya.Core.Interfaces;
using Tanya.Core.Models;

namespace Tanya.Core.Types
{
    public class VectorType : IType<Vector>
    {
        public static readonly VectorType Instance = new();

        #region Constructors

        private VectorType()
        {
        }

        #endregion

        #region Implementation of IType<Vector>

        public Vector Get(byte[] buffer)
        {
            var x = Unsafe.ReadUnaligned<float>(ref buffer[0]);
            var y = Unsafe.ReadUnaligned<float>(ref buffer[4]);
            var z = Unsafe.ReadUnaligned<float>(ref buffer[8]);
            return new Vector(x, y, z);
        }

        public void Set(byte[] buffer, Vector value)
        {
            Unsafe.As<byte, float>(ref buffer[0]) = value.X;
            Unsafe.As<byte, float>(ref buffer[4]) = value.Y;
            Unsafe.As<byte, float>(ref buffer[8]) = value.Z;
        }

        public int Size()
        {
            return sizeof(float) * 3;
        }

        #endregion
    }
}