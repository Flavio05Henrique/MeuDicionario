using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class TextConfigurations : IEntityTypeConfiguration<Text>
    {

        public void Configure(EntityTypeBuilder<Text> builder)
        {
            builder.ToTable("Texts");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.Title)
                .HasMaxLength(120)
                .IsRequired();

            builder.Property(e => e.TextItSelf)
                .IsRequired();

            builder.Property(e => e.WordsInText)
                .IsRequired();
        }
    }
}
