using MeuDicionario.Model;
using Microsoft.EntityFrameworkCore;

namespace MeuDicionario.Infra
{
    public class MyDictionaryContex : DbContext
    {
        public DbSet<Word> Words { get; set; }
        public DbSet<RevisionV3> RevisionV3 {  get; set; }
        public DbSet<RevisionLog> RevisionLogs {  get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<TextWord> TextWords { get; set; }

        private string dbString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MeuDicionario;" +
            "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;" +
            "Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(dbString);
        }
    }
}
