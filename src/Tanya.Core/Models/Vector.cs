using System.Text.Json.Serialization;

namespace Tanya.Core.Models
{
    public record Vector
    {
        public static Vector Origin = new(0, 0, 0);

        #region Constructors

        public Vector(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Methods

        public float Distance(Vector otherVector)
        {
            var x = X - otherVector.X;
            var y = Y - otherVector.Y;
            return MathF.Sqrt(x * x + y * y);
        }

        #endregion

        #region Properties

        [JsonPropertyName("x")]
        public float X { get; }

        [JsonPropertyName("y")]
        public float Y { get; }

        [JsonPropertyName("z")]
        public float Z { get; }

        #endregion

        #region Statics

        public static Vector operator +(Vector a, Vector b)
        {
            var x = a.X + b.X;
            var y = a.Y + b.Y;
            var z = a.Z + b.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator +(Vector a, float b)
        {
            var x = a.X + b;
            var y = a.Y + b;
            var z = a.Z + b;
            return new Vector(x, y, z);
        }

        public static Vector operator /(Vector a, Vector b)
        {
            var x = a.X / b.X;
            var y = a.Y / b.Y;
            var z = a.Z / b.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator /(Vector a, float b)
        {
            var x = a.X / b;
            var y = a.Y / b;
            var z = a.Z / b;
            return new Vector(x, y, z);
        }

        public static Vector operator *(Vector a, Vector b)
        {
            var x = a.X * b.X;
            var y = a.Y * b.Y;
            var z = a.Z * b.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator *(Vector a, float b)
        {
            var x = a.X * b;
            var y = a.Y * b;
            var z = a.Z * b;
            return new Vector(x, y, z);
        }

        public static Vector operator -(Vector a, Vector b)
        {
            var x = a.X - b.X;
            var y = a.Y - b.Y;
            var z = a.Z - b.Z;
            return new Vector(x, y, z);
        }

        public static Vector operator -(Vector a, float b)
        {
            var x = a.X - b;
            var y = a.Y - b;
            var z = a.Z - b;
            return new Vector(x, y, z);
        }

        #endregion

        #region Overrides of object

        public override string ToString()
        {
            return $"({X:F},{Y:F},{Z:F})";
        }

        #endregion
    }
}