using MeuDicionariov2.Infra.Data.Entities;

namespace MeuDicionario.Model
{
    public class RevisionLog
    {
        public Word Word { get; set; }
        public int WordId { get; set; }
        public TimeSpan Time    { get; set; }
        public bool Correct { get; set; }
        public DateTime Date { get; set; }
    }
}
