using MeuDicionarioV2.Core.Enums;
using MeuDicionarioV2.Infra.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class ConjugationConfigurations : IEntityTypeConfiguration<Conjugation>
    {

        public void Configure(EntityTypeBuilder<Conjugation> builder)
        {
            builder.ToTable("Conjugations");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.ConjugationItSelf)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(e => e.ConjugationType)
                .HasConversion(new EnumToStringConverter<ConjugationType>());
        }
    }
}
