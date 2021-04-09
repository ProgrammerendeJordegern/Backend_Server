﻿// <auto-generated />
using System;
using DataBase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataBase.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20210409092949_seedData")]
    partial class seedData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataBase.Inventory", b =>
                {
                    b.Property<int>("InventoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("PpUserId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("InventoryId");

                    b.HasIndex("PpUserId");

                    b.ToTable("Inventory");

                    b.HasData(
                        new
                        {
                            InventoryId = 11
                        });
                });

            modelBuilder.Entity("DataBase.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("AverageLifespanDays")
                        .HasColumnType("bigint");

                    b.Property<long>("DesiredMinimumAmount")
                        .HasColumnType("bigint");

                    b.Property<string>("Ean")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.HasKey("ItemId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("DataBase.Models.InventoryItem", b =>
                {
                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.HasKey("InventoryId", "ItemId");

                    b.HasIndex("ItemId");

                    b.ToTable("InventoryItem");
                });

            modelBuilder.Entity("DataBase.PpUser", b =>
                {
                    b.Property<int>("PpUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PpUserId");

                    b.ToTable("PpUser");

                    b.HasData(
                        new
                        {
                            PpUserId = 22,
                            CreationDate = new DateTime(2021, 4, 9, 11, 29, 48, 763, DateTimeKind.Local).AddTicks(4880),
                            Email = "john@john.dk",
                            Name = "John køkkensen",
                            PasswordHash = "2233"
                        });
                });

            modelBuilder.Entity("DataBase.Inventory", b =>
                {
                    b.HasOne("DataBase.PpUser", null)
                        .WithMany("Inventories")
                        .HasForeignKey("PpUserId");
                });

            modelBuilder.Entity("DataBase.Models.InventoryItem", b =>
                {
                    b.HasOne("DataBase.Inventory", "Inventory")
                        .WithMany("ItemCollection")
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataBase.Item", "Item")
                        .WithMany("InventoryCollection")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("DataBase.Inventory", b =>
                {
                    b.Navigation("ItemCollection");
                });

            modelBuilder.Entity("DataBase.Item", b =>
                {
                    b.Navigation("InventoryCollection");
                });

            modelBuilder.Entity("DataBase.PpUser", b =>
                {
                    b.Navigation("Inventories");
                });
#pragma warning restore 612, 618
        }
    }
}
