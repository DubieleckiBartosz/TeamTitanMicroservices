using General.Domain.Entities;
using General.Domain.ValueObjects;
using General.Infrastructure.DomainConfigurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class ReactTypeConfiguration : IEntityTypeConfiguration<Reaction>
{
    private const string ReactionTable = "Reactions";

    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable(ReactionTable);
        builder.HasKey("Id");

        builder.Property(_ => _.Creator).IsRequired();

        builder
            .Property(_ => _.Type)
            .HasConversion<ReactionTypeConverter>()
            .IsRequired();  

        builder.HasOne<Post>()
            .WithMany()
            .HasForeignKey("PostId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne<Comment>()
            .WithMany()
            .HasForeignKey("CommentId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasCheckConstraint("CK_Reaction_AtLeastOneForeignKey",
            "PostId IS NOT NULL OR CommentId IS NOT NULL");
    }
}