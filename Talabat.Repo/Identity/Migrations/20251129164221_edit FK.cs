using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repo.Identity.Migrations
{
    /// <inheritdoc />
    public partial class editFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss");

            migrationBuilder.DropIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Addresss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss",
                column: "AppUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Addresss",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresss",
                table: "Addresss",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresss_AppUserId",
                table: "Addresss",
                column: "AppUserId",
                unique: true);
        }
    }
}
