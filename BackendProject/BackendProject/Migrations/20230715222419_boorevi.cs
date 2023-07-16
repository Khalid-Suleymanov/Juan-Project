using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendProject.Migrations
{
    public partial class boorevi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_AspNetUsers_AppUserId",
                table: "ProductReview");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview");

            migrationBuilder.RenameTable(
                name: "ProductReview",
                newName: "ProductReviews");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_ProductId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReview_AppUserId",
                table: "ProductReviews",
                newName: "IX_ProductReviews_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_AspNetUsers_AppUserId",
                table: "ProductReviews",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_AspNetUsers_AppUserId",
                table: "ProductReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReviews_Products_ProductId",
                table: "ProductReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductReviews",
                table: "ProductReviews");

            migrationBuilder.RenameTable(
                name: "ProductReviews",
                newName: "ProductReview");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_ProductId",
                table: "ProductReview",
                newName: "IX_ProductReview_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductReviews_AppUserId",
                table: "ProductReview",
                newName: "IX_ProductReview_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductReview",
                table: "ProductReview",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_AspNetUsers_AppUserId",
                table: "ProductReview",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReview_Products_ProductId",
                table: "ProductReview",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
