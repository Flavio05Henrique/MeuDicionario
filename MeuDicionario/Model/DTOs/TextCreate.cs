using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model.DTOs
{
    public class TextCreate
    {
        [StringLength(500)]
        public string Title { get; private set; }
        public string TextItSelf { get; private set; }
        public TextCreate(string title, string textItSelf)
        {
            Title = title;
            TextItSelf = textItSelf;
        }
    }
}
