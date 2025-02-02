using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model
{
    public class Revision
    {

        [Key]
        public int Id { get; set; }
        public Word WordRef { get; private set; }
        
        public void SetAttributes(Word word)
        {
            WordRef = word;
        }
    }
}
