using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model
{
    public class RevisionV3
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Word")]
        public Word WordRef { get; set; }

        public RevisionV3()
        {
        }
        public RevisionV3(Word wordRef)
        {
            //DateTime = DateTime.Now;
            WordRef = wordRef;
        }
    }
}
