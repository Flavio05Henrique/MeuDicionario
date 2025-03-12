using MeuDicionario.Model;
using MeuDicionariov2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class RevisionConfigurations : IEntityTypeConfiguration<Revision>
    {

        public void Configure(EntityTypeBuilder<Revision> builder)
        {
            builder.ToTable("Revisions");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.HasOne(e => e.Word)
                .WithMany()
                .HasForeignKey(e => e.WordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
