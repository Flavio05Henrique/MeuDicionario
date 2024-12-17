using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MeuDicionario.Model
{
    public class Word
    {

        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; private set; }
        [Required]
        [StringLength(1000)]
        public string Meaning { get; set; }
        public DateTime LastSeen { get; private set; }
       
        public Word(string name, string meaning)
        {
            Name = WordFormatted(name);
            Meaning = meaning;
            LastSeen = DateTime.Now;
        }

        private string WordFormatted(string word)
        {
            var wordLower = word.ToLower();
            var wordFomatted = char.ToUpper(wordLower[0]) + wordLower.Substring(1);
            return wordFomatted;
        }
    }
}
