using Tanya.Driver.Linux.Models;

namespace Tanya.Driver.Linux.Utilities
{
    public static class ProcessEntryParser
    {
        #region Statics

        public static ProcessEntry? Parse(string value, int pid)
        {
            var pieces = value.Split('\0', StringSplitOptions.RemoveEmptyEntries);
            if (pieces.Length != 0) return new ProcessEntry(pieces.Skip(1), pieces[0], pid);
            return null;
        }

        #endregion
    }
}