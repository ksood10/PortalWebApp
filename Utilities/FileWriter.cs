namespace PortalWebApp.Areas.Utilities
{
    public class FileWriter
    {
        private readonly System.IO.StreamWriter fileWriter;
        internal string FileName { get; set; }

        public FileWriter(string fileName)
        {
            FileName = fileName;
            fileWriter = new System.IO.StreamWriter(fileName, true);
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
    }
}
