using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hospital.Migrations
{
    /// <inheritdoc />
    public partial class CheckupImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Checkups");

            migrationBuilder.CreateTable(
                name: "CheckupImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CheckupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckupImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckupImages_Checkups_CheckupId",
                        column: x => x.CheckupId,
                        principalTable: "Checkups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckupImages_CheckupId",
                table: "CheckupImages",
                column: "CheckupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckupImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Checkups",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
