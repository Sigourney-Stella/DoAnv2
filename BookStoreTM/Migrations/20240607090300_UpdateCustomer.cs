using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreTM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptDate",
                table: "Receipt");

            migrationBuilder.AlterColumn<string>(
                name: "NameProduct",
                table: "ReceiptDetails",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Customer",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Customer");

            migrationBuilder.AlterColumn<string>(
                name: "NameProduct",
                table: "ReceiptDetails",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptDate",
                table: "Receipt",
                type: "datetime",
                nullable: true);
        }
    }
}
