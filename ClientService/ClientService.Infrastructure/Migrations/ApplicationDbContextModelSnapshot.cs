﻿// <auto-generated />
using System;
using ClientService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClientService.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("AccountPost", b =>
                {
                    b.Property<long>("ApplicationId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("ApplierId")
                        .HasColumnType("uuid");

                    b.HasKey("ApplicationId", "ApplierId");

                    b.HasIndex("ApplierId");

                    b.ToTable("AccountPost");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("integer");

                    b.Property<string>("AvartarUlr")
                        .HasColumnType("text");

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<string>("Card")
                        .HasColumnType("text");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("boolean");

                    b.Property<string>("LicensePlate")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("SubjectId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.ExponentPushToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique();

                    b.ToTable("ExponentPushTokens");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uuid");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset>("ReadAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Time")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("EndStationId")
                        .HasColumnType("bigint");

                    b.Property<string>("FeedbackContent")
                        .HasColumnType("text");

                    b.Property<float?>("FeedbackPoint")
                        .HasColumnType("real");

                    b.Property<long>("StartStationId")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TripRole")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("EndStationId");

                    b.HasIndex("StartStationId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Station", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ObjectStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Stations");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Trip", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTimeOffset?>("CancelAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("EndStationId")
                        .HasColumnType("bigint");

                    b.Property<string>("FeedbackContent")
                        .HasColumnType("text");

                    b.Property<float?>("FeedbackPoint")
                        .HasColumnType("real");

                    b.Property<DateTimeOffset?>("FinishAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("GrabberId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("PassengerId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("PostedStartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("StartAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("StartStationId")
                        .HasColumnType("bigint");

                    b.Property<int>("TripStatus")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EndStationId");

                    b.HasIndex("GrabberId");

                    b.HasIndex("PassengerId");

                    b.HasIndex("StartStationId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("StationStation", b =>
                {
                    b.Property<long>("NextStationId")
                        .HasColumnType("bigint");

                    b.Property<long>("PreviousStationId")
                        .HasColumnType("bigint");

                    b.HasKey("NextStationId", "PreviousStationId");

                    b.HasIndex("PreviousStationId");

                    b.ToTable("StationStation");
                });

            modelBuilder.Entity("AccountPost", b =>
                {
                    b.HasOne("ClientService.Domain.Entities.Post", null)
                        .WithMany()
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("ApplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClientService.Domain.Entities.ExponentPushToken", b =>
                {
                    b.HasOne("ClientService.Domain.Entities.Account", "Account")
                        .WithOne("ExponentPushToken")
                        .HasForeignKey("ClientService.Domain.Entities.ExponentPushToken", "AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Post", b =>
                {
                    b.HasOne("ClientService.Domain.Entities.Account", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Station", "EndStation")
                        .WithMany()
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Station", "StartStation")
                        .WithMany()
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("EndStation");

                    b.Navigation("StartStation");
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Trip", b =>
                {
                    b.HasOne("ClientService.Domain.Entities.Station", "EndStation")
                        .WithMany()
                        .HasForeignKey("EndStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Account", "Grabber")
                        .WithMany()
                        .HasForeignKey("GrabberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Post", "Post")
                        .WithOne("Trip")
                        .HasForeignKey("ClientService.Domain.Entities.Trip", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Account", "Passenger")
                        .WithMany()
                        .HasForeignKey("PassengerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Station", "StartStation")
                        .WithMany()
                        .HasForeignKey("StartStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EndStation");

                    b.Navigation("Grabber");

                    b.Navigation("Passenger");

                    b.Navigation("Post");

                    b.Navigation("StartStation");
                });

            modelBuilder.Entity("StationStation", b =>
                {
                    b.HasOne("ClientService.Domain.Entities.Station", null)
                        .WithMany()
                        .HasForeignKey("NextStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClientService.Domain.Entities.Station", null)
                        .WithMany()
                        .HasForeignKey("PreviousStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Account", b =>
                {
                    b.Navigation("ExponentPushToken")
                        .IsRequired();
                });

            modelBuilder.Entity("ClientService.Domain.Entities.Post", b =>
                {
                    b.Navigation("Trip");
                });
#pragma warning restore 612, 618
        }
    }
}
