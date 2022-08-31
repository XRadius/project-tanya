using Tanya.Driver.Linux.Models;
using Tanya.Driver.Linux.Utilities;

namespace Tanya.Driver.Linux
{
    public class Linux
    {
        #region Methods

        public async IAsyncEnumerable<MapEntry> MapsAsync(int pid)
        {
            var value = await SafeReadTextAsync($"/proc/{pid}/maps")
                .ContinueWith(x => x.Result.Split(Environment.NewLine))
                .ConfigureAwait(false);
            foreach (var entry in value)
            {
                var map = MapEntryParser.Parse(entry);
                if (map == null) continue;
                yield return map;
            }
        }

        public async IAsyncEnumerable<ProcessEntry> ProcessesAsync()
        {
            foreach (var path in SafeEnumerateDirectories("/proc"))
            {
                if (int.TryParse(Path.GetFileName(path), out var pid))
                {
                    var process = ProcessEntryParser.Parse(await SafeReadTextAsync($"/proc/{pid}/cmdline").ConfigureAwait(false), pid);
                    if (process == null) continue;
                    yield return process;
                }
            }
        }

        #endregion

        #region Statics

        private static IEnumerable<string> SafeEnumerateDirectories(string path)
        {
            try
            {
                return Directory.EnumerateDirectories(path);
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
        }

        private static async Task<string> SafeReadTextAsync(string path)
        {
            try
            {
                return await File.ReadAllTextAsync(path).ConfigureAwait(false);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion
    }
}