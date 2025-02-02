using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra.DALs
{
    public class TextDAL : DAL<Text>
    {
        public TextDAL(MyDictionaryContex context) : base(context)
        {
        }
    }
}
