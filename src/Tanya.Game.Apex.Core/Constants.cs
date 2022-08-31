namespace Tanya.Game.Apex.Core
{
    public static class Constants
    {
        public const int EntityListInterval = 1000;
        public const int EntityListSizeFull = 65536 * 32;
        public const int EntityListSizePlayer = 64 * 32;
        public const float UnitToMeter = 0.0254f;
        public const int VisibilityTicks = (int)(TimeSpan.TicksPerMillisecond * 100);
    }
}