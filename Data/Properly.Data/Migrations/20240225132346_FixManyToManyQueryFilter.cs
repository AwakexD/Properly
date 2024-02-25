using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properly.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixManyToManyQueryFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PropertyFeatures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "PropertyFeatures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PropertyFeatures",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "PropertyFeatures",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeatures_IsDeleted",
                table: "PropertyFeatures",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PropertyFeatures_IsDeleted",
                table: "PropertyFeatures");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PropertyFeatures");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "PropertyFeatures");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PropertyFeatures");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "PropertyFeatures");
        }
    }
}
