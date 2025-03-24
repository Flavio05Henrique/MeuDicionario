using System.ComponentModel;

namespace MeuDicionarioV2.Core.Enums
{
    public enum WordType
    {
        [Description("Noun")]
        Noun,
        [Description("Verb")]
        Verb,
        [Description("Adjective")]
        Adjective,
        [Description("Adverb")]
        Adverb,
        [Description("Preposition")]
        Preposition,
        [Description("Conjunction")]
        Conjunction
    }
}
