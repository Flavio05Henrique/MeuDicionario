using MeuDicionario.Model;
using MeuDicionariov2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class RevisionLogConfigurations : IEntityTypeConfiguration<RevisionLog>
    {

        public void Configure(EntityTypeBuilder<RevisionLog> builder)
        {
            builder.ToTable("RevisionLogs");

            builder.HasKey(e => e.WordId);

            builder.Property(e => e.Time)
                .IsRequired();

            builder.Property(e => e.Correct)
                .IsRequired();

            builder.Property(e => e.Date)
                .IsRequired();

            builder.HasOne(e => e.Word)
                .WithMany()
                .HasForeignKey(e => e.WordId);
        }
    }
}
