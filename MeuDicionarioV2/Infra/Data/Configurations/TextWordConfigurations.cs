using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class TextWordConfigurations : IEntityTypeConfiguration<TextWord>
    {

        public void Configure(EntityTypeBuilder<TextWord> builder)
        {
            builder.ToTable("TextsWords");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.HasOne(e => e.Text)
                .WithMany()
                .HasForeignKey(e => e.TextId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Word)
                .WithMany()
                .HasForeignKey(e => e.WordId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
