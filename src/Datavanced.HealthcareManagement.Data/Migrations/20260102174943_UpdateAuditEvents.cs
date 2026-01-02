using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Datavanced.HealthcareManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAuditEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AuditEvents");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "AuditEvents");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "AuditEvents");

            migrationBuilder.RenameColumn(
                name: "OccurredAt",
                table: "AuditEvents",
                newName: "Timestamp");

            migrationBuilder.RenameIndex(
                name: "IX_AuditEvents_OccurredAt",
                table: "AuditEvents",
                newName: "IX_AuditEvents_Timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "OfficeId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "EntityId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ActionType",
                table: "AuditEvents",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "AuditEvents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditEvents_ActionType",
                table: "AuditEvents",
                column: "ActionType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEvents_Entity",
                table: "AuditEvents",
                column: "Entity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AuditEvents_ActionType",
                table: "AuditEvents");

            migrationBuilder.DropIndex(
                name: "IX_AuditEvents_Entity",
                table: "AuditEvents");

            migrationBuilder.DropColumn(
                name: "ActionType",
                table: "AuditEvents");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "AuditEvents");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AuditEvents");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "AuditEvents",
                newName: "OccurredAt");

            migrationBuilder.RenameIndex(
                name: "IX_AuditEvents_Timestamp",
                table: "AuditEvents",
                newName: "IX_AuditEvents_OccurredAt");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OfficeId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EntityId",
                table: "AuditEvents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AuditEvents",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "AuditEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "AuditEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
