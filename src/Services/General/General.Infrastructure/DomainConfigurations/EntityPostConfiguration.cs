using General.Domain.Entities;
using General.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class EntityPostConfiguration : IEntityTypeConfiguration<Post>
{
    internal const string NavigationCommentsList = "_comments"; 
    internal const string NavigationAttachmentsList = "_attachments";
    private const string NavigationContent = "Content";

    private const string PostTable = "Posts";
    private const string CommentTable = "Comments";
    private const string AttachmentTable = "Attachments";

    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(PostTable);
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.CreatedBy).IsRequired();

        builder.OwnsMany<Comment>(NavigationCommentsList, _ =>
        {
            _.ToTable(CommentTable);
            _.HasKey(_ => _.Id);
            _.WithOwner().HasForeignKey("PostId");

            _.Property(_ => _.Creator).IsRequired();

            _.OwnsOne<Content>(NavigationContent,
                _ =>
                {
                    _.Property(c => c.Description).HasColumnName("Comment").IsRequired(false);

                });
        });

        builder.OwnsMany<Attachment>(NavigationAttachmentsList, _ =>
        {
            _.ToTable(AttachmentTable);
            _.HasKey(_ => _.Id);
            _.WithOwner().HasForeignKey("PostId");
        });

        builder.OwnsOne<Content>(NavigationContent,
            _ =>
            {
                _.Property(c => c.Description).HasColumnName("Description").IsRequired(false);

            });

    }
}