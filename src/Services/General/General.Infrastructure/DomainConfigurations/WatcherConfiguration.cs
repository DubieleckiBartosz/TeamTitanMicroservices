﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Base;

namespace General.Infrastructure.DomainConfigurations;

public abstract class WatcherConfiguration
{
    protected void ConfigureWatcher<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : Entity
    { 
        builder.OwnsOne(_ => _.Watcher, w =>
        {
            w.Property<DateTime>(_ => _.Created).HasColumnName("Created").IsRequired();
            w.Property<DateTime>(_ => _.LastModified).HasColumnName("LastModified").IsRequired();
            w.Property<DateTime?>(_ => _.DeletedAt).HasColumnName("DeletedAt").IsRequired(false);
        });
    }
}