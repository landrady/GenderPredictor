using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NameProcessor.Services
{
    public class TextCleanerService
    {
        public string RemoveSpecialCharactersAndNumbers(string text)
        {
            MatchCollection matchList;
            matchList = Regex.Matches(text.ToLowerInvariant(), "[a-záéíóúàèìòùãõâêîôûäëïöüç\\-]+");

            var tokens = matchList.Cast<Match>().Select(match => match.Value).ToArray();
            var cleaned = string.Join(" ", tokens);
            return cleaned;
        }

        public string RemoveAccents(string word)
        {
            if (word == null) return string.Empty;

            var replacementMaps = new Dictionary<string, string[]>()
            {
                {"a", new string[]{ "ã", "á", "à", "ä", "â"}},
                {"e", new string[]{ "é", "è", "ê", "ë"}},
                {"i", new string[]{ "í", "ì", "î", "ï", "î"}},
                {"o", new string[]{ "õ", "ó", "ò", "ö", "ô"}},
                {"u", new string[]{ "ú", "ù", "û", "ü", "û"}},
                {"c", new string[]{ "ç"}},
            };


            var result = word.ToLowerInvariant();
            foreach (var replacementMap in replacementMaps)
            {
                foreach (var replaceable in replacementMap.Value)
                {
                    result = result.Replace(replaceable, replacementMap.Key);
                }
            }

            return result;
        }
    }
}
