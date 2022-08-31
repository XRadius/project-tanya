using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Tanya.Driver.Linux;
using Tanya.Game.Apex;
using Tanya.Logging;

namespace Tanya
{
    public static class Startup
    {
        #region Statics

        public static void ConfigureLogging(ILoggingBuilder builder)
        {
            builder.ClearProviders();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, LoggerProvider>());
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Linux>();
            Bootstrap.ConfigureServices(services);
        }

        #endregion
    }
}