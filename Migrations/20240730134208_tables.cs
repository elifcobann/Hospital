using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoktorId",
                table: "Appointment");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.RenameColumn(
                name: "DoktorId",
                table: "Appointment",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_DoktorId",
                table: "Appointment",
                newName: "IX_Appointment_PatientId");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Doctor",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Appointment",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AppointmentDate",
                table: "Appointment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Cancel",
                table: "Appointment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "Appointment",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctor_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patient_PatientId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_DoctorId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Cancel",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "Appointment",
                newName: "DoktorId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_PatientId",
                table: "Appointment",
                newName: "IX_Appointment_DoktorId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Appointment",
                type: "text",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "AppointmentDate",
                table: "Appointment",
                type: "text",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DoctorId = table.Column<int>(type: "integer", nullable: true),
                    PatientId = table.Column<int>(type: "integer", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_DoctorId",
                table: "User",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PatientId",
                table: "User",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctor_DoktorId",
                table: "Appointment",
                column: "DoktorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
