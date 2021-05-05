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
    [Migration("20210505120023_Added-AcessTokens-To-User-tables")]
    partial class AddedAcessTokensToUsertables
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

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PpUserId")
                        .HasColumnType("int");

                    b.HasKey("InventoryId");

                    b.HasIndex("PpUserId");

                    b.ToTable("Inventory");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Inventory");
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
                        .IsRequired()
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

            modelBuilder.Entity("DataBase.Models.UserDb", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessJWTToken")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("Email")
                        .HasMaxLength(254)
                        .HasColumnType("nvarchar(254)");

                    b.Property<string>("FullName")
                        .HasMaxLength(96)
                        .HasColumnType("nvarchar(96)");

                    b.Property<string>("PwHash")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("UserId");

                    b.ToTable("User");
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
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PpUserId");

                    b.ToTable("PpUser");
                });

            modelBuilder.Entity("DataBase.Freezer", b =>
                {
                    b.HasBaseType("DataBase.Inventory");

                    b.HasDiscriminator().HasValue("Freezer");
                });

            modelBuilder.Entity("DataBase.Fridge", b =>
                {
                    b.HasBaseType("DataBase.Inventory");

                    b.HasDiscriminator().HasValue("Fridge");
                });

            modelBuilder.Entity("DataBase.Pantry", b =>
                {
                    b.HasBaseType("DataBase.Inventory");

                    b.HasDiscriminator().HasValue("Pantry");
                });

            modelBuilder.Entity("DataBase.ShoppingList", b =>
                {
                    b.HasBaseType("DataBase.Inventory");

                    b.HasDiscriminator().HasValue("ShoppingList");
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
