namespace Tanya.Driver.Interfaces
{
    public interface IDriver
    {
        #region Methods

        bool Read(ulong address, byte[] buffer, int count);

        bool Write(ulong address, byte[] buffer, int count);

        #endregion
    }
}