using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class WordConfigurations : IEntityTypeConfiguration<Word>
    {

        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.ToTable("Words");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(e => e.Name)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(e => e.Meaning)
                .HasMaxLength(550)
                .IsRequired();

            builder.Property(e => e.CrationDate)
                .IsRequired();

            builder.Property(e => e.WordType)
                .HasConversion(new EnumToStringConverter<WordType>());

            builder.Property(e => e.Revision)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.RevisionGap)
                .HasDefaultValue(3)
                .IsRequired();

            builder.Property(e => e.RevisionScore)
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasMany(e => e.Conjugations)
                .WithOne(e => e.Word)
                .HasForeignKey(e => e.WordId);
        }
    }
}
