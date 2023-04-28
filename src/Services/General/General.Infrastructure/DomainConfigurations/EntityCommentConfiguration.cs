using General.Domain.Entities;
using General.Domain.ValueObjects;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class EntityCommentConfiguration : IEntityTypeConfiguration<Comment>
{ 
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable(Common.CommentTable);
        builder.HasKey(_ => _.Id);

        builder.HasOne<Post>()
            .WithMany(_ => _.Comments)
            .HasForeignKey("PostId");

        builder.Property(_ => _.Creator).IsRequired();

        builder.OwnsOne<Content>(Common.NavigationContent,
            _ =>
            {
                _.Property(c => c.Description).HasColumnName("Comment").IsRequired(false);

            });

        builder.Property(_ => _.Version).HasDefaultValue(1);
        builder.Ignore(_ => _.Events);
    }
}