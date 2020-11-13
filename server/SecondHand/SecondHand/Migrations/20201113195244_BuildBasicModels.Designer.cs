﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SecondHand.model;

namespace SecondHand.Migrations
{
    [DbContext(typeof(Databases))]
    [Migration("20201113195244_BuildBasicModels")]
    partial class BuildBasicModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("SecondHand.model.Commodity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Photos")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("ReleaseTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SellerId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Sold")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("Commodities");
                });

            modelBuilder.Entity("SecondHand.model.SalesRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Auction")
                        .HasColumnType("TEXT");

                    b.Property<int?>("BuyerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CommodityId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("TransactionTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BuyerId");

                    b.ToTable("SalesRecords");
                });

            modelBuilder.Entity("SecondHand.model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IDNumber")
                        .HasMaxLength(18)
                        .HasColumnType("TEXT");

                    b.Property<string>("IconURL")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("TEXT");

                    b.Property<string>("Profile")
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("RegistrationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("SecondHand.model.Admin", b =>
                {
                    b.HasBaseType("SecondHand.model.User");

                    b.Property<string>("Department")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Admin_SerialNumber");

                    b.HasDiscriminator().HasValue("Admin");
                });

            modelBuilder.Entity("SecondHand.model.Student", b =>
                {
                    b.HasBaseType("SecondHand.model.User");

                    b.Property<string>("College")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("Dormitory")
                        .HasColumnType("TEXT");

                    b.Property<string>("Major")
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("SecondHand.model.Commodity", b =>
                {
                    b.HasOne("SecondHand.model.Student", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId");

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("SecondHand.model.SalesRecord", b =>
                {
                    b.HasOne("SecondHand.model.Student", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerId");

                    b.Navigation("Buyer");
                });
#pragma warning restore 612, 618
        }
    }
}
