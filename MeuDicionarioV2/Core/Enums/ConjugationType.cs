using System.ComponentModel;

namespace MeuDicionarioV2.Core.Enums
{
    public enum ConjugationType
    {
        [Description("ThirdPerson")]
        ThirdPerson,
        [Description("Preterite")]
        Preterite,
        [Description("PresentContinuous")]
        PresentContinuous,
        [Description("PaticiplePresent")]
        PaticiplePresent,
        [Description("PaticiplePass")]
        PaticiplePass,
        [Description("Plural")]
        Plural
    }
}
