﻿// <auto-generated />
using System;
using GeoAPI.Geometries;
using GlodnyStudent.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GlodnyStudent.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190608114457_CuisineEnityFix")]
    partial class CuisineEnityFix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Cuisine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long>("RestaurantId");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId")
                        .IsUnique();

                    b.ToTable("Cuisines");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FilePath");

                    b.Property<long>("RestaurantId");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.MenuItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(13,2)");

                    b.Property<long>("RestaurantId");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Restaurant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("HighestPrice")
                        .HasColumnType("decimal(13,2)");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("OwnerId");

                    b.Property<int>("ReviewsCount");

                    b.Property<int>("Score");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Restaurants");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.RestaurantAddress", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("District");

                    b.Property<int>("LocalNumber");

                    b.Property<IPoint>("Location");

                    b.Property<long>("RestaurantId");

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(7);

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId")
                        .IsUnique();

                    b.ToTable("RestaurantAddresses");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddTime");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<long>("RestaurantId");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RestaurantId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("GlodnyStudent.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Cuisine", b =>
                {
                    b.HasOne("GlodnyStudent.Models.Domain.Restaurant", "Restaurant")
                        .WithOne("Cuisine")
                        .HasForeignKey("GlodnyStudent.Models.Domain.Cuisine", "RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Image", b =>
                {
                    b.HasOne("GlodnyStudent.Models.Domain.Restaurant", "Restaurant")
                        .WithMany("Gallery")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.MenuItem", b =>
                {
                    b.HasOne("GlodnyStudent.Models.Domain.Restaurant", "Restaurant")
                        .WithMany("Menu")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Restaurant", b =>
                {
                    b.HasOne("GlodnyStudent.Models.User", "Owner")
                        .WithMany("Restaurants")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.RestaurantAddress", b =>
                {
                    b.HasOne("GlodnyStudent.Models.Domain.Restaurant", "Restaurant")
                        .WithOne("Address")
                        .HasForeignKey("GlodnyStudent.Models.Domain.RestaurantAddress", "RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("GlodnyStudent.Models.Domain.Review", b =>
                {
                    b.HasOne("GlodnyStudent.Models.Domain.Restaurant", "Restaurant")
                        .WithMany("Reviews")
                        .HasForeignKey("RestaurantId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("GlodnyStudent.Models.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
