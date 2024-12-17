using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model.DTOs
{
    public class WordCreate
    {

        [Required]
        [StringLength(255)]
        public string Name { get; private set; }
        [Required]
        [StringLength(1000)]
        public string Meaning { get; set; }

        public WordCreate(string name, string meaning)
        {
            Name = name;
            Meaning = meaning;
        }
    }
}
