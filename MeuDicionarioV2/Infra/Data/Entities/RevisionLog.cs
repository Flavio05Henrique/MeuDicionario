using System.ComponentModel.DataAnnotations;

namespace MeuDicionariov2.Infra.Data.Entities
{
    public class RevisionLog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
