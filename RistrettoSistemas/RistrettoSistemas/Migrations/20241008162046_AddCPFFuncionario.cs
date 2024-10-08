using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RistrettoSistemas.Migrations
{
    /// <inheritdoc />
    public partial class AddCPFFuncionario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Funcionarios",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Funcionarios_CPF",
                table: "Funcionarios",
                column: "CPF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_NomeEmpresarial",
                table: "Empresas",
                column: "NomeEmpresarial",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Funcionarios_CPF",
                table: "Funcionarios");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_NomeEmpresarial",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Funcionarios");
        }
    }
}
