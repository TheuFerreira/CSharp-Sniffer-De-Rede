using SnifferDeRede.Repository.Interfaces;
using System.IO;

namespace SnifferDeRede.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly string path;

        public FileRepository(string path)
        {
            this.path = path;
        }

        public void WriteInfo(object obj)
        {
            string line = string.Format("{0}\n", obj);
            File.AppendAllText(path, line);
        }
    }
}
