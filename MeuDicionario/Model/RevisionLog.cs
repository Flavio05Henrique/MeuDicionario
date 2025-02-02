using System.ComponentModel.DataAnnotations;

namespace MeuDicionario.Model
{
    public class RevisionLog
    {

        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public RevisionLog(DateTime date)
        {
            Date = date;
        }
    }
}
