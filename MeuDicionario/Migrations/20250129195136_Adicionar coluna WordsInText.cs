using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuDicionario.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarcolunaWordsInText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WordsInText",
                table: "Texts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordsInText",
                table: "Texts");
        }
    }
}
