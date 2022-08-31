namespace Tanya.Game.Apex.Core.Interfaces
{
    public interface IFeature
    {
        #region Methods

        void Tick(DateTime frameTime, State state);

        #endregion
    }
}