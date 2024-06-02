﻿// <auto-generated />
using System;
using Agrigate.Authentication.Domain.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Agrigate.Authentication.Migrations
{
    [DbContext(typeof(AuthenticationContext))]
    [Migration("20240602225430_Add-Username_Index")]
    partial class AddUsername_Index
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Agrigate.Authentication.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool>("ForcePasswordReset")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LastLogin")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("PasswordExpiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("Agrigate.Domain.Entities.System.Log", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("LogLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Modified")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("StackTrace")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });
#pragma warning restore 612, 618
        }
    }
}