using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datavanced.HealthcareManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForCascadResticted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCaregiver_Caregiver_CaregiverId",
                table: "PatientCaregiver");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCaregiver_Patient_PatientId",
                table: "PatientCaregiver");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCaregiver_Caregiver_CaregiverId",
                table: "PatientCaregiver",
                column: "CaregiverId",
                principalTable: "Caregiver",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCaregiver_Patient_PatientId",
                table: "PatientCaregiver",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientCaregiver_Caregiver_CaregiverId",
                table: "PatientCaregiver");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientCaregiver_Patient_PatientId",
                table: "PatientCaregiver");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCaregiver_Caregiver_CaregiverId",
                table: "PatientCaregiver",
                column: "CaregiverId",
                principalTable: "Caregiver",
                principalColumn: "CaregiverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientCaregiver_Patient_PatientId",
                table: "PatientCaregiver",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
