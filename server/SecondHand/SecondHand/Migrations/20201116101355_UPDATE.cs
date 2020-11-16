using Microsoft.EntityFrameworkCore.Migrations;

namespace SecondHand.Migrations
{
    public partial class UPDATE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commodities_Users_SellerId",
                table: "Commodities");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Users_BuyerId",
                table: "SalesRecords");

            migrationBuilder.AlterColumn<int>(
                name: "CommodityId",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Check",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Commodities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_CommodityId",
                table: "SalesRecords",
                column: "CommodityId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesRecords_SellerId",
                table: "SalesRecords",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commodities_Users_SellerId",
                table: "Commodities",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Commodities_CommodityId",
                table: "SalesRecords",
                column: "CommodityId",
                principalTable: "Commodities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Users_BuyerId",
                table: "SalesRecords",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Users_SellerId",
                table: "SalesRecords",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commodities_Users_SellerId",
                table: "Commodities");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Commodities_CommodityId",
                table: "SalesRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Users_BuyerId",
                table: "SalesRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesRecords_Users_SellerId",
                table: "SalesRecords");

            migrationBuilder.DropIndex(
                name: "IX_SalesRecords_CommodityId",
                table: "SalesRecords");

            migrationBuilder.DropIndex(
                name: "IX_SalesRecords_SellerId",
                table: "SalesRecords");

            migrationBuilder.DropColumn(
                name: "Check",
                table: "SalesRecords");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "SalesRecords");

            migrationBuilder.AlterColumn<int>(
                name: "CommodityId",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                table: "SalesRecords",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "SellerId",
                table: "Commodities",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Commodities_Users_SellerId",
                table: "Commodities",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesRecords_Users_BuyerId",
                table: "SalesRecords",
                column: "BuyerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
