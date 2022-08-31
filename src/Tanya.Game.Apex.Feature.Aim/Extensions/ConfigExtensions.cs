using Tanya.Core.Models;
using Tanya.Game.Apex.Feature.Aim.Utilities;

namespace Tanya.Game.Apex.Feature.Aim.Extensions
{
    public static class ConfigExtensions
    {
        #region Statics

        public static Vector GetSmoothAngle(this Config config, Vector a, Vector b, float p)
        {
            var dx = Normalize.X(a.X - b.X);
            var dy = Normalize.Y(a.Y - b.Y);
            var sx = a.X - dx * p * config.PitchSpeed;
            var sy = a.Y - dy * p * config.YawSpeed;
            return new Vector(Normalize.X(sx), Normalize.Y(sy), 0);
        }

        #endregion
    }
}