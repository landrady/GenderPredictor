using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameProcessor.Infra
{
    public class FileWrapper
    {
        public async Task Save(string data, string path, bool append)
        {
            using (StreamWriter outfile = new StreamWriter(path, append))
            {
               await outfile.WriteLineAsync(data);
            }
        }

        public async Task<string> GetData(string path)
        {
            string result;
            using (StreamReader sr = new StreamReader(path))
            {
                result = await sr.ReadToEndAsync();
            }
            return result;
        }

        public async Task<string[]> GetDataLines(string path)
        {
            string[] result;
            using (StreamReader sr = new StreamReader(path))
            {
                string all = await sr.ReadToEndAsync();
                result = all.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }
            return result;
        }
    }
}
