using MeuDicionariov2.Infra.Data.Entities;
using MeuDicionarioV2.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;

namespace MeuDicionarioV2.Infra.Data.Configurations
{
    public class RevisionLogConfigurations : IEntityTypeConfiguration<RevisionLog>
    {

        public void Configure(EntityTypeBuilder<RevisionLog> builder)
        {
            builder.ToTable("RevisionLogs");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn();

            builder.Property(e => e.Date);
        }
    }
}
