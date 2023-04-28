using General.Domain.Entities;
using General.Domain.ValueObjects;
using General.Infrastructure.Database;
using General.Infrastructure.DomainConfigurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class ReactTypeConfiguration : IEntityTypeConfiguration<Reaction>
{ 
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable(Common.ReactionTable);
        builder.HasKey("Id");

        builder.Property(_ => _.Creator).IsRequired();

        builder
            .Property(_ => _.Type)
            .HasConversion<ReactionTypeConverter>()
            .IsRequired();  

        builder.HasOne<Post>()
            .WithMany(_ => _.Reactions)
            .HasForeignKey("PostId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasOne<Comment>()
            .WithMany(_ => _.Reactions)
            .HasForeignKey("CommentId")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        builder.HasCheckConstraint("CK_Reaction_AtLeastOneForeignKey",
            "PostId IS NOT NULL OR CommentId IS NOT NULL");
    }
}