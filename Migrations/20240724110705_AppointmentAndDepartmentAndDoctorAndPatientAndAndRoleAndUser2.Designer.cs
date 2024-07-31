﻿// <auto-generated />
using System;
using Hospital.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hospital.Migrations
{
    [DbContext(typeof(HospitalSchema))]
    [Migration("20240724110705_AppointmentAndDepartmentAndDoctorAndPatientAndAndRoleAndUser2")]
    partial class AppointmentAndDepartmentAndDoctorAndPatientAndAndRoleAndUser2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hospital.Context.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AppointmentDate")
                        .HasColumnType("text");

                    b.Property<int>("DoktorId")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DoktorId");

                    b.ToTable("Appointment");
                });

            modelBuilder.Entity("Hospital.Context.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DepartmentName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("Hospital.Context.Doctor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("DoctorName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Doctor");
                });

            modelBuilder.Entity("Hospital.Context.Patient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("MedicalHistory")
                        .HasColumnType("text");

                    b.Property<string>("PatientName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patient");
                });

            modelBuilder.Entity("Hospital.Context.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("Hospital.Context.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("DoctorId")
                        .HasColumnType("integer");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<int?>("PatientId")
                        .HasColumnType("integer");

                    b.Property<string>("Role")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("PatientId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("Hospital.Context.Appointment", b =>
                {
                    b.HasOne("Hospital.Context.Doctor", "Doktor")
                        .WithMany()
                        .HasForeignKey("DoktorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doktor");
                });

            modelBuilder.Entity("Hospital.Context.Doctor", b =>
                {
                    b.HasOne("Hospital.Context.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Hospital.Context.User", b =>
                {
                    b.HasOne("Hospital.Context.Doctor", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId");

                    b.HasOne("Hospital.Context.Patient", "Patient")
                        .WithMany()
                        .HasForeignKey("PatientId");

                    b.Navigation("Doctor");

                    b.Navigation("Patient");
                });
#pragma warning restore 612, 618
        }
    }
}
