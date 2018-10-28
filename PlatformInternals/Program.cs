using System;
using System.IO;
using Task1_2.ResourceManagement;

namespace PlatformInternals
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".tmp");

            using (var managed = new ManagedToFileWriter(filePath))
            {
                managed.WriteToFile();
            }
            ReadFile(filePath);

            using (var unmanaged = new UnmanagedToFileWriter(filePath))
            {
                unmanaged.WriteToFile();
            }

            ReadFile(filePath);
            
            Console.ReadKey();
        }

        public static void ReadFile(string path)
        {
            Console.WriteLine(path + ": " + File.ReadAllText(path));
        }
    }
}
