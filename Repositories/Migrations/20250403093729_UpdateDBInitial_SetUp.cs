using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDBInitial_SetUp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "CreatedAt",
                table: "Product",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "Images",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "StockQuantity",
                table: "Product",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    FeedbackId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductID = table.Column<string>(type: "varchar(255)", nullable: false),
                    IsGoodReview = table.Column<bool>(type: "bit", nullable: false),
                    AccountId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_Feedbacks_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Account1Id = table.Column<string>(type: "varchar(255)", nullable: false),
                    Account2Id = table.Column<string>(type: "varchar(255)", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<long>(type: "bigint", nullable: false),
                    TotalMoney = table.Column<double>(type: "float", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Account_Account1Id",
                        column: x => x.Account1Id,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Account_Account2Id",
                        column: x => x.Account2Id,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PayoutHistories",
                columns: table => new
                {
                    PayoutId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    PayoutDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayoutHistories", x => x.PayoutId);
                    table.ForeignKey(
                        name: "FK_PayoutHistories_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Issue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", nullable: false),
                    AccountId = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Reports_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountID");
                    table.ForeignKey(
                        name: "FK_Reports_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProductId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidPrice = table.Column<double>(type: "float", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "DepositInformations",
                columns: table => new
                {
                    DepositId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositInformations", x => x.DepositId);
                    table.ForeignKey(
                        name: "FK_DepositInformations_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepositId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalMoney = table.Column<double>(type: "float", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.CheckConstraint("CHK_DepositId_For_Barter", "(TransactionType ='Buy' AND DepositId IS NULL) OR (TransactionType = 'Barter' AND DepositId IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_Transactions_DepositInformations_DepositId",
                        column: x => x.DepositId,
                        principalTable: "DepositInformations",
                        principalColumn: "DepositId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Account",
                columns: new[] { "AccountID", "Address", "Birthday", "Email", "FullName", "Gender", "Password", "Phone", "Role", "Status", "UserName" },
                values: new object[,]
                {
                    { "AC00000001", "Ho Chi Minh", new DateTime(1995, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "user01@example.com", "Nguyễn Văn A", "Nam", "Pass@123", 84901234567L, "Customer", "Active", "user01" },
                    { "AC00000002", "Hanoi", new DateTime(1990, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "user02@example.com", "Trần Thị B", "Nữ", "Pass@123", 84901234568L, "Seller", "Active", "user02" },
                    { "AC00000003", "Da Nang", new DateTime(1988, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "user03@example.com", "Lê Văn C", "Nam", "Pass@123", 84901234569L, "Customer", "Inactive", "user03" },
                    { "AC00000004", "Can Tho", new DateTime(1992, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "user04@example.com", "Phạm Thị D", "Nữ", "Pass@123", 84901234570L, "Seller", "Active", "user04" },
                    { "AC00000005", "Nha Trang", new DateTime(1985, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "user05@example.com", "Bùi Văn E", "Nam", "Pass@123", 84901234571L, "Customer", "Active", "user05" },
                    { "AC00000006", "Vinh", new DateTime(1993, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "user06@example.com", "Đặng Thị F", "Nữ", "Pass@123", 84901234572L, "Seller", "Inactive", "user06" },
                    { "AC00000007", "Hue", new DateTime(1987, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "user07@example.com", "Ngô Văn G", "Nam", "Pass@123", 84901234573L, "Customer", "Active", "user07" },
                    { "AC00000008", "Bac Giang", new DateTime(1994, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "user08@example.com", "Tô Thị H", "Nữ", "Pass@123", 84901234574L, "Seller", "Active", "user08" },
                    { "AC00000009", "Quang Ninh", new DateTime(1986, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "user09@example.com", "Hoàng Văn I", "Nam", "Pass@123", 84901234575L, "Customer", "Inactive", "user09" },
                    { "AC00000010", "Tay Ninh", new DateTime(1991, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "user10@example.com", "Vũ Thị J", "Nữ", "Pass@123", 84901234576L, "Seller", "Active", "user10" }
                });

            migrationBuilder.InsertData(
                table: "CardProvider",
                columns: new[] { "CardProviderName", "CPFullName" },
                values: new object[,]
                {
                    { "ABBANK", "Ngân hàng TMCP An Bình" },
                    { "ACB", "Ngân hàng TMCP Á Châu" },
                    { "Agribank", "Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam" },
                    { "BacABank", "Ngân hàng TMCP Bắc Á" },
                    { "BaoVietBank", "Ngân hàng TMCP Bảo Việt" },
                    { "BIDV", "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam" },
                    { "CAKE", "TMCP Việt Nam Thịnh Vượng - Ngân hàng số CAKE by VPBank" },
                    { "CBBank", "Ngân hàng Thương mại TNHH MTV Xây dựng Việt Nam" },
                    { "CIMB", "Ngân hàng TNHH MTV CIMB Việt Nam" },
                    { "Citibank", "Ngân hàng Citibank, N.A. - Chi nhánh Hà Nội" },
                    { "COOPBANK", "Ngân hàng Hợp tác xã Việt Nam" },
                    { "DBSBank", "DBS Bank Ltd - Chi nhánh Thành phố Hồ Chí Minh" },
                    { "DongABank", "Ngân hàng TMCP Đông Á" },
                    { "Eximbank", "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam" },
                    { "GPBank", "Ngân hàng Thương mại TNHH MTV Dầu Khí Toàn Cầu" },
                    { "HDBank", "Ngân hàng TMCP Phát triển Thành phố Hồ Chí Minh" },
                    { "HongLeong", "Ngân hàng TNHH MTV Hong Leong Việt Nam" },
                    { "HSBC", "Ngân hàng TNHH MTV HSBC (Việt Nam)" },
                    { "IBKHCM", "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh TP. Hồ Chí Minh" },
                    { "IBKHN", "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh Hà Nội" },
                    { "IndovinaBank", "Ngân hàng TNHH Indovina" },
                    { "KBank", "Ngân hàng Đại chúng TNHH Kasikornbank" },
                    { "KEBHanaHCM", "Ngân hàng KEB Hana – Chi nhánh Thành phố Hồ Chí Minh" },
                    { "KEBHANAHN", "Công ty Tài chính TNHH MTV Mirae Asset (Việt Nam)" },
                    { "KienLongBank", "Ngân hàng TMCP Kiên Long" },
                    { "KookminHCM", "Ngân hàng Kookmin - Chi nhánh Thành phố Hồ Chí Minh" },
                    { "KookminHN", "Ngân hàng Kookmin - Chi nhánh Hà Nội" },
                    { "LienVietPostBank", "Ngân hàng TMCP Bưu Điện Liên Việt" },
                    { "MBBank", "Ngân hàng TMCP Quân đội" },
                    { "MSB", "Ngân hàng TMCP Hàng Hải" },
                    { "NamABank", "Ngân hàng TMCP Nam Á" },
                    { "NCB", "Ngân hàng TMCP Quốc Dân" },
                    { "Nonghyup", "Ngân hàng Nonghyup - Chi nhánh Hà Nội" },
                    { "OCB", "Ngân hàng TMCP Phương Đông" },
                    { "Oceanbank", "Ngân hàng Thương mại TNHH MTV Đại Dương" },
                    { "PGBank", "Ngân hàng TMCP Xăng dầu Petrolimex" },
                    { "PublicBank", "Ngân hàng TNHH MTV Public Việt Nam" },
                    { "PVcomBank", "Ngân hàng TMCP Đại Chúng Việt Nam" },
                    { "Sacombank", "Ngân hàng TMCP Sài Gòn Thương Tín" },
                    { "SaigonBank", "NgânNgân hàng TMCP Sài Gòn Công Thương" },
                    { "SCB", "Ngân hàng TMCP Sài Gòn" },
                    { "SeABank", "Ngân hàng TMCP Đông Nam Á" },
                    { "SHB", "Ngân hàng TMCP Sài Gòn - Hà Nội" },
                    { "ShinhanBank", "Ngân hàng TNHH MTV Shinhan Việt Nam" },
                    { "StandardChartered", "Ngân hàng TNHH MTV Standard Chartered Bank Việt Nam" },
                    { "Techcombank", "Ngân hàng TMCP Kỹ thương Việt Nam" },
                    { "Timo", "Ngân hàng số Timo by Ban Viet Bank (Timo by Ban Viet Bank)" },
                    { "TPBank", "Ngân hàng TMCP Tiên Phong" },
                    { "Ubank", "NgânTMCP Việt Nam Thịnh Vượng - Ngân hàng số Ubank by VPBank" },
                    { "UnitedOverseas", "Ngân hàng United Overseas - Chi nhánh TP. Hồ Chí Minh" },
                    { "VBSP", "Ngân hàng Chính sách Xã hội" },
                    { "VIB", "Ngân hàng TMCP Quốc tế Việt Nam" },
                    { "VietABank", "Ngân hàng TMCP Việt Á" },
                    { "VietBank", "Ngân hàng TMCP Việt Nam Thương Tín" },
                    { "VietCapitalBank", "Ngân hàng TMCP Bản Việt" },
                    { "Vietcombank", "Ngân hàng TMCP Ngoại Thương Việt Nam" },
                    { "VietinBank", "Ngân hàng TMCP Công thương Việt Nam" },
                    { "ViettelMoney", "Tổng Công ty Dịch vụ số Viettel - Chi nhánh tập đoàn công nghiệp viễn thông Quân Đội" },
                    { "VNPTMoney", "VNPT Money" },
                    { "VPBank", "Ngân hàng TMCP Việt Nam Thịnh Vượng" },
                    { "VRB", "Ngân hàng Liên doanh Việt - Nga" },
                    { "Woori", "Ngân hàng TNHH MTV Woori Việt Nam" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "PCateID", "PCateDesc", "PCateName", "PCateStatus" },
                values: new object[,]
                {
                    { "PC00000001", "Các loại điện thoại di động", "Điện thoại", "Active" },
                    { "PC00000002", "Các loại máy tính xách tay", "Laptop", "Active" },
                    { "PC00000003", "Quần áo, giày dép nam", "Thời trang nam", "Active" },
                    { "PC00000004", "Quần áo, giày dép nữ", "Thời trang nữ", "Active" },
                    { "PC00000005", "Sản phẩm cho gia đình", "Đồ gia dụng", "Active" },
                    { "PC00000006", "Các thiết bị điện tử", "Đồ điện tử", "Active" },
                    { "PC00000007", "Các loại sách", "Sách", "Active" },
                    { "PC00000008", "Đồ chơi trẻ em", "Đồ chơi", "Active" },
                    { "PC00000009", "Tai nghe, bàn phím, chuột", "Phụ kiện công nghệ", "Active" },
                    { "PC00000010", "Các loại xe đạp, xe máy", "Xe cộ", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Customer",
                columns: new[] { "CustomerID", "AccountID", "Avatar", "CardName", "CardNumber", "CardProviderName", "TaxNumber" },
                values: new object[,]
                {
                    { "C000000001", "AC00000001", null, "Visa Platinum", 1234567890123456L, "Vietcombank", 123456789L },
                    { "C000000002", "AC00000003", null, "MasterCard Gold", 9876543210987654L, "BIDV", 987654321L },
                    { "C000000003", "AC00000005", null, "JCB Standard", 4561237894561237L, "Techcombank", 456123789L },
                    { "C000000004", "AC00000007", null, "Visa Infinite", 7418529638527418L, "Sacombank", 741852963L },
                    { "C000000005", "AC00000009", null, "MasterCard Standard", 3692581473692581L, "MBBank", 369258147L }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "AccountID", "CreatedAt", "CreatedBy", "DamageDetail", "DeletedAt", "DeletedBy", "Discount", "Location", "PCateID", "PercentageOfDamage", "ProductDesc", "ProductName", "ProductPrice", "PurchaseDate", "PurchaseType", "Status", "StockQuantity", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { "P000000001", "AC00000002", new DateOnly(1, 1, 1), "AC00000002", null, null, null, 10.00m, "Ho Chi Minh", "PC00000001", 10.00m, "Điện thoại Apple iPhone 14 mới", "iPhone 14", 20000000.00m, new DateOnly(2024, 1, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000002", "AC00000004", new DateOnly(1, 1, 1), "AC00000004", null, null, null, 5.00m, "Hanoi", "PC00000002", 20.00m, "Laptop Dell XPS 13 2023", "Dell XPS 13", 35000000.00m, new DateOnly(2024, 2, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000003", "AC00000006", new DateOnly(1, 1, 1), "AC00000006", null, null, null, 20.00m, "Can Tho", "PC00000003", 15.00m, "Giày thể thao Nike Air Jordan", "Giày Nike Air", 5000000.00m, new DateOnly(2024, 3, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000004", "AC00000008", new DateOnly(1, 1, 1), "AC00000008", null, null, null, 15.00m, "Da Nang", "PC00000004", 5.00m, "Áo thun nữ cotton", "Áo thun nữ", 300000.00m, new DateOnly(2024, 4, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000005", "AC00000010", new DateOnly(1, 1, 1), "AC00000010", null, null, null, 10.00m, "Hue", "PC00000005", 40.00m, "Nồi cơm điện Toshiba", "Nồi cơm điện", 2000000.00m, new DateOnly(2024, 5, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000006", "AC00000002", new DateOnly(1, 1, 1), "AC00000002", null, null, null, 5.00m, "Quang Ninh", "PC00000006", 50.00m, "Tai nghe Sony WH-1000XM4", "Tai nghe Sony", 8000000.00m, new DateOnly(2024, 6, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000007", "AC00000004", new DateOnly(1, 1, 1), "AC00000004", null, null, null, 0.00m, "Nha Trang", "PC00000007", 12.00m, "Sách Python cho người mới", "Sách lập trình", 250000.00m, new DateOnly(2024, 7, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000008", "AC00000006", new DateOnly(1, 1, 1), "AC00000006", null, null, null, 10.00m, "Vinh", "PC00000008", 35.00m, "Bộ xếp hình lego 1000 mảnh", "Bộ lego", 1200000.00m, new DateOnly(2024, 8, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000009", "AC00000008", new DateOnly(1, 1, 1), "AC00000008", null, null, null, 5.00m, "Hanoi", "PC00000009", 44.00m, "Bàn phím cơ RGB", "Bàn phím cơ", 1500000.00m, new DateOnly(2024, 9, 1), "Mua ngay", "Active", 1.0, null, null },
                    { "P000000010", "AC00000010", new DateOnly(1, 1, 1), "AC00000010", null, null, null, 10.00m, "Tay Ninh", "PC00000010", 10.00m, "Xe đạp địa hình", "Xe đạp thể thao", 5000000.00m, new DateOnly(2024, 10, 1), "Mua ngay", "Active", 1.0, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepositInformations_TransactionId",
                table: "DepositInformations",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositInformations_UserId",
                table: "DepositInformations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AccountId",
                table: "Feedbacks",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ProductID",
                table: "Feedbacks",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Account1Id",
                table: "Orders",
                column: "Account1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Account2Id",
                table: "Orders",
                column: "Account2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PayoutHistories_AccountId",
                table: "PayoutHistories",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AccountId",
                table: "Reports",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProductId",
                table: "Reports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DepositId",
                table: "Transactions",
                column: "DepositId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OrderId",
                table: "Transactions",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositInformations_Transactions_TransactionId",
                table: "DepositInformations",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "TransactionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositInformations_Transactions_TransactionId",
                table: "DepositInformations");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PayoutHistories");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "DepositInformations");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "ABBANK");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "ACB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Agribank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "BacABank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "BaoVietBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "CAKE");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "CBBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "CIMB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Citibank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "COOPBANK");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "DBSBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "DongABank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Eximbank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "GPBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "HDBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "HongLeong");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "HSBC");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "IBKHCM");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "IBKHN");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "IndovinaBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KEBHanaHCM");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KEBHANAHN");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KienLongBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KookminHCM");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "KookminHN");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "LienVietPostBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "MSB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "NamABank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "NCB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Nonghyup");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "OCB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Oceanbank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "PGBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "PublicBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "PVcomBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "SaigonBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "SCB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "SeABank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "SHB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "ShinhanBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "StandardChartered");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Timo");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "TPBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Ubank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "UnitedOverseas");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VBSP");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VIB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VietABank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VietBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VietCapitalBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VietinBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "ViettelMoney");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VNPTMoney");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VPBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "VRB");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Woori");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerID",
                keyValue: "C000000001");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerID",
                keyValue: "C000000002");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerID",
                keyValue: "C000000003");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerID",
                keyValue: "C000000004");

            migrationBuilder.DeleteData(
                table: "Customer",
                keyColumn: "CustomerID",
                keyValue: "C000000005");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000001");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000002");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000003");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000004");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000005");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000006");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000007");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000008");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000009");

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: "P000000010");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000001");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000002");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000003");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000004");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000005");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000006");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000007");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000008");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000009");

            migrationBuilder.DeleteData(
                table: "Account",
                keyColumn: "AccountID",
                keyValue: "AC00000010");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "BIDV");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "MBBank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Sacombank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Techcombank");

            migrationBuilder.DeleteData(
                table: "CardProvider",
                keyColumn: "CardProviderName",
                keyValue: "Vietcombank");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000001");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000002");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000003");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000004");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000005");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000006");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000007");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000008");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000009");

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "PCateID",
                keyValue: "PC00000010");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Images",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Product");
        }
    }
}
