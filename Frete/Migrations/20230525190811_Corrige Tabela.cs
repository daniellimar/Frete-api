using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Frete.Migrations
{
    /// <inheritdoc />
    public partial class CorrigeTabela : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Frete",
                table: "Frete");

            migrationBuilder.DropColumn(
                name: "DataUltimaAtualizacao",
                table: "Frete");

            migrationBuilder.RenameTable(
                name: "Frete",
                newName: "Cotacoes");

            migrationBuilder.RenameColumn(
                name: "ValorRemessa",
                table: "Cotacoes",
                newName: "Width");

            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "Cotacoes",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "Peso",
                table: "Cotacoes",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "Largura",
                table: "Cotacoes",
                newName: "ShipmentInvoiceValue");

            migrationBuilder.RenameColumn(
                name: "Comprimento",
                table: "Cotacoes",
                newName: "Length");

            migrationBuilder.RenameColumn(
                name: "CodigoServicoEnvio",
                table: "Cotacoes",
                newName: "ShippingServiceCode");

            migrationBuilder.RenameColumn(
                name: "CepOrigem",
                table: "Cotacoes",
                newName: "SellerCEP");

            migrationBuilder.RenameColumn(
                name: "CepDestino",
                table: "Cotacoes",
                newName: "RecipientCountry");

            migrationBuilder.RenameColumn(
                name: "Altura",
                table: "Cotacoes",
                newName: "Height");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Cotacoes",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateLastUpdate",
                table: "Cotacoes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecipientCEP",
                table: "Cotacoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cotacoes",
                table: "Cotacoes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cotacoes",
                table: "Cotacoes");

            migrationBuilder.DropColumn(
                name: "DateLastUpdate",
                table: "Cotacoes");

            migrationBuilder.DropColumn(
                name: "RecipientCEP",
                table: "Cotacoes");

            migrationBuilder.RenameTable(
                name: "Cotacoes",
                newName: "Frete");

            migrationBuilder.RenameColumn(
                name: "Width",
                table: "Frete",
                newName: "ValorRemessa");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Frete",
                newName: "Peso");

            migrationBuilder.RenameColumn(
                name: "ShippingServiceCode",
                table: "Frete",
                newName: "CodigoServicoEnvio");

            migrationBuilder.RenameColumn(
                name: "ShipmentInvoiceValue",
                table: "Frete",
                newName: "Largura");

            migrationBuilder.RenameColumn(
                name: "SellerCEP",
                table: "Frete",
                newName: "CepOrigem");

            migrationBuilder.RenameColumn(
                name: "RecipientCountry",
                table: "Frete",
                newName: "CepDestino");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Frete",
                newName: "Quantidade");

            migrationBuilder.RenameColumn(
                name: "Length",
                table: "Frete",
                newName: "Comprimento");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Frete",
                newName: "Altura");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Frete",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataUltimaAtualizacao",
                table: "Frete",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Frete",
                table: "Frete",
                column: "Id");
        }
    }
}
