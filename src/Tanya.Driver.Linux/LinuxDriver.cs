using Tanya.Driver.Interfaces;
using Tanya.Driver.Linux.Interop;

namespace Tanya.Driver.Linux
{
    public class LinuxDriver : IDriver
    {
        private readonly int _pid;

        #region Constructors

        public LinuxDriver(int pid)
        {
            _pid = pid;
        }

        #endregion

        #region Implementation of IDriver

        public unsafe bool Read(ulong address, byte[] buffer, int count)
        {
            fixed (void* pointer = buffer)
            {
                var localIo = new Iovec { iov_base = pointer, iov_len = count };
                var remoteIo = new Iovec { iov_base = (void*)address, iov_len = count };
                return Libc.process_vm_readv(_pid, &localIo, 1, &remoteIo, 1, 0) == count;
            }
        }

        public unsafe bool Write(ulong address, byte[] buffer, int count)
        {
            fixed (void* pointer = buffer)
            {
                var localIo = new Iovec { iov_base = pointer, iov_len = count };
                var remoteIo = new Iovec { iov_base = (void*)address, iov_len = count };
                return Libc.process_vm_writev(_pid, &localIo, 1, &remoteIo, 1, 0) == count;
            }
        }

        #endregion
    }
}