using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace URLShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomAliasExpiryDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomAlias",
                table: "shortenedURLs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "shortenedURLs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_shortenedURLs_CustomAlias",
                table: "shortenedURLs",
                column: "CustomAlias",
                unique: true,
                filter: "[CustomAlias] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_shortenedURLs_CustomAlias",
                table: "shortenedURLs");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "shortenedURLs");

            migrationBuilder.AlterColumn<string>(
                name: "CustomAlias",
                table: "shortenedURLs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
