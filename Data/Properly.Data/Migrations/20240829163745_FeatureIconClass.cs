using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properly.Data.Migrations
{
    /// <inheritdoc />
    public partial class FeatureIconClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Listings_ListingId",
                table: "Photos");

            migrationBuilder.AlterColumn<Guid>(
                name: "ListingId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IconClass",
                table: "Features",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Listings_ListingId",
                table: "Photos",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Listings_ListingId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "IconClass",
                table: "Features");

            migrationBuilder.AlterColumn<Guid>(
                name: "ListingId",
                table: "Photos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Listings_ListingId",
                table: "Photos",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id");
        }
    }
}
