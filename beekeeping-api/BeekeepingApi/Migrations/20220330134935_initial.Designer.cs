﻿// <auto-generated />
using System;
using BeekeepingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BeekeepingApi.Migrations
{
    [DbContext(typeof(BeekeepingContext))]
    [Migration("20220330134935_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BeekeepingApi.Models.Apiary", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Apiaries");
                });

            modelBuilder.Entity("BeekeepingApi.Models.ApiaryBeeFamily", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ApiaryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("ArriveDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("BeeFamilyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DepartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ApiaryId");

                    b.HasIndex("BeeFamilyId");

                    b.ToTable("ApiaryBeeFamilies");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeeFamily", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsNucleus")
                        .HasColumnType("bit");

                    b.Property<int>("Origin")
                        .HasColumnType("int");

                    b.Property<double?>("RequiredFoodForWinter")
                        .HasColumnType("float");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("BeeFamilies");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Beehive", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("AcquireDay")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Color")
                        .HasColumnType("int");

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsEmpty")
                        .HasColumnType("bit");

                    b.Property<int?>("MaxHoneyCombsSupers")
                        .HasColumnType("int");

                    b.Property<int?>("MaxNestCombs")
                        .HasColumnType("int");

                    b.Property<int?>("NestCombs")
                        .HasColumnType("int");

                    b.Property<int?>("No")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Beehives");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeehiveBeeFamily", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArriveDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("BeeFamilyId")
                        .HasColumnType("bigint");

                    b.Property<long>("BeehiveId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DepartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BeeFamilyId");

                    b.HasIndex("BeehiveId");

                    b.ToTable("BeehiveBeeFamilies");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeehiveComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BeehiveId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("InstallationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Position")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BeehiveId");

                    b.ToTable("BeehiveComponents");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Farm", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("BeekeepingApi.Models.FarmWorker", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("UserId", "FarmId");

                    b.HasIndex("FarmId");

                    b.ToTable("FarmWorkers");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Feeding", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BeeFamilyId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<long>("FoodId")
                        .HasColumnType("bigint");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BeeFamilyId");

                    b.HasIndex("FoodId");

                    b.ToTable("Feedings");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Food", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FarmId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Harvest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApiaryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("date");

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<int>("Product")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ApiaryId");

                    b.HasIndex("FarmId");

                    b.ToTable("Harvests");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Manufacturer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Manufacturers");
                });

            modelBuilder.Entity("BeekeepingApi.Models.NestShortening", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BeeFamilyId")
                        .HasColumnType("bigint");

                    b.Property<int?>("CombsBefore")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("StayedBroodCombs")
                        .HasColumnType("int");

                    b.Property<int>("StayedCombs")
                        .HasColumnType("int");

                    b.Property<double>("StayedHoney")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BeeFamilyId");

                    b.ToTable("NestShortenings");
                });

            modelBuilder.Entity("BeekeepingApi.Models.TodoItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ApiaryId")
                        .HasColumnType("bigint");

                    b.Property<long?>("BeeFamilyId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("date");

                    b.Property<long>("FarmId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ApiaryId");

                    b.HasIndex("BeeFamilyId");

                    b.HasIndex("FarmId");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("BeekeepingApi.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("DefaultFarmId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Apiary", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("Apiaries")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.ApiaryBeeFamily", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Apiary", "Apiary")
                        .WithMany("ApiaryBeeFamilies")
                        .HasForeignKey("ApiaryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeekeepingApi.Models.BeeFamily", "BeeFamily")
                        .WithMany("ApiaryBeeFamilies")
                        .HasForeignKey("BeeFamilyId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Apiary");

                    b.Navigation("BeeFamily");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeeFamily", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("BeeFamilies")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Beehive", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany()
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeehiveBeeFamily", b =>
                {
                    b.HasOne("BeekeepingApi.Models.BeeFamily", "BeeFamily")
                        .WithMany("BeehiveBeeFamilies")
                        .HasForeignKey("BeeFamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeekeepingApi.Models.Beehive", "Beehive")
                        .WithMany("BeehiveBeeFamilies")
                        .HasForeignKey("BeehiveId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("BeeFamily");

                    b.Navigation("Beehive");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeehiveComponent", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Beehive", "Beehive")
                        .WithMany("Components")
                        .HasForeignKey("BeehiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Beehive");
                });

            modelBuilder.Entity("BeekeepingApi.Models.FarmWorker", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("FarmWorkers")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeekeepingApi.Models.User", "User")
                        .WithMany("FarmWorkers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Feeding", b =>
                {
                    b.HasOne("BeekeepingApi.Models.BeeFamily", "BeeFamily")
                        .WithMany("Feedings")
                        .HasForeignKey("BeeFamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BeekeepingApi.Models.Food", "Food")
                        .WithMany("Feedings")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("BeeFamily");

                    b.Navigation("Food");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Food", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("Foods")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Harvest", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Apiary", "Apiary")
                        .WithMany("Harvests")
                        .HasForeignKey("ApiaryId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("Harvests")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Apiary");

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.NestShortening", b =>
                {
                    b.HasOne("BeekeepingApi.Models.BeeFamily", "BeeFamily")
                        .WithMany("NestShortenings")
                        .HasForeignKey("BeeFamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BeeFamily");
                });

            modelBuilder.Entity("BeekeepingApi.Models.TodoItem", b =>
                {
                    b.HasOne("BeekeepingApi.Models.Apiary", "Apiary")
                        .WithMany("TodoItems")
                        .HasForeignKey("ApiaryId");

                    b.HasOne("BeekeepingApi.Models.BeeFamily", "BeeFamily")
                        .WithMany("TodoItems")
                        .HasForeignKey("BeeFamilyId");

                    b.HasOne("BeekeepingApi.Models.Farm", "Farm")
                        .WithMany("TodoItems")
                        .HasForeignKey("FarmId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Apiary");

                    b.Navigation("BeeFamily");

                    b.Navigation("Farm");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Apiary", b =>
                {
                    b.Navigation("ApiaryBeeFamilies");

                    b.Navigation("Harvests");

                    b.Navigation("TodoItems");
                });

            modelBuilder.Entity("BeekeepingApi.Models.BeeFamily", b =>
                {
                    b.Navigation("ApiaryBeeFamilies");

                    b.Navigation("BeehiveBeeFamilies");

                    b.Navigation("Feedings");

                    b.Navigation("NestShortenings");

                    b.Navigation("TodoItems");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Beehive", b =>
                {
                    b.Navigation("BeehiveBeeFamilies");

                    b.Navigation("Components");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Farm", b =>
                {
                    b.Navigation("Apiaries");

                    b.Navigation("BeeFamilies");

                    b.Navigation("FarmWorkers");

                    b.Navigation("Foods");

                    b.Navigation("Harvests");

                    b.Navigation("TodoItems");
                });

            modelBuilder.Entity("BeekeepingApi.Models.Food", b =>
                {
                    b.Navigation("Feedings");
                });

            modelBuilder.Entity("BeekeepingApi.Models.User", b =>
                {
                    b.Navigation("FarmWorkers");
                });
#pragma warning restore 612, 618
        }
    }
}
