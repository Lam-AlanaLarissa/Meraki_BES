using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "AC00000001"),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<long>(type: "bigint", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Account__349DA586A61F0662", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "CardProvider",
                columns: table => new
                {
                    CardProviderName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CPFullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CardProv__3B8DEBCC9CD99C33", x => x.CardProviderName);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    PCateID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "PC00000001"),
                    PCateName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PCateDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PCateStatus = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductC__5DF9FF092873E01E", x => x.PCateID);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    WishID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "W000000001"),
                    ProductID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wishlist__64BA6541F2011F05", x => x.WishID);
                });

            migrationBuilder.CreateTable(
                name: "CustomerWallet",
                columns: table => new
                {
                    WalletID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "CW00000001"),
                    AccountID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(8,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__84D4F92E55E9A551", x => x.WalletID);
                    table.ForeignKey(
                        name: "customerwallet_accountid_foreign",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "C000000001"),
                    AccountID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CardName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    CardNumber = table.Column<long>(type: "bigint", nullable: true),
                    CardProviderName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TaxNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64B8B16F035F", x => x.CustomerID);
                    table.ForeignKey(
                        name: "customer_accountid_foreign",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "customer_cardprovider_foreign",
                        column: x => x.CardProviderName,
                        principalTable: "CardProvider",
                        principalColumn: "CardProviderName");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false, defaultValue: "P000000001"),
                    AccountID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PCateID = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductDesc = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductPrice = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PurchaseType = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PurchaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PercentageOfDamage = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    DamageDetail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    UpdatedBy = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__B40CC6EDEFCE57C4", x => x.ProductID);
                    table.ForeignKey(
                        name: "product_accountid_foreign",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "product_pcateid_foreign",
                        column: x => x.PCateID,
                        principalTable: "ProductCategory",
                        principalColumn: "PCateID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_AccountID",
                table: "Customer",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CardProviderName",
                table: "Customer",
                column: "CardProviderName");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerWallet_AccountID",
                table: "CustomerWallet",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_AccountID",
                table: "Product",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_PCateID",
                table: "Product",
                column: "PCateID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerWallet");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropTable(
                name: "CardProvider");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "ProductCategory");
        }
    }
}
