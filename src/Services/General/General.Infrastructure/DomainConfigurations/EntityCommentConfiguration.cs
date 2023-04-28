using General.Domain.Entities;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class EntityCommentConfiguration : WatcherConfiguration, IEntityTypeConfiguration<Comment>
{ 
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(Common.CommentTable);
        builder.HasKey(_ => _.Id);

        builder.Property<int>("PostId");

        builder.HasOne<Post>()
            .WithMany(_ => _.Comments)
            .HasForeignKey("PostId");

        builder.Property(_ => _.Creator).IsRequired();

        builder.OwnsOne(_ => _.Description,
            _ =>
            {
                _.Property(c => c.Description).HasColumnName("Description").IsRequired();

            });

        builder.Property(_ => _.Version).HasDefaultValue(1);
        builder.Ignore(_ => _.Events);

        this.ConfigureWatcher(builder);
    }
}