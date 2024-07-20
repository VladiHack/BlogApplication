﻿// <auto-generated />
using System;
using BlogApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogApplication.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogApplication.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CategoryId")
                        .HasName("PK__Categori__19093A2BD38ED518");

                    b.HasIndex(new[] { "Name" }, "UQ__Categori__737584F61001CCE9")
                        .IsUnique();

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BlogApplication.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .HasColumnType("int")
                        .HasColumnName("CommentID");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PostId")
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    b.HasKey("CommentId")
                        .HasName("PK__Comments__C3B4DFAA1757ED53");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BlogApplication.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .HasColumnType("int")
                        .HasColumnName("PostID");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int")
                        .HasColumnName("AuthorID");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasColumnName("CategoryID");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PostId")
                        .HasName("PK__Posts__AA126038CA74685F");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogApplication.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.Property<DateTime?>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("Image")
                        .HasColumnType("image");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CCAC6290D9C4");

                    b.HasIndex(new[] { "Username" }, "UQ__Users__536C85E43B6474CC")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "UQ__Users__A9D105349A78A7D9")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogApplication.Models.Comment", b =>
                {
                    b.HasOne("BlogApplication.Models.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("FK__Comments__Author__5812160E");

                    b.HasOne("BlogApplication.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .IsRequired()
                        .HasConstraintName("FK__Comments__PostID__59063A47");

                    b.Navigation("Author");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("BlogApplication.Models.Post", b =>
                {
                    b.HasOne("BlogApplication.Models.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("FK__Posts__AuthorID__534D60F1");

                    b.HasOne("BlogApplication.Models.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK__Posts__CategoryI__5441852A");

                    b.Navigation("Author");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BlogApplication.Models.Category", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("BlogApplication.Models.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("BlogApplication.Models.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}