using MeuDicionario.Model;

namespace MeuDicionario.Infra.DALs
{
    public class RevisionLogDAL : DAL<RevisionLog>
    {
        public RevisionLogDAL(MyDictionaryContex context) : base(context)
        {
        }
    }
}
