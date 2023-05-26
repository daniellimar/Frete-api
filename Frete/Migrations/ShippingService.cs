using Microsoft.EntityFrameworkCore.Migrations;

namespace Frete.Migrations
{
	public partial class CreateShippingServiceTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "ShippingServices",
				columns: table => new
				{
					ServiceCode = table.Column<string>(nullable: false),
					ServiceDescription = table.Column<string>(nullable: true),
					Carrier = table.Column<string>(nullable: true),
					CarrierCode = table.Column<string>(nullable: true),
					ShippingPrice = table.Column<string>(nullable: true),
					DeliveryTime = table.Column<string>(nullable: true),
					Error = table.Column<bool>(nullable: false),
					Msg = table.Column<string>(nullable: true),
					OriginalDeliveryTime = table.Column<string>(nullable: true),
					OriginalShippingPrice = table.Column<string>(nullable: true),
					ResponseTime = table.Column<string>(nullable: true),
					AllowBuyLabel = table.Column<bool>(nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ShippingServices", x => x.ServiceCode);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ShippingServices");
		}
	}
}
