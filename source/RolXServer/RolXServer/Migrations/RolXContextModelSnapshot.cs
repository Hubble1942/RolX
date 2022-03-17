﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RolXServer;

#nullable disable

namespace RolXServer.Migrations
{
    [DbContext(typeof(RolXContext))]
    partial class RolXContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BillabilityId")
                        .HasColumnType("int");

                    b.Property<long?>("BudgetSeconds")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("EndDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<int>("SubprojectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BillabilityId");

                    b.HasIndex("SubprojectId", "Number")
                        .IsUnique();

                    b.ToTable("Activities");

                    b.HasData(
                        new
                        {
                            Id = 11,
                            BillabilityId = 2,
                            Name = "Take off",
                            Number = 1,
                            StartDate = new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 12,
                            BillabilityId = 1,
                            Name = "Cruise",
                            Number = 2,
                            StartDate = new DateTime(2022, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 13,
                            BillabilityId = 3,
                            EndDate = new DateTime(2022, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Landing",
                            Number = 3,
                            StartDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 21,
                            BillabilityId = 4,
                            Name = "Take off",
                            Number = 1,
                            StartDate = new DateTime(2021, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 2
                        },
                        new
                        {
                            Id = 22,
                            BillabilityId = 1,
                            Name = "Cruise",
                            Number = 2,
                            StartDate = new DateTime(2022, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 2
                        },
                        new
                        {
                            Id = 23,
                            BillabilityId = 2,
                            EndDate = new DateTime(2022, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Landing",
                            Number = 3,
                            StartDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 2
                        },
                        new
                        {
                            Id = 31,
                            BillabilityId = 3,
                            Name = "Analyse",
                            Number = 1,
                            StartDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 3
                        },
                        new
                        {
                            Id = 32,
                            BillabilityId = 4,
                            Name = "Umsetzung",
                            Number = 2,
                            StartDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 3
                        },
                        new
                        {
                            Id = 33,
                            BillabilityId = 2,
                            Name = "Übergabe",
                            Number = 3,
                            StartDate = new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SubprojectId = 3
                        });
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Billability", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Inactive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsBillable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SortingWeight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Billabilities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Inactive = false,
                            IsBillable = false,
                            Name = "Nicht verrechenbar",
                            SortingWeight = 100
                        },
                        new
                        {
                            Id = 2,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar Engineering",
                            SortingWeight = 1
                        },
                        new
                        {
                            Id = 3,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar TP",
                            SortingWeight = 2
                        },
                        new
                        {
                            Id = 4,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar 50+",
                            SortingWeight = 3
                        });
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.FavouriteActivity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ActivityId");

                    b.HasIndex("ActivityId");

                    b.ToTable("FavouriteActivities");
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Subproject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ProjectNumber")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectNumber", "Number")
                        .IsUnique();

                    b.ToTable("Subprojects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CustomerName = "Lockheed Martin",
                            Name = "F35",
                            Number = 1,
                            ProjectName = "Auto Pilot",
                            ProjectNumber = 4711
                        },
                        new
                        {
                            Id = 2,
                            CustomerName = "Lockheed Martin",
                            Name = "F117A",
                            Number = 2,
                            ProjectName = "Auto Pilot",
                            ProjectNumber = 4711
                        },
                        new
                        {
                            Id = 3,
                            CustomerName = "SRF",
                            Name = "Fragengenerator",
                            Number = 1,
                            ProjectName = "ABC SRF 3",
                            ProjectNumber = 3141
                        });
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<string>("PaidLeaveReason")
                        .HasColumnType("longtext");

                    b.Property<int?>("PaidLeaveType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Date", "UserId")
                        .IsUnique();

                    b.ToTable("Records");
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.RecordEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ActivityId")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("Begin")
                        .HasPrecision(0)
                        .HasColumnType("time(0)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("DurationSeconds")
                        .HasColumnType("bigint");

                    b.Property<long?>("PauseSeconds")
                        .HasColumnType("bigint");

                    b.Property<int>("RecordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("RecordId");

                    b.ToTable("RecordEntries", (string)null);
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("EntryDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("LeftDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GoogleId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserBalanceCorrection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<long>("OvertimeSeconds")
                        .HasColumnType("bigint");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<long>("VacationSeconds")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId", "Date")
                        .IsUnique();

                    b.ToTable("UserBalanceCorrections");
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserPartTimeSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double>("Factor")
                        .HasColumnType("double");

                    b.Property<DateTime>("StartDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "StartDate")
                        .IsUnique();

                    b.ToTable("UserPartTimeSettings");
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Activity", b =>
                {
                    b.HasOne("RolXServer.Projects.DataAccess.Billability", "Billability")
                        .WithMany()
                        .HasForeignKey("BillabilityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RolXServer.Projects.DataAccess.Subproject", "Subproject")
                        .WithMany("Activities")
                        .HasForeignKey("SubprojectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Billability");

                    b.Navigation("Subproject");
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.FavouriteActivity", b =>
                {
                    b.HasOne("RolXServer.Projects.DataAccess.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RolXServer.Users.DataAccess.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.Record", b =>
                {
                    b.HasOne("RolXServer.Users.DataAccess.User", "User")
                        .WithMany("Records")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.RecordEntry", b =>
                {
                    b.HasOne("RolXServer.Projects.DataAccess.Activity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RolXServer.Records.DataAccess.Record", null)
                        .WithMany("Entries")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserBalanceCorrection", b =>
                {
                    b.HasOne("RolXServer.Users.DataAccess.User", null)
                        .WithMany("BalanceCorrections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserPartTimeSetting", b =>
                {
                    b.HasOne("RolXServer.Users.DataAccess.User", null)
                        .WithMany("PartTimeSettings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Subproject", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.Record", b =>
                {
                    b.Navigation("Entries");
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.User", b =>
                {
                    b.Navigation("BalanceCorrections");

                    b.Navigation("PartTimeSettings");

                    b.Navigation("Records");
                });
#pragma warning restore 612, 618
        }
    }
}
