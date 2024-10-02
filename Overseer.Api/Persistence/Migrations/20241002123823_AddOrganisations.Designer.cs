﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Overseer.Api.Persistence;

#nullable disable

namespace Overseer.Api.Persistence.Migrations
{
    [DbContext(typeof(OverseerDbContext))]
    [Migration("20241002123823_AddOrganisations")]
    partial class AddOrganisations
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Overseer.Api.Features.Organisations.Entities.Organisation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Organisations", "public");
                });

            modelBuilder.Entity("Overseer.Api.Features.Products.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Products", "public");
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "users:read"
                        },
                        new
                        {
                            Id = 2,
                            Name = "users:write"
                        },
                        new
                        {
                            Id = 3,
                            Name = "users:delete"
                        });
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.RolePermission", b =>
                {
                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions", "public");

                    b.HasData(
                        new
                        {
                            RoleId = 1,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 3
                        },
                        new
                        {
                            RoleId = 1,
                            PermissionId = 4
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 1
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 2
                        },
                        new
                        {
                            RoleId = 2,
                            PermissionId = 3
                        });
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid?>("EmailVerificationToken")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("EmailVerificationTokenExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid?>("InviteToken")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("OrganisationId")
                        .HasColumnType("uuid");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid?>("PasswordResetToken")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("PasswordResetTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("RefreshTokenExpires")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("EmailVerificationToken")
                        .IsUnique();

                    b.HasIndex("InviteToken")
                        .IsUnique();

                    b.HasIndex("OrganisationId");

                    b.HasIndex("PasswordResetToken")
                        .IsUnique();

                    b.HasIndex("RefreshToken")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", "public");
                });

            modelBuilder.Entity("Overseer.Api.Services.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("jsonb");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<DateTime>("OccurredOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ProcessedOnUtc")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Outbox_Message", "public");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleUser", "public");
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.RolePermission", b =>
                {
                    b.HasOne("Overseer.Api.Features.Users.Entities.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Overseer.Api.Features.Users.Entities.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.User", b =>
                {
                    b.HasOne("Overseer.Api.Features.Organisations.Entities.Organisation", "Organisation")
                        .WithMany("Users")
                        .HasForeignKey("OrganisationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organisation");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Overseer.Api.Features.Users.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Overseer.Api.Features.Users.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Overseer.Api.Features.Organisations.Entities.Organisation", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Overseer.Api.Features.Users.Entities.Role", b =>
                {
                    b.Navigation("RolePermissions");
                });
#pragma warning restore 612, 618
        }
    }
}
