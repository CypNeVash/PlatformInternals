using System;
using System.IO;
using System.Text;

namespace Task1_2.ResourceManagement
{
    public class ManagedToFileWriter : IDisposable
    {
        private Stream SomeFile { get; set; }
        public string FilePath { get; private set; }
        private bool _disposed = false;

        public ManagedToFileWriter()
        {
            FilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");

            SomeFile = File.Create(FilePath);
        }

        public ManagedToFileWriter(string path)
        {
            FilePath = path;

            SomeFile = File.Create(FilePath);
        }

        public void WriteToFile()
        {
            var bytes = Encoding.UTF8.GetBytes("Text written by managed writer...");

            SomeFile.Write(
                buffer: bytes,
                offset: 0,
                count: bytes.Length);
        }
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            SomeFile.Dispose();
            _disposed = true;
        }
    }
}
