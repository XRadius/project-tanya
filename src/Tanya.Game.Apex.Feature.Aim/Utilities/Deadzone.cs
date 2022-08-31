using System.Text.Json.Serialization;
using Tanya.Core.Models;

namespace Tanya.Game.Apex.Feature.Aim.Utilities
{
    public class Deadzone
    {
        #region Constructors

        public Deadzone(Vector desiredAngle, float x, float y)
        {
            MinX = Normalize.X(desiredAngle.X - x);
            MinY = Normalize.Y(desiredAngle.Y - y);
            MaxX = Normalize.X(desiredAngle.X + x);
            MaxY = Normalize.Y(desiredAngle.Y + y);
        }

        #endregion

        #region Methods

        public Vector ToVector(Vector viewAngle)
        {
            var xa = Normalize.X(MinX - viewAngle.X);
            var xb = Normalize.X(MaxX - viewAngle.X);
            var ya = Normalize.Y(MinY - viewAngle.Y);
            var yb = Normalize.Y(MaxY - viewAngle.Y);
            var x = xa * xb < 0 && MathF.Abs(xa - xb) < 89 ? viewAngle.X : MathF.Abs(xa) < MathF.Abs(xb) ? MinX : MaxX;
            var y = ya * yb < 0 && MathF.Abs(ya - yb) < 180 ? viewAngle.Y : MathF.Abs(ya) < MathF.Abs(yb) ? MinY : MaxY;
            return new Vector(x, y, 0);
        }

        #endregion

        #region Properties

        [JsonPropertyName("maxX")]
        public float MaxX { get; set; }

        [JsonPropertyName("maxY")]
        public float MaxY { get; set; }

        [JsonPropertyName("minX")]
        public float MinX { get; set; }

        [JsonPropertyName("minY")]
        public float MinY { get; set; }

        #endregion
    }
}