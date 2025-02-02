using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeuDicionario.Migrations
{
    /// <inheritdoc />
    public partial class AdicaotabelaTextoPalavra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextRefId = table.Column<int>(type: "int", nullable: false),
                    WordRefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextWords_Texts_TextRefId",
                        column: x => x.TextRefId,
                        principalTable: "Texts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextWords_Words_WordRefId",
                        column: x => x.WordRefId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextWords_TextRefId",
                table: "TextWords",
                column: "TextRefId");

            migrationBuilder.CreateIndex(
                name: "IX_TextWords_WordRefId",
                table: "TextWords",
                column: "WordRefId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextWords");
        }
    }
}
