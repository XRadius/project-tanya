using Tanya.Core.Models;
using Tanya.Game.Apex.Feature.Aim.Utilities;

namespace Tanya.Game.Apex.Feature.Aim.Extensions
{
    public static class VectorExtensions
    {
        #region Statics

        public static Vector GetDesiredAngle(this Vector a, Vector b)
        {
            var d = b - a;
            var h = MathF.Sqrt(d.Y * d.Y + d.X * d.X);
            var x = MathF.Atan2(d.Z, h) * 180 / MathF.PI * -1;
            var y = MathF.Atan2(d.Y, d.X) * 180 / MathF.PI;
            return new Vector(Normalize.X(x), Normalize.Y(y), 0);
        }

        #endregion
    }
}