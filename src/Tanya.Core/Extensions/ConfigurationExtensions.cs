using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Tanya.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        #region Statics

        public static T GetProperty<T>(this IConfiguration config, [CallerMemberName] string? key = null)
        {
            return config.GetValue<T>(key);
        }

        #endregion
    }
}