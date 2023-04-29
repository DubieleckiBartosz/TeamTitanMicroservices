using General.Domain.Entities;
using General.Domain.ValueObjects;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace General.Infrastructure.DomainConfigurations;

public class EntityPostConfiguration : WatcherConfiguration, IEntityTypeConfiguration<Post>
{ 
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(Common.PostTable);
        builder.HasKey(_ => _.Id);
        builder.Property(_ => _.CreatedBy).IsRequired();
        builder.Property<bool>("IsDeleted").IsRequired(); 

        builder.OwnsMany<Attachment>(Common.NavigationAttachmentsList, _ =>
        {
            _.WithOwner().HasForeignKey("PostId"); 
            _.ToTable(Common.AttachmentTable);
            _.Property<int>("Id");
            _.HasKey("Id");
        });
          
        builder.OwnsOne(_ => _.Content,
            _ =>
            { 
                _.Property(c => c.Description).HasColumnName("Description").IsRequired(false);

            }); 

        builder.Property(_ => _.Version).HasDefaultValue(1);

        builder.Ignore(_ => _.Events);
        builder.HasQueryFilter(_ => EF.Property<bool>(_, "IsDeleted") == false);

        this.ConfigureWatcher(builder);
    }
}