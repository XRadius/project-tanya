using System.Globalization;
using System.Text.RegularExpressions;
using Tanya.Driver.Linux.Models;

namespace Tanya.Driver.Linux.Utilities
{
    public static class MapEntryParser
    {
        private static readonly Regex Me = new(
            @"^([0-9A-F]+)-([0-9A-F]+)\s+(r|-)(w|-)(x|-)(p|s)\s+([0-9A-F]+)\s+([0-9A-F]+):([0-9A-F]+)\s+([0-9]+)\s+(.*)$",
            RegexOptions.IgnoreCase);

        #region Statics

        public static MapEntry? Parse(string value)
        {
            var match = Me.Match(value);
            if (match.Success) return Map(match);
            return null;
        }

        private static MapEntry Map(Match match)
        {
            var start = ulong.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
            var end = ulong.Parse(match.Groups[2].Value, NumberStyles.HexNumber);
            var perms = Permissions(match);
            var offset = ulong.Parse(match.Groups[7].Value, NumberStyles.HexNumber);
            var devMajor = ushort.Parse(match.Groups[8].Value, NumberStyles.HexNumber);
            var devMinor = ushort.Parse(match.Groups[9].Value, NumberStyles.HexNumber);
            var inode = ulong.Parse(match.Groups[10].Value);
            var pathname = match.Groups[11].Value;
            return new MapEntry(devMajor, devMinor, end, inode, offset, pathname, perms, start);
        }

        private static MapEntryPermissions Permissions(Match match)
        {
            var result = MapEntryPermissions.None;
            if (match.Groups[3].Value == "r") result |= MapEntryPermissions.Read;
            if (match.Groups[4].Value == "w") result |= MapEntryPermissions.Write;
            if (match.Groups[5].Value == "x") result |= MapEntryPermissions.Execute;
            if (match.Groups[6].Value == "s") result |= MapEntryPermissions.Shared;
            return result;
        }

        #endregion
    }
}