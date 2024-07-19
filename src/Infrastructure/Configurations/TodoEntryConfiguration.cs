using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class TodoEntryConfiguration : IEntityTypeConfiguration<TodoEntry>
    {
        public void Configure(EntityTypeBuilder<TodoEntry> builder)
        {
            builder.ToTable("TodoEntries");
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Todo).Property(p => p.Value).HasColumnName("Todo");
            builder.Property(x => x.CreatedDateTime).IsRequired();
            builder.Property(x => x.ModifiedDateTime);
        }
    }
}
