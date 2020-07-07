using Microsoft.EntityFrameworkCore.Migrations;

namespace AskQ.Infrastructure.Data.Migrations
{
    public partial class ReplaceIdentityUserWithGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_AskedFromId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_AspNetUsers_AskedToId",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Replies_UserId",
                table: "Replies");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AskedFromId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_AskedToId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "AskedFromId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AskedToId",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "UserGuid",
                table: "Replies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AskedFromGuid",
                table: "Questions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AskedToGuid",
                table: "Questions",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "AskedFromGuid",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "AskedToGuid",
                table: "Questions");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Replies",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AskedFromId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AskedToId",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Replies_UserId",
                table: "Replies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AskedFromId",
                table: "Questions",
                column: "AskedFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AskedToId",
                table: "Questions",
                column: "AskedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_AskedFromId",
                table: "Questions",
                column: "AskedFromId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_AspNetUsers_AskedToId",
                table: "Questions",
                column: "AskedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Replies_AspNetUsers_UserId",
                table: "Replies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
