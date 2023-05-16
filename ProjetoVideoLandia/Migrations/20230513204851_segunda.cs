using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoVideoLandia.Migrations
{
    /// <inheritdoc />
    public partial class segunda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodigoDeBarras",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "DataAdquirida",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "Preco",
                table: "Filmes");

            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Filmes");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Filmes",
                newName: "Sinopse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sinopse",
                table: "Filmes",
                newName: "Tipo");

            migrationBuilder.AddColumn<string>(
                name: "CodigoDeBarras",
                table: "Filmes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAdquirida",
                table: "Filmes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Preco",
                table: "Filmes",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Situacao",
                table: "Filmes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
