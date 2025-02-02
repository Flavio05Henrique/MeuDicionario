using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra.DALs
{
    public class TextWordDAL : DAL<TextWord>
    {
        public TextWordDAL(MyDictionaryContex context) : base(context)
        {
        }
    }
}
