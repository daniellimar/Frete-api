using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frete.Migrations
{
	/// <inheritdoc />
	public partial class AddForeignKeyConstraint : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddForeignKey(
				name: "FK_ShippingService_Cotacao",
				table: "ShippingService",
				column: "CotacaoId",
				principalTable: "Cotacao",
				principalColumn: "Id",
				onDelete: ReferentialAction.NoAction);
		}


		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{

		}
	}
}
