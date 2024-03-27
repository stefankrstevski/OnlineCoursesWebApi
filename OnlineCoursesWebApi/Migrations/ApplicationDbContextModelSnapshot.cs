﻿// <auto-generated />
using System;
using OnlineCoursesWebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace OnlineCoursesWebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Application", b =>
                {
                    b.Property<int>("ApplicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicationId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("CourseDateId")
                        .HasColumnType("int");

                    b.HasKey("ApplicationId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("CourseDateId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.ApplicationParticipant", b =>
                {
                    b.Property<int>("ApplicationId")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantId")
                        .HasColumnType("int");

                    b.HasKey("ApplicationId", "ParticipantId");

                    b.HasIndex("ParticipantId");

                    b.ToTable("ApplicationParticipants");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.CourseDate", b =>
                {
                    b.Property<int>("CourseDateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseDateId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseDateId");

                    b.HasIndex("CourseId");

                    b.ToTable("CourseDates");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Participant", b =>
                {
                    b.Property<int>("ParticipantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ParticipantId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ParticipantId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Application", b =>
                {
                    b.HasOne("OnlineCoursesWebApi.Models.Company", "Company")
                        .WithMany("Applications")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCoursesWebApi.Models.CourseDate", "CourseDate")
                        .WithMany()
                        .HasForeignKey("CourseDateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("CourseDate");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.ApplicationParticipant", b =>
                {
                    b.HasOne("OnlineCoursesWebApi.Models.Application", "Application")
                        .WithMany("ApplicationParticipants")
                        .HasForeignKey("ApplicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineCoursesWebApi.Models.Participant", "Participant")
                        .WithMany("ApplicationParticipants")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Application");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.CourseDate", b =>
                {
                    b.HasOne("OnlineCoursesWebApi.Models.Course", "Course")
                        .WithMany("Dates")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Application", b =>
                {
                    b.Navigation("ApplicationParticipants");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Company", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Course", b =>
                {
                    b.Navigation("Dates");
                });

            modelBuilder.Entity("OnlineCoursesWebApi.Models.Participant", b =>
                {
                    b.Navigation("ApplicationParticipants");
                });
#pragma warning restore 612, 618
        }
    }
}