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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasData
            (
                new Product
                {
                    ProductId = "P000000001",
                    AccountId = "AC00000002",
                    PcateId = "PC00000001",
                    ProductName = "iPhone 14",
                    ProductDesc = "Điện thoại Apple iPhone 14 mới",
                    ProductPrice = 20000000.00m,
                    Discount = 10.00m,
                    Location = "Ho Chi Minh",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 1, 1),
                    PercentageOfDamage = 10.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000002",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000002",
                    AccountId = "AC00000004",
                    PcateId = "PC00000002",
                    ProductName = "Dell XPS 13",
                    ProductDesc = "Laptop Dell XPS 13 2023",
                    ProductPrice = 35000000.00m,
                    Discount = 5.00m,
                    Location = "Hanoi",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 2, 1),
                    PercentageOfDamage = 20.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000004",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000003",
                    AccountId = "AC00000006",
                    PcateId = "PC00000003",
                    ProductName = "Giày Nike Air",
                    ProductDesc = "Giày thể thao Nike Air Jordan",
                    ProductPrice = 5000000.00m,
                    Discount = 20.00m,
                    Location = "Can Tho",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 3, 1),
                    PercentageOfDamage = 15.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000006",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000004",
                    AccountId = "AC00000008",
                    PcateId = "PC00000004",
                    ProductName = "Áo thun nữ",
                    ProductDesc = "Áo thun nữ cotton",
                    ProductPrice = 300000.00m,
                    Discount = 15.00m,
                    Location = "Da Nang",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 4, 1),
                    PercentageOfDamage = 5.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000008",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000005",
                    AccountId = "AC00000010",
                    PcateId = "PC00000005",
                    ProductName = "Nồi cơm điện",
                    ProductDesc = "Nồi cơm điện Toshiba",
                    ProductPrice = 2000000.00m,
                    Discount = 10.00m,
                    Location = "Hue",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 5, 1),
                    PercentageOfDamage = 40.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000010",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000006",
                    AccountId = "AC00000002",
                    PcateId = "PC00000006",
                    ProductName = "Tai nghe Sony",
                    ProductDesc = "Tai nghe Sony WH-1000XM4",
                    ProductPrice = 8000000.00m,
                    Discount = 5.00m,
                    Location = "Quang Ninh",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 6, 1),
                    PercentageOfDamage = 50.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000002",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000007",
                    AccountId = "AC00000004",
                    PcateId = "PC00000007",
                    ProductName = "Sách lập trình",
                    ProductDesc = "Sách Python cho người mới",
                    ProductPrice = 250000.00m,
                    Discount = 0.00m,
                    Location = "Nha Trang",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 7, 1),
                    PercentageOfDamage = 12.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000004",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000008",
                    AccountId = "AC00000006",
                    PcateId = "PC00000008",
                    ProductName = "Bộ lego",
                    ProductDesc = "Bộ xếp hình lego 1000 mảnh",
                    ProductPrice = 1200000.00m,
                    Discount = 10.00m,
                    Location = "Vinh",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 8, 1),
                    PercentageOfDamage = 35.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000006",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000009",
                    AccountId = "AC00000008",
                    PcateId = "PC00000009",
                    ProductName = "Bàn phím cơ",
                    ProductDesc = "Bàn phím cơ RGB",
                    ProductPrice = 1500000.00m,
                    Discount = 5.00m,
                    Location = "Hanoi",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 9, 1),
                    PercentageOfDamage = 44.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000008",
                    Status = "Active"
                },
                new Product
                {
                    ProductId = "P000000010",
                    AccountId = "AC00000010",
                    PcateId = "PC00000010",
                    ProductName = "Xe đạp thể thao",
                    ProductDesc = "Xe đạp địa hình",
                    ProductPrice = 5000000.00m,
                    Discount = 10.00m,
                    Location = "Tay Ninh",
                    PurchaseType = "Mua ngay",
                    PurchaseDate = new DateOnly(2024, 10, 1),
                    PercentageOfDamage = 10.00m,
                    DamageDetail = null,
                    CreatedBy = "AC00000010",
                    Status = "Active"
                }
            );
        }
    }
}
