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
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {

            builder.HasData(
                new Account 
                { 
                    AccountId = "AC00000001", 
                    UserName = "user01", 
                    Email = "user01@example.com", 
                    Password = "Pass@123", 
                    FullName = "Nguyễn Văn A", 
                    Address = "Ho Chi Minh", 
                    Phone = 84901234567, 
                    Birthday = new DateTime(1995, 1, 1),
                    Gender = "Nam", 
                    Role = "Customer", 
                    Status = "Active" 
                },
                new Account 
                { 
                    AccountId = "AC00000002", 
                    UserName = "user02", 
                    Email = "user02@example.com", 
                    Password = "Pass@123", 
                    FullName = "Trần Thị B", 
                    Address = "Hanoi", 
                    Phone = 84901234568, 
                    Birthday = new DateTime(1990, 2, 15), 
                    Gender = "Nữ", 
                    Role = "Seller", 
                    Status = "Active" 
                },
                new Account 
                { 
                    AccountId = "AC00000003", 
                    UserName = "user03", 
                    Email = "user03@example.com", 
                    Password = "Pass@123", 
                    FullName = "Lê Văn C", 
                    Address = "Da Nang", 
                    Phone = 84901234569, 
                    Birthday = new DateTime(1988, 5, 20), 
                    Gender = "Nam", 
                    Role = "Customer", 
                    Status = "Inactive" 
                },
                new Account { AccountId = "AC00000004", UserName = "user04", Email = "user04@example.com", Password = "Pass@123", FullName = "Phạm Thị D", Address = "Can Tho", Phone = 84901234570, Birthday = new DateTime(1992, 7, 10), Gender = "Nữ", Role = "Seller", Status = "Active" },
                new Account { AccountId = "AC00000005", UserName = "user05", Email = "user05@example.com", Password = "Pass@123", FullName = "Bùi Văn E", Address = "Nha Trang", Phone = 84901234571, Birthday = new DateTime(1985, 9, 25), Gender = "Nam", Role = "Customer", Status = "Active" },
                new Account { AccountId = "AC00000006", UserName = "user06", Email = "user06@example.com", Password = "Pass@123", FullName = "Đặng Thị F", Address = "Vinh", Phone = 84901234572, Birthday = new DateTime(1993, 12, 30), Gender = "Nữ", Role = "Seller", Status = "Inactive" },
                new Account { AccountId = "AC00000007", UserName = "user07", Email = "user07@example.com", Password = "Pass@123", FullName = "Ngô Văn G", Address = "Hue", Phone = 84901234573, Birthday = new DateTime(1987, 3, 17), Gender = "Nam", Role = "Customer", Status = "Active" },
                new Account { AccountId = "AC00000008", UserName = "user08", Email = "user08@example.com", Password = "Pass@123", FullName = "Tô Thị H", Address = "Bac Giang", Phone = 84901234574, Birthday = new DateTime(1994, 6, 5), Gender = "Nữ", Role = "Seller", Status = "Active" },
                new Account { AccountId = "AC00000009", UserName = "user09", Email = "user09@example.com", Password = "Pass@123", FullName = "Hoàng Văn I", Address = "Quang Ninh", Phone = 84901234575, Birthday = new DateTime(1986, 8, 19), Gender = "Nam", Role = "Customer", Status = "Inactive" },
                new Account { AccountId = "AC00000010", UserName = "user10", Email = "user10@example.com", Password = "Pass@123", FullName = "Vũ Thị J", Address = "Tay Ninh", Phone = 84901234576, Birthday = new DateTime(1991, 11, 11), Gender = "Nữ", Role = "Seller", Status = "Active" }
            );
        }
    }
}
