using Tanya.Core.Interfaces;
using Tanya.Driver.Interfaces;

namespace Tanya.Core.Extensions
{
    public static class DriverExtensions
    {
        #region Statics

        public static Access<T> Access<T>(this IDriver driver, ulong address, IType<T> type)
        {
            return new Access<T>(driver, address, type);
        }

        public static Access<T> Access<T>(this IDriver driver, ulong address, IType<T> type, uint interval)
        {
            return new Access<T>(driver, address, type, interval);
        }

        #endregion
    }
}