using System.IO;
namespace PortalWebApp.Areas.Utilities
{
    public class FileWriter
    {
        private readonly StreamWriter fileWriter;
        internal string FileName { get; set; }

        public FileWriter(string fileName)
        {
            this.FileName = fileName;
            fileWriter = new StreamWriter(fileName, true);
        }

        public void Write(string line)
        {
            fileWriter.WriteLine(line);
        }

        public void Close()
        {
            fileWriter.Close();
            fileWriter.Dispose();
        }

        public void Delete(string fileName)
        {
            FileInfo myFile = new FileInfo(fileName);
            myFile.Delete();
        }
    }
}
