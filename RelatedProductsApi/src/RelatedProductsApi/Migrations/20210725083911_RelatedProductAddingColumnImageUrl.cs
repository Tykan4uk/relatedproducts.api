using Microsoft.EntityFrameworkCore.Migrations;

namespace RelatedProductsApi.Migrations
{
    public partial class RelatedProductAddingColumnImageUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "RelatedProducts",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "RelatedProducts");
        }
    }
}
