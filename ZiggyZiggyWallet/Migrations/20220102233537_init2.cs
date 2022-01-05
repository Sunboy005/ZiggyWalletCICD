using Microsoft.EntityFrameworkCore.Migrations;

namespace ZiggyZiggyWallet.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TranxTypeId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Transactions");

            migrationBuilder.AddColumn<float>(
                name: "AmountReceived",
                table: "Transactions",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "AmountSent",
                table: "Transactions",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "RecieverCurrency",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderCurrency",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranxType",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbrevation",
                table: "Currencies",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountReceived",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AmountSent",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecieverCurrency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderCurrency",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TranxType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Abbrevation",
                table: "Currencies");

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Transactions",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TranxTypeId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_WalletId",
                table: "Transactions",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
