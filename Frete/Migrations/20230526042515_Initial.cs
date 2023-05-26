using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frete.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cotacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerCEP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientCEP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingServiceCode = table.Column<int>(type: "int", nullable: false),
                    ShipmentInvoiceValue = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Width = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Height = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RecipientCountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    DateLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cotacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShippingService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Carrier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CarrierCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Error = table.Column<bool>(type: "bit", nullable: false),
                    Msg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalDeliveryTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalShippingPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ResponseTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowBuyLabel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingService", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cotacoes");

            migrationBuilder.DropTable(
                name: "ShippingService");
        }
    }
}
