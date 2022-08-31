namespace Tanya.Game.Apex.Feature.Aim.Utilities
{
    public static class Normalize
    {
        #region Statics

        public static float X(float x)
        {
            while (x > 89.0)
            {
                x -= 180;
            }

            while (x < -89.0)
            {
                x += 180;
            }

            return x;
        }

        public static float Y(float y)
        {
            while (y > 180)
            {
                y -= 360;
            }

            while (y < -180)
            {
                y += 360;
            }

            return y;
        }

        #endregion
    }
}