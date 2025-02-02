using MeuDicionario.Infra.DALs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MeuDicionario.Model
{
    public class Text
    {
        [Key]
        public int Id { get; set; }
        [StringLength(500)]
        public string Title { get; private set; }
        public string TextItSelf { get; private set; }
        public string WordsInText { get; private set; }
        public Text(string title, string textItSelf)
        {
            Title = title;
            TextItSelf = textItSelf;
        }

        public void SearchAllWordsInText()
        {
            var words = Regex.Matches(TextItSelf, @"\b[a-zA-Zá-úÁ-ÚçÇ]+\b")
                                .Cast<Match>()
                                .Select(m => m.Value)
                                .ToArray();

            var listaDePalavras = new List<string>();

            foreach (var word in words)
            {
                var wordFormated = word.ToLower().Trim();
             
                if(!listaDePalavras.Contains(wordFormated))
                {
                    listaDePalavras.Add(wordFormated);
                }
            }

            foreach (var lp in listaDePalavras)
            {
                Console.WriteLine(lp);
            }

           WordsInText = string.Join(",", listaDePalavras);
        }

        public void SetRelationTextWord(WordDAL wordDAL, TextWordDAL textWordDAL)
        {
            var wordsInText = WordsInText.Split(",");
            var validWors = new List<Word>();

            var textWordList = textWordDAL.FindBySome(e => e.TextRef.Id == this.Id);
            textWordDAL.RemoveRange(textWordList);
            
            foreach (var word in wordsInText)
            {
                var item = wordDAL.FindBy(e => e.Name.ToLower().Equals(word.ToLower()));
                if(item != null)
                {
                    if(!textWordDAL.Has(e => e.TextRef.Id == this.Id && e.WordRef.Id == item.Id))
                    {
                        textWordDAL.Add(new TextWord(this, item));
                    }
                }
            }
        }

        public void ClearRelationTextWord(TextWordDAL textWordDAL, int id)
        {
            var textWordList = textWordDAL.FindBySome(e => e.TextRef.Id == id);
            textWordDAL.RemoveRange(textWordList);
        }
    }
}
