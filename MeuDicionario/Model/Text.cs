using MeuDicionario.Infra;
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

           WordsInText = string.Join(",", listaDePalavras);
        }

        public void SetRelationTextWord(MyDictionaryContex contex)
        {
            var wordsInText = WordsInText.Split(",");
            var validWors = new List<Word>();

            var textWordList = contex.TextWords.Where(e => e.TextRef.Id == this.Id);
            contex.TextWords.RemoveRange(textWordList);
            contex.SaveChanges();
            
            foreach (var word in wordsInText)
            {
                var item = contex.Words.FirstOrDefault(e => e.Name.ToLower().Equals(word.ToLower()));
                if(item != null)
                {
                    if(!contex.TextWords.Any(e => e.TextRef.Id == this.Id && e.WordRef.Id == item.Id))
                    {
                        contex.TextWords.Add(new TextWord(this, item));
                    }
                }
            }

            contex.SaveChanges();
        }

        public void ClearRelationTextWord(MyDictionaryContex contex, int id)
        {
            var textWordList = contex.TextWords.Where(e => e.TextRef.Id == id);
            contex.TextWords.RemoveRange(textWordList);
        }
    }
}
