using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model
{
    public class TextWord
    {

        [Key]
        public int Id { get; set; }

        public Text TextRef { get; private set; }
        public Word WordRef { get; private set; }

        public TextWord()
        {
        }
        public TextWord(Text text, Word word)
        {
            TextRef = text;
            WordRef = word;
        }
       

    }
}
