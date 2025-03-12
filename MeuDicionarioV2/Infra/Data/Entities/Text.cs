using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MeuDicionariov2.Infra.Data.Entities
{
    public class Text
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string  TextItSelf{ get; set; }
        public string WordsInText { get; set; }
    }
}
