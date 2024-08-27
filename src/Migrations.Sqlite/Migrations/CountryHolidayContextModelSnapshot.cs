﻿// <auto-generated />
using System;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Migrations.Sqlite.Migrations
{
    [DbContext(typeof(CountryHolidayContext))]
    partial class CountryHolidayContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Domain.Entities.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CountryId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("Domain.Entities.HolidayDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("HolidayId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("HolidayId");

                    b.HasIndex("Date", "HolidayId");

                    b.ToTable("HolidayDates");
                });

            modelBuilder.Entity("Domain.Entities.LocalizedName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("HolidayId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HolidayId");

                    b.ToTable("LocalizedNames");
                });

            modelBuilder.Entity("Domain.Entities.Holiday", b =>
                {
                    b.HasOne("Domain.Entities.Country", "Country")
                        .WithMany("Holidays")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Domain.Entities.HolidayDate", b =>
                {
                    b.HasOne("Domain.Entities.Holiday", "Holiday")
                        .WithMany("Dates")
                        .HasForeignKey("HolidayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holiday");
                });

            modelBuilder.Entity("Domain.Entities.LocalizedName", b =>
                {
                    b.HasOne("Domain.Entities.Holiday", "Holiday")
                        .WithMany("Names")
                        .HasForeignKey("HolidayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holiday");
                });

            modelBuilder.Entity("Domain.Entities.Country", b =>
                {
                    b.Navigation("Holidays");
                });

            modelBuilder.Entity("Domain.Entities.Holiday", b =>
                {
                    b.Navigation("Dates");

                    b.Navigation("Names");
                });
#pragma warning restore 612, 618
        }
    }
}
