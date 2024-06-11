using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreTM.Migrations
{
    /// <inheritdoc />
    public partial class AddReceiptAndReceiptDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalMoney",
                table: "ReceiptDetails",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Receipt",
                newName: "ReceiptDate");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ReceiptDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameProduct",
                table: "ReceiptDetails",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Receipt",
                type: "ntext",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalMoney",
                table: "Receipt",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "ReceiptDetails");

            migrationBuilder.DropColumn(
                name: "NameProduct",
                table: "ReceiptDetails");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "TotalMoney",
                table: "Receipt");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ReceiptDetails",
                newName: "TotalMoney");

            migrationBuilder.RenameColumn(
                name: "ReceiptDate",
                table: "Receipt",
                newName: "OrderDate");
        }
    }
}
