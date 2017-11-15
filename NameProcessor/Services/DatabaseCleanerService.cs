using System.Linq;

namespace NameProcessor.Services
{
    public class DatabaseCleanerService
    {
        private TextCleanerService _textCleanerService;
        public DatabaseCleanerService(TextCleanerService textCleanerService)
        {
            this._textCleanerService = textCleanerService;
        }
        public string[] Clean(string[] data)
        {
            var list = data.Select(x => _textCleanerService.RemoveAccents(_textCleanerService.RemoveSpecialCharactersAndNumbers(x)).Split(' ')[0]);
            return list.Distinct().ToArray();
        }

        public string[] CleanWithFrequency(string[] data)
        {
            var list = data.Select(x => _textCleanerService.RemoveAccents(_textCleanerService.RemoveSpecialCharactersAndNumbers(x)).Split(' ')[0]);
            return list.GroupBy(x => x).Select(x => new { name = x.Key, count = x.Key.Count() }).Where(x => x.count > 10).Select(x => x.name).OrderBy(x=>x).ToArray();
        }
    }
}
