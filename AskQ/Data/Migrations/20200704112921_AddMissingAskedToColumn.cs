using Microsoft.EntityFrameworkCore.Migrations;

namespace AskQ.Data.Migrations
{
    public partial class AddMissingAskedToColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AskedToId",
                table: "Questions",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AskedToId",
                table: "Questions",
                column: "AskedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_AskedToId",
                table: "Questions",
                column: "AskedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_AskedToId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AskedToId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AskedToId",
                table: "Questions");
        }
    }
}
