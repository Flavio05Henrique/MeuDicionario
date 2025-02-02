using MeuDicionario.Model;

namespace MeuDicionario.Infra.DALs
{
    public class RevisionDAL : DAL<Revision>
    {
        public RevisionDAL(MyDictionaryContex context) : base(context)
        {
        }
    }
}
