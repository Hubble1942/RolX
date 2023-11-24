﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RolXServer;

#nullable disable

namespace RolXServer.Migrations
{
    [DbContext(typeof(RolXContext))]
    [Migration("20231115101226_AddUserVacationDaysSettings")]
    partial class AddUserVacationDaysSettings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
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

                    b.Property<DateOnly?>("EndedDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<long?>("PlannedSeconds")
                        .HasColumnType("bigint");

                    b.Property<DateOnly>("StartDate")
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
                            Id = 1,
                            BillabilityId = 7,
                            Name = "Ferien",
                            Number = 1,
                            StartDate = new DateOnly(2020, 1, 1),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 2,
                            BillabilityId = 7,
                            Name = "Krank",
                            Number = 2,
                            StartDate = new DateOnly(2020, 1, 1),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 3,
                            BillabilityId = 7,
                            Name = "Militär",
                            Number = 3,
                            StartDate = new DateOnly(2020, 1, 1),
                            SubprojectId = 1
                        },
                        new
                        {
                            Id = 4,
                            BillabilityId = 7,
                            Name = "Sonstige",
                            Number = 4,
                            StartDate = new DateOnly(2020, 1, 1),
                            SubprojectId = 1
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
                            SortingWeight = 10
                        },
                        new
                        {
                            Id = 3,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar TP",
                            SortingWeight = 20
                        },
                        new
                        {
                            Id = 4,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar Extern",
                            SortingWeight = 30
                        },
                        new
                        {
                            Id = 5,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar Nearshore",
                            SortingWeight = 40
                        },
                        new
                        {
                            Id = 6,
                            Inactive = false,
                            IsBillable = true,
                            Name = "Verrechenbar 50+",
                            SortingWeight = 50
                        },
                        new
                        {
                            Id = 7,
                            Inactive = false,
                            IsBillable = false,
                            Name = "Abwesenheit",
                            SortingWeight = 200
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

                    b.Property<Guid?>("ArchitectId")
                        .HasColumnType("char(36)");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("DeputyManagerId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("char(36)");

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

                    b.HasIndex("ArchitectId");

                    b.HasIndex("DeputyManagerId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("ProjectNumber", "Number")
                        .IsUnique();

                    b.ToTable("Subprojects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CustomerName = "M&F",
                            Name = "Bezahlte Abwesenheiten",
                            Number = 1,
                            ProjectName = "Allgemein",
                            ProjectNumber = 8900
                        });
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.EditLock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("EditLocks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateOnly(2022, 1, 1)
                        });
                });

            modelBuilder.Entity("RolXServer.Records.DataAccess.Record", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
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

                    b.Property<TimeOnly?>("Begin")
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

                    b.ToTable("RecordEntries");
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

                    b.Property<DateOnly>("EntryDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("GoogleId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly?>("LeftDate")
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

                    b.Property<DateOnly>("Date")
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

                    b.Property<DateOnly>("StartDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "StartDate")
                        .IsUnique();

                    b.ToTable("UserPartTimeSettings");
                });

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserVacationDaysSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("StartDate")
                        .HasPrecision(0)
                        .HasColumnType("date");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<int>("VacationDaysPerYear")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "StartDate")
                        .IsUnique();

                    b.ToTable("UserVacationDaysSettings");
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

            modelBuilder.Entity("RolXServer.Projects.DataAccess.Subproject", b =>
                {
                    b.HasOne("RolXServer.Users.DataAccess.User", "Architect")
                        .WithMany()
                        .HasForeignKey("ArchitectId");

                    b.HasOne("RolXServer.Users.DataAccess.User", "DeputyManager")
                        .WithMany()
                        .HasForeignKey("DeputyManagerId");

                    b.HasOne("RolXServer.Users.DataAccess.User", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("Architect");

                    b.Navigation("DeputyManager");

                    b.Navigation("Manager");
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

                    b.HasOne("RolXServer.Records.DataAccess.Record", "Record")
                        .WithMany("Entries")
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Activity");

                    b.Navigation("Record");
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

            modelBuilder.Entity("RolXServer.Users.DataAccess.UserVacationDaysSetting", b =>
                {
                    b.HasOne("RolXServer.Users.DataAccess.User", null)
                        .WithMany("VacationDaysSettings")
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

                    b.Navigation("VacationDaysSettings");
                });
#pragma warning restore 612, 618
        }
    }
}