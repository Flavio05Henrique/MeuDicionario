using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra
{
    public class WordDAL : DAL<Word>
    {
        public WordDAL(MyDictionaryContex context) : base(context)
        {
        }
    }
}
