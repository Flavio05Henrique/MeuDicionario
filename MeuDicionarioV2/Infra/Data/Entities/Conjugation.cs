using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;

namespace MeuDicionarioV2.Infra.Data.Entities
{
    public class Conjugation
    {
        public int Id { get; set; }
        public Word Word { get; set; }
        public int WordId { get; set; }
        public string ConjugationItSelf { get; set; }
        public ConjugationType ConjugationType { get; set; }
    }
}
