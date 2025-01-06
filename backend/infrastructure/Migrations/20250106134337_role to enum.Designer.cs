﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using infrastructure;

#nullable disable

namespace infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20250106134337_role to enum")]
    partial class roletoenum
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<string>("GroupsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UsersId")
                        .HasColumnType("TEXT");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("core.models.File", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileContents")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nonce")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("core.models.Group", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CreatorEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("core.models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("core.models.UserFileAccess", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("EncryptedFileKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.HasIndex("UserId");

                    b.ToTable("UserFileAccesses");
                });

            modelBuilder.Entity("core.models.UserRsaKeyPair", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nonce")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserRsaKeyPairs");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("core.models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("core.models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("core.models.File", b =>
                {
                    b.HasOne("core.models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("core.models.UserFileAccess", b =>
                {
                    b.HasOne("core.models.File", "File")
                        .WithMany("UserFileAccesses")
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("core.models.User", "User")
                        .WithMany("UserFileAccesses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("File");

                    b.Navigation("User");
                });

            modelBuilder.Entity("core.models.UserRsaKeyPair", b =>
                {
                    b.HasOne("core.models.User", "User")
                        .WithOne("UserRsaKeyPair")
                        .HasForeignKey("core.models.UserRsaKeyPair", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("core.models.File", b =>
                {
                    b.Navigation("UserFileAccesses");
                });

            modelBuilder.Entity("core.models.User", b =>
                {
                    b.Navigation("UserFileAccesses");

                    b.Navigation("UserRsaKeyPair")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
