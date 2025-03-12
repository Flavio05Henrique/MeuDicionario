using System.ComponentModel.DataAnnotations;

namespace MeuDicionariov2.Infra.Data.Entities
{
    public class TextWord
    {
        public int Id { get; set; }
        public Text Text { get;  set; }
        public Word Word { get;  set; }
        public int TextId { get; set; }
        public int WordId { get; set; }
    }
}
