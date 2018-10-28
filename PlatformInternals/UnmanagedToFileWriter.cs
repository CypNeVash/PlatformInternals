using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Task1_2.ResourceManagement
{
    public class UnmanagedToFileWriter : IDisposable
    {
        #region Import kernel32.dll functions

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CreateFile(
            [MarshalAs(UnmanagedType.LPTStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] FileAccess access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
            IntPtr templateFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WriteFile(
            IntPtr fileHandle,
            IntPtr buffer,
            int count,
            out int resultCount,
            IntPtr lpOverlapped);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle); 

        #endregion

        private IntPtr SomeFile { get; set; }
        public string FilePath { get; private set; }

        private bool _disposed = false;


        public UnmanagedToFileWriter()
        {
            FilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");

            SomeFile = CreateFile(
                filename: FilePath,
                access: FileAccess.Write,
                share: FileShare.Write,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Create,
                flagsAndAttributes: FileAttributes.Normal,
                templateFile: IntPtr.Zero);
        }

        public UnmanagedToFileWriter(string path)
        {
            FilePath = path;

            SomeFile = CreateFile(
                filename: FilePath,
                access: FileAccess.Write,
                share: FileShare.Write,
                securityAttributes: IntPtr.Zero,
                creationDisposition: FileMode.Create,
                flagsAndAttributes: FileAttributes.Normal,
                templateFile: IntPtr.Zero);
        }

        public void WriteToFile()
        {
            var bytes = Encoding.UTF8.GetBytes("Text written by unmanaged writer...");

            var bytesPointer = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, bytesPointer, bytes.Length);

            WriteFile(
                fileHandle: SomeFile,
                buffer: bytesPointer,
                count: bytes.Length,
                resultCount: out int bytesWritten,
                lpOverlapped: IntPtr.Zero);
        }

        public void Dispose(bool dispossing)
        {
            if (_disposed)
            {
                return;
            }

            CloseHandle(SomeFile);
            _disposed = true;
        }

        ~UnmanagedToFileWriter()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
