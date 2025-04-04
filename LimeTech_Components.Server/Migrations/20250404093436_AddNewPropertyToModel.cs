using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimeTech_Components.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertyToModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerUnit",
                table: "PurchaseHistories",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "PurchaseHistories");
        }
    }
}
