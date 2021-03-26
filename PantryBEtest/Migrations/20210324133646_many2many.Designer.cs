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
    [Migration("20210324133646_many2many")]
    partial class many2many
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PantryBEtest.Inventory", b =>
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
                });

            modelBuilder.Entity("PantryBEtest.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("amountInFreezer")
                        .HasColumnType("int");

                    b.Property<int>("amountInfridge")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("PantryBEtest.Models.InventoryItem", b =>
                {
                    b.Property<int>("InventoryId")
                        .HasColumnType("int");

                    b.Property<long>("Amount")
                        .HasColumnType("bigint");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.HasKey("InventoryId");

                    b.ToTable("InventoryItem");
                });

            modelBuilder.Entity("PantryBEtest.PpUser", b =>
                {
                    b.Property<int>("PpUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PpUserId");

                    b.ToTable("PpUser");
                });

            modelBuilder.Entity("PantryBEtest.Inventory", b =>
                {
                    b.HasOne("PantryBEtest.PpUser", null)
                        .WithMany("Inventories")
                        .HasForeignKey("PpUserId");
                });

            modelBuilder.Entity("PantryBEtest.Models.InventoryItem", b =>
                {
                    b.HasOne("PantryBEtest.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PantryBEtest.Item", "Item")
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("PantryBEtest.PpUser", b =>
                {
                    b.Navigation("Inventories");
                });
#pragma warning restore 612, 618
        }
    }
}
