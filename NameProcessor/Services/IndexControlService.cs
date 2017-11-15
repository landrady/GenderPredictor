using NameProcessor.Infra;
using System.IO;
using System.Threading.Tasks;

namespace NameProcessor.Services
{
    public class IndexControlService
    {
        private readonly FileWrapper _fileWrapper;

        private readonly string _indexControlPath;

        private int _lastIdCache;

        public IndexControlService(FileWrapper fileWrapper, string indexControlPath)
        {
            this._indexControlPath = indexControlPath;
            this._fileWrapper = fileWrapper;
            _lastIdCache = 0;
        }

        public async Task SaveLast(int value)
        {
            if (value > this._lastIdCache)
            {
                this._lastIdCache = value;
                await _fileWrapper.Save(value.ToString(), _indexControlPath, false);
            }
        }

        public async Task<int> GetLast()
        {
            if (_lastIdCache == 0)
            {
                await UpdateIdFromFile();
            }
            return _lastIdCache;
        }

        private async Task UpdateIdFromFile()
        {
            if (File.Exists(_indexControlPath))
            {
                string result = await _fileWrapper.GetData(_indexControlPath);
                int.TryParse(result, out _lastIdCache);
            }
        }
    }
}
