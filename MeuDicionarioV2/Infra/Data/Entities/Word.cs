using MeuDicionario.Model;
using MeuDicionarioV2.Core.Enums;
using MeuDicionarioV2.Infra.Data.Entities;

namespace MeuDicionariov2.Infra.Data.Entities
{
    public class Word
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public string Name { get;  set; }
        public string Meaning { get; set; }
        public DateTime CrationDate { get; set; }
        public DateTime LastSeen { get; set; }
        public WordType WordType { get; set; }
        public bool IsRegular { get; set; }
        public bool Revision {  get; set; }
        public int RevisionGap { get; set; }
        public int RevisionScore { get; set; }
        public List<Conjugation>? Conjugations { get; set; }
    }
}
