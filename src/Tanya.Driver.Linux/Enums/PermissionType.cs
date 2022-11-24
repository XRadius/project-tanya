namespace Tanya.Driver.Linux.Enums
{
    [Flags]
    public enum PermissionType
    {
        None,
        Read,
        Write,
        Execute,
        Shared
    }
}