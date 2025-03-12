using System.ComponentModel;

namespace MeuDicionarioV2.Core.Enums
{
    public enum ConjugationType
    {
        [Description("ThirdPerson")]
        ThirdPerson,
        [Description("Past")]
        Past,
        [Description("Plural")]
        Plural,
        [Description("Participle")]
        Participle,
        [Description("Gerundio")]
        Gerundio
    }
}
