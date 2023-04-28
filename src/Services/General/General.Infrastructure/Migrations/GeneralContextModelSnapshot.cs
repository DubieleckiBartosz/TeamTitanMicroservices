﻿// <auto-generated />
using System;
using General.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace General.Infrastructure.Migrations
{
    [DbContext(typeof(GeneralContext))]
    partial class GeneralContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("General.Domain.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Creator")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("General.Domain.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.ToTable("Posts", (string)null);
                });

            modelBuilder.Entity("General.Domain.ValueObjects.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("Creator")
                        .HasColumnType("int");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Reactions", (string)null);

                    b.HasCheckConstraint("CK_Reaction_AtLeastOneForeignKey", "PostId IS NOT NULL OR CommentId IS NOT NULL");
                });

            modelBuilder.Entity("General.Domain.Entities.Comment", b =>
                {
                    b.HasOne("General.Domain.Entities.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("General.Domain.ValueObjects.Content", "Description", b1 =>
                        {
                            b1.Property<int>("CommentId")
                                .HasColumnType("int");

                            b1.Property<string>("Description")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Description");

                            b1.HasKey("CommentId");

                            b1.ToTable("Comments");

                            b1.WithOwner()
                                .HasForeignKey("CommentId");
                        });

                    b.OwnsOne("Shared.Domain.Base.Watcher", "Watcher", b1 =>
                        {
                            b1.Property<int>("CommentId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime2")
                                .HasColumnName("Created");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("datetime2")
                                .HasColumnName("DeletedAt");

                            b1.Property<DateTime?>("LastModified")
                                .IsRequired()
                                .HasColumnType("datetime2")
                                .HasColumnName("LastModified");

                            b1.HasKey("CommentId");

                            b1.ToTable("Comments");

                            b1.WithOwner()
                                .HasForeignKey("CommentId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Watcher");
                });

            modelBuilder.Entity("General.Domain.Entities.Post", b =>
                {
                    b.OwnsOne("General.Domain.ValueObjects.Content", "Content", b1 =>
                        {
                            b1.Property<int>("PostId")
                                .HasColumnType("int");

                            b1.Property<string>("Description")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Description");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsOne("Shared.Domain.Base.Watcher", "Watcher", b1 =>
                        {
                            b1.Property<int>("PostId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("datetime2")
                                .HasColumnName("Created");

                            b1.Property<DateTime?>("DeletedAt")
                                .HasColumnType("datetime2")
                                .HasColumnName("DeletedAt");

                            b1.Property<DateTime?>("LastModified")
                                .IsRequired()
                                .HasColumnType("datetime2")
                                .HasColumnName("LastModified");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsMany("General.Domain.ValueObjects.Attachment", "Attachments", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"), 1L, 1);

                            b1.Property<string>("Path")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("PostId")
                                .HasColumnType("int");

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("Id");

                            b1.HasIndex("PostId");

                            b1.ToTable("Attachments", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.Navigation("Attachments");

                    b.Navigation("Content");

                    b.Navigation("Watcher");
                });

            modelBuilder.Entity("General.Domain.ValueObjects.Reaction", b =>
                {
                    b.HasOne("General.Domain.Entities.Comment", null)
                        .WithMany("Reactions")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("General.Domain.Entities.Post", null)
                        .WithMany("Reactions")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("General.Domain.Entities.Comment", b =>
                {
                    b.Navigation("Reactions");
                });

            modelBuilder.Entity("General.Domain.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Reactions");
                });
#pragma warning restore 612, 618
        }
    }
}
