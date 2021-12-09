﻿// <auto-generated />
#pragma warning disable 1591
using System;
using BackendBPR.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace OrangeBushApi.Migrations
{
    [DbContext(typeof(OrangeBushContext))]
    [Migration("20211208142716_DashboardUserPLants")]
    partial class DashboardUserPLants
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("BackendBPR.Database.Advice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int?>("TagId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TagId");

                    b.ToTable("Advices");
                });

            modelBuilder.Entity("BackendBPR.Database.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DashboardId")
                        .HasColumnType("integer");

                    b.Property<int>("PlantId")
                        .HasColumnType("integer");

                    b.Property<string>("Type")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DashboardId");

                    b.HasIndex("PlantId");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("BackendBPR.Database.Dashboard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Dashboards");
                });

            modelBuilder.Entity("BackendBPR.Database.Measurement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MeasurementDefinitionId")
                        .HasColumnType("integer");

                    b.Property<int>("UserPlantId")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MeasurementDefinitionId");

                    b.HasIndex("UserPlantId");

                    b.ToTable("Measurements");
                });

            modelBuilder.Entity("BackendBPR.Database.MeasurementDefinition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PlantId")
                        .HasColumnType("integer");

                    b.Property<string>("PreferredRange")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PlantId");

                    b.ToTable("MeasurementDefinitions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("MeasurementDefinition");
                });

            modelBuilder.Entity("BackendBPR.Database.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("PlantId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlantId");

                    b.HasIndex("UserId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("BackendBPR.Database.Plant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CommonName")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("ScientificName")
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("BackendBPR.Database.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("BackendBPR.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("bytea");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BackendBPR.Database.UserAdvice", b =>
                {
                    b.Property<int>("AdviceId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("AdviceId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAdvices");
                });

            modelBuilder.Entity("BackendBPR.Database.UserPlant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PlantId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PlantId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPlants");
                });

            modelBuilder.Entity("DashboardUserPlant", b =>
                {
                    b.Property<int>("DashboardsId")
                        .HasColumnType("integer");

                    b.Property<int>("UserPlantsId")
                        .HasColumnType("integer");

                    b.HasKey("DashboardsId", "UserPlantsId");

                    b.HasIndex("UserPlantsId");

                    b.ToTable("DashboardUserPlant");
                });

            modelBuilder.Entity("PlantTag", b =>
                {
                    b.Property<int>("PlantsId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("PlantsId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("PlantTag");
                });

            modelBuilder.Entity("BackendBPR.Database.CustomMeasurementDefinition", b =>
                {
                    b.HasBaseType("BackendBPR.Database.MeasurementDefinition");

                    b.Property<int>("UserPlantId")
                        .HasColumnType("integer");

                    b.HasIndex("UserPlantId");

                    b.HasDiscriminator().HasValue("CustomMeasurementDefinition");
                });

            modelBuilder.Entity("BackendBPR.Database.Advice", b =>
                {
                    b.HasOne("BackendBPR.Database.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("BackendBPR.Database.Board", b =>
                {
                    b.HasOne("BackendBPR.Database.Dashboard", "Dashboard")
                        .WithMany("Boards")
                        .HasForeignKey("DashboardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.Plant", "Plant")
                        .WithMany()
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dashboard");

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("BackendBPR.Database.Dashboard", b =>
                {
                    b.HasOne("BackendBPR.Database.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendBPR.Database.Measurement", b =>
                {
                    b.HasOne("BackendBPR.Database.MeasurementDefinition", "MeasurementDefinition")
                        .WithMany()
                        .HasForeignKey("MeasurementDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.UserPlant", "UserPlant")
                        .WithMany("Measurements")
                        .HasForeignKey("UserPlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MeasurementDefinition");

                    b.Navigation("UserPlant");
                });

            modelBuilder.Entity("BackendBPR.Database.MeasurementDefinition", b =>
                {
                    b.HasOne("BackendBPR.Database.Plant", "Plant")
                        .WithMany("MeasurementDefinitions")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");
                });

            modelBuilder.Entity("BackendBPR.Database.Note", b =>
                {
                    b.HasOne("BackendBPR.Database.Plant", "Plant")
                        .WithMany()
                        .HasForeignKey("PlantId");

                    b.HasOne("BackendBPR.Database.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendBPR.Database.UserAdvice", b =>
                {
                    b.HasOne("BackendBPR.Database.Advice", "Advice")
                        .WithMany("UserAdvices")
                        .HasForeignKey("AdviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Advice");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackendBPR.Database.UserPlant", b =>
                {
                    b.HasOne("BackendBPR.Database.Plant", "Plant")
                        .WithMany("UserPlants")
                        .HasForeignKey("PlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.User", "User")
                        .WithMany("UserPlants")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plant");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DashboardUserPlant", b =>
                {
                    b.HasOne("BackendBPR.Database.Dashboard", null)
                        .WithMany()
                        .HasForeignKey("DashboardsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.UserPlant", null)
                        .WithMany()
                        .HasForeignKey("UserPlantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PlantTag", b =>
                {
                    b.HasOne("BackendBPR.Database.Plant", null)
                        .WithMany()
                        .HasForeignKey("PlantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendBPR.Database.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BackendBPR.Database.CustomMeasurementDefinition", b =>
                {
                    b.HasOne("BackendBPR.Database.UserPlant", "UserPlant")
                        .WithMany()
                        .HasForeignKey("UserPlantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserPlant");
                });

            modelBuilder.Entity("BackendBPR.Database.Advice", b =>
                {
                    b.Navigation("UserAdvices");
                });

            modelBuilder.Entity("BackendBPR.Database.Dashboard", b =>
                {
                    b.Navigation("Boards");
                });

            modelBuilder.Entity("BackendBPR.Database.Plant", b =>
                {
                    b.Navigation("MeasurementDefinitions");

                    b.Navigation("UserPlants");
                });

            modelBuilder.Entity("BackendBPR.Database.User", b =>
                {
                    b.Navigation("Notes");

                    b.Navigation("UserPlants");
                });

            modelBuilder.Entity("BackendBPR.Database.UserPlant", b =>
                {
                    b.Navigation("Measurements");
                });
#pragma warning restore 612, 618
        }
    }
}
