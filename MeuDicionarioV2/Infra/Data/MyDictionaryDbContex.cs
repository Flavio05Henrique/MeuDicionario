using MeuDicionario.Model;
using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Infra.Data.Configurations;
using MeuDicionarioV2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MeuDicionariov2.Infra.Data
{
    public class MyDictionaryDbContex : DbContext
    {
       private IConfiguration _configuration;

        public MyDictionaryDbContex(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<Revision> Revisions {  get; set; }
        public DbSet<RevisionLog> RevisionLogs {  get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<TextWord> TextWords { get; set; }
        public DbSet<Conjugation> conjugations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbString = _configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(dbString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WordConfigurations());
            modelBuilder.ApplyConfiguration(new TextConfigurations());
            modelBuilder.ApplyConfiguration(new RevisionConfigurations());
            modelBuilder.ApplyConfiguration(new RevisionLogConfigurations());
            modelBuilder.ApplyConfiguration(new TextWordConfigurations());
            modelBuilder.ApplyConfiguration(new ConjugationConfigurations());
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Remove(typeof(ForeignKeyIndexConvention));
        }
    }
}
