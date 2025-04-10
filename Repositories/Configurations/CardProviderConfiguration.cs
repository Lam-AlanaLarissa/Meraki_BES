using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Configurations
{
    public class CardProviderConfiguration : IEntityTypeConfiguration<CardProvider>
    {
        public void Configure(EntityTypeBuilder<CardProvider> builder)
        {
            builder.HasData
            (
                new CardProvider
                {
                    CardProviderName = "VietinBank",
                    CpfullName = "Ngân hàng TMCP Công thương Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "Vietcombank",
                    CpfullName = "Ngân hàng TMCP Ngoại Thương Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "BIDV",
                    CpfullName = "Ngân hàng TMCP Đầu tư và Phát triển Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "Agribank",
                    CpfullName = "Ngân hàng Nông nghiệp và Phát triển Nông thôn Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "OCB",
                    CpfullName = "Ngân hàng TMCP Phương Đông",
                }, new CardProvider
                {
                    CardProviderName = "MBBank",
                    CpfullName = "Ngân hàng TMCP Quân đội",
                }, new CardProvider
                {
                    CardProviderName = "Techcombank",
                    CpfullName = "Ngân hàng TMCP Kỹ thương Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "ACB",
                    CpfullName = "Ngân hàng TMCP Á Châu",
                }, new CardProvider
                {
                    CardProviderName = "VPBank",
                    CpfullName = "Ngân hàng TMCP Việt Nam Thịnh Vượng",
                }, new CardProvider
                {
                    CardProviderName = "TPBank",
                    CpfullName = "Ngân hàng TMCP Tiên Phong",
                }, new CardProvider
                {
                    CardProviderName = "Sacombank",
                    CpfullName = "Ngân hàng TMCP Sài Gòn Thương Tín",
                }, new CardProvider
                {
                    CardProviderName = "HDBank",
                    CpfullName = "Ngân hàng TMCP Phát triển Thành phố Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "VietCapitalBank",
                    CpfullName = "Ngân hàng TMCP Bản Việt",
                }, new CardProvider
                {
                    CardProviderName = "SCB",
                    CpfullName = "Ngân hàng TMCP Sài Gòn",
                }, new CardProvider
                {
                    CardProviderName = "VIB",
                    CpfullName = "Ngân hàng TMCP Quốc tế Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "SHB",
                    CpfullName = "Ngân hàng TMCP Sài Gòn - Hà Nội",
                }, new CardProvider
                {
                    CardProviderName = "Eximbank",
                    CpfullName = "Ngân hàng TMCP Xuất Nhập khẩu Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "MSB",
                    CpfullName = "Ngân hàng TMCP Hàng Hải",
                }, new CardProvider
                {
                    CardProviderName = "CAKE",
                    CpfullName = "TMCP Việt Nam Thịnh Vượng - Ngân hàng số CAKE by VPBank",
                }, new CardProvider
                {
                    CardProviderName = "Ubank",
                    CpfullName = "NgânTMCP Việt Nam Thịnh Vượng - Ngân hàng số Ubank by VPBank",
                }, new CardProvider
                {
                    CardProviderName = "Timo",
                    CpfullName = "Ngân hàng số Timo by Ban Viet Bank (Timo by Ban Viet Bank)",
                }, new CardProvider
                {
                    CardProviderName = "ViettelMoney",
                    CpfullName = "Tổng Công ty Dịch vụ số Viettel - Chi nhánh tập đoàn công nghiệp viễn thông Quân Đội",
                }, new CardProvider
                {
                    CardProviderName = "VNPTMoney",
                    CpfullName = "VNPT Money",
                }, new CardProvider
                {
                    CardProviderName = "SaigonBank",
                    CpfullName = "NgânNgân hàng TMCP Sài Gòn Công Thương",
                }, new CardProvider
                {
                    CardProviderName = "BacABank",
                    CpfullName = "Ngân hàng TMCP Bắc Á",
                }, new CardProvider
                {
                    CardProviderName = "PVcomBank",
                    CpfullName = "Ngân hàng TMCP Đại Chúng Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "Oceanbank",
                    CpfullName = "Ngân hàng Thương mại TNHH MTV Đại Dương",
                }, new CardProvider
                {
                    CardProviderName = "NCB",
                    CpfullName = "Ngân hàng TMCP Quốc Dân",
                }, new CardProvider
                {
                    CardProviderName = "ShinhanBank",
                    CpfullName = "Ngân hàng TNHH MTV Shinhan Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "ABBANK",
                    CpfullName = "Ngân hàng TMCP An Bình",
                }, new CardProvider
                {
                    CardProviderName = "VietABank",
                    CpfullName = "Ngân hàng TMCP Việt Á",
                }, new CardProvider
                {
                    CardProviderName = "NamABank",
                    CpfullName = "Ngân hàng TMCP Nam Á",
                }, new CardProvider
                {
                    CardProviderName = "PGBank",
                    CpfullName = "Ngân hàng TMCP Xăng dầu Petrolimex",
                }, new CardProvider
                {
                    CardProviderName = "VietBank",
                    CpfullName = "Ngân hàng TMCP Việt Nam Thương Tín",
                }, new CardProvider
                {
                    CardProviderName = "BaoVietBank",
                    CpfullName = "Ngân hàng TMCP Bảo Việt",
                }, new CardProvider
                {
                    CardProviderName = "SeABank",
                    CpfullName = "Ngân hàng TMCP Đông Nam Á",
                }, new CardProvider
                {
                    CardProviderName = "COOPBANK",
                    CpfullName = "Ngân hàng Hợp tác xã Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "LienVietPostBank",
                    CpfullName = "Ngân hàng TMCP Bưu Điện Liên Việt",
                }, new CardProvider
                {
                    CardProviderName = "KienLongBank",
                    CpfullName = "Ngân hàng TMCP Kiên Long",
                }, new CardProvider
                {
                    CardProviderName = "KBank",
                    CpfullName = "Ngân hàng Đại chúng TNHH Kasikornbank",
                }, new CardProvider
                {
                    CardProviderName = "KookminHN",
                    CpfullName = "Ngân hàng Kookmin - Chi nhánh Hà Nội",
                }, new CardProvider
                {
                    CardProviderName = "KEBHanaHCM",
                    CpfullName = "Ngân hàng KEB Hana – Chi nhánh Thành phố Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "KEBHANAHN",
                    CpfullName = "Công ty Tài chính TNHH MTV Mirae Asset (Việt Nam)",
                }, new CardProvider
                {
                    CardProviderName = "Citibank",
                    CpfullName = "Ngân hàng Citibank, N.A. - Chi nhánh Hà Nội",
                }, new CardProvider
                {
                    CardProviderName = "KookminHCM",
                    CpfullName = "Ngân hàng Kookmin - Chi nhánh Thành phố Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "VBSP",
                    CpfullName = "Ngân hàng Chính sách Xã hội",
                }, new CardProvider
                {
                    CardProviderName = "Woori",
                    CpfullName = "Ngân hàng TNHH MTV Woori Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "VRB",
                    CpfullName = "Ngân hàng Liên doanh Việt - Nga",
                }, new CardProvider
                {
                    CardProviderName = "UnitedOverseas",
                    CpfullName = "Ngân hàng United Overseas - Chi nhánh TP. Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "StandardChartered",
                    CpfullName = "Ngân hàng TNHH MTV Standard Chartered Bank Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "PublicBank",
                    CpfullName = "Ngân hàng TNHH MTV Public Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "Nonghyup",
                    CpfullName = "Ngân hàng Nonghyup - Chi nhánh Hà Nội",
                }, new CardProvider
                {
                    CardProviderName = "IndovinaBank",
                    CpfullName = "Ngân hàng TNHH Indovina",
                }, new CardProvider
                {
                    CardProviderName = "IBKHCM",
                    CpfullName = "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh TP. Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "IBKHN",
                    CpfullName = "Ngân hàng Công nghiệp Hàn Quốc - Chi nhánh Hà Nội",
                }, new CardProvider
                {
                    CardProviderName = "HSBC",
                    CpfullName = "Ngân hàng TNHH MTV HSBC (Việt Nam)",
                }, new CardProvider
                {
                    CardProviderName = "HongLeong",
                    CpfullName = "Ngân hàng TNHH MTV Hong Leong Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "GPBank",
                    CpfullName = "Ngân hàng Thương mại TNHH MTV Dầu Khí Toàn Cầu",
                }, new CardProvider
                {
                    CardProviderName = "DongABank",
                    CpfullName = "Ngân hàng TMCP Đông Á",
                }, new CardProvider
                {
                    CardProviderName = "DBSBank",
                    CpfullName = "DBS Bank Ltd - Chi nhánh Thành phố Hồ Chí Minh",
                }, new CardProvider
                {
                    CardProviderName = "CIMB",
                    CpfullName = "Ngân hàng TNHH MTV CIMB Việt Nam",
                }, new CardProvider
                {
                    CardProviderName = "CBBank",
                    CpfullName = "Ngân hàng Thương mại TNHH MTV Xây dựng Việt Nam",
                }
            );
        }
    }
}
