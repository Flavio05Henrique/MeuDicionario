namespace MeuDicionarioV2.Utilitys
{
    public static class StringFormat
    {
        public static string ToUperFirstChar(string str)
        {
            var strToLwer = str.ToLower();
            var strFomatted = char.ToUpper(strToLwer[0]) + strToLwer.Substring(1);
            return strFomatted;
        }
    }
}
