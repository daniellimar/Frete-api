using Microsoft.EntityFrameworkCore.Migrations;

namespace Frete.Migrations
{
	public partial class CreateShippingServiceTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ShippingService",
				columns: table => new
				{
					Id = table.Column<int>(nullable: false),
					ServiceCode = table.Column<int>(nullable: false),
					ServiceDescription = table.Column<string>(nullable: true),
					Carrier = table.Column<string>(nullable: true),
					CarrierCode = table.Column<int>(nullable: false),
					ShippingPrice = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					DeliveryTime = table.Column<string>(nullable: true),
					Error = table.Column<bool>(nullable: false),
					Msg = table.Column<string>(nullable: true),
					OriginalDeliveryTime = table.Column<string>(nullable: true),
					OriginalShippingPrice = table.Column<decimal>(type: "decimal(10, 2)", nullable: false),
					ResponseTime = table.Column<string>(nullable: true),
					AllowBuyLabel = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ShippingService", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ShippingService");
		}
	}
}