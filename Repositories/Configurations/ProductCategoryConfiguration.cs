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
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasData(
                new ProductCategory { PcateId = "PC00000001", PcateName = "Điện thoại", PcateDesc = "Các loại điện thoại di động", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000002", PcateName = "Laptop", PcateDesc = "Các loại máy tính xách tay", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000003", PcateName = "Thời trang nam", PcateDesc = "Quần áo, giày dép nam", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000004", PcateName = "Thời trang nữ", PcateDesc = "Quần áo, giày dép nữ", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000005", PcateName = "Đồ gia dụng", PcateDesc = "Sản phẩm cho gia đình", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000006", PcateName = "Đồ điện tử", PcateDesc = "Các thiết bị điện tử", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000007", PcateName = "Sách", PcateDesc = "Các loại sách", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000008", PcateName = "Đồ chơi", PcateDesc = "Đồ chơi trẻ em", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000009", PcateName = "Phụ kiện công nghệ", PcateDesc = "Tai nghe, bàn phím, chuột", PcateStatus = "Active" },
                new ProductCategory { PcateId = "PC00000010", PcateName = "Xe cộ", PcateDesc = "Các loại xe đạp, xe máy", PcateStatus = "Active" }
            );
        }
    }
}
