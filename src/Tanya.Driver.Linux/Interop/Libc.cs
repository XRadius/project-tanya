using System.Runtime.InteropServices;

namespace Tanya.Driver.Linux.Interop
{
    public static class Libc
    {
        #region Statics

        [DllImport("libc")]
        public static extern unsafe int process_vm_readv(int pid,
            Iovec* localIov, ulong liovcnt,
            Iovec* remoteIov, ulong riovcnt,
            ulong flags);

        [DllImport("libc")]
        public static extern unsafe int process_vm_writev(int pid,
            Iovec* localIov, ulong liovcnt,
            Iovec* remoteIov, ulong riovcnt,
            ulong flags);

        #endregion
    }
}