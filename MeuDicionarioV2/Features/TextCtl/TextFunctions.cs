using MeuDicionariov2.Infra.Data;
using MeuDicionariov2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace MeuDicionarioV2.Features.TextCtl
{
    public static class TextFunctions
    {
        public static string SearchAllWordsInText(string TextItSelf)
        {
            var words = Regex.Matches(TextItSelf, @"\b[a-zA-Zá-úÁ-ÚçÇ]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .ToArray();

            var listaDePalavras = new List<string>();

            foreach (var word in words)
            {
                var wordFormated = word.ToLower().Trim();

                if (!listaDePalavras.Contains(wordFormated))
                {
                    listaDePalavras.Add(wordFormated);
                }
            }

            return string.Join(",", listaDePalavras);
        }

        public static async Task SetRelationTextWord(MyDictionaryDbContex contex, Text text, CancellationToken cancellationToken)
        {
            var wordsInText = text.WordsInText.ToLower().Split(",");

            await ClearRelationTextWord(contex, text, cancellationToken);

            var itens = await contex.Words
                .Where(e => wordsInText.Contains(e.Name.ToLower()))
                .Select(e => new TextWord() { Text = text, Word = e})
                .ToListAsync(cancellationToken);

            await contex.TextWords.AddRangeAsync(itens, cancellationToken);

            await contex.SaveChangesAsync(cancellationToken);
        }

        public static async Task ClearRelationTextWord(MyDictionaryDbContex contex, Text text, CancellationToken cancellationToken)
        {
            var textWordList = await contex.TextWords.Where(e => e.Text.Id == text.Id).ToListAsync(cancellationToken);

            if (!(textWordList.Count() > 0)) return;

            contex.TextWords.RemoveRange(textWordList);
            await contex.SaveChangesAsync(cancellationToken);
        }
    }
}
