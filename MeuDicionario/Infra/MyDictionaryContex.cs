using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra
{
    public class MyDictionaryContex : DbContext
    {
        public DbSet<Word> Words { get; set; }

        private string dbString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MeuDicionario;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(dbString);
        }
    }
}
