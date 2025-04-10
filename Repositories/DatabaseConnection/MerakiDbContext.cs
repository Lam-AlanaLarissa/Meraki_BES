using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Configurations;
using Repositories.Models;

namespace Repositories.DatabaseConnection;

public partial class MerakiDbContext : DbContext
{
    public MerakiDbContext()
    {
    }

    public MerakiDbContext(DbContextOptions<MerakiDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<CardProvider> CardProviders { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerWallet> CustomerWallets { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<DepositInformation> DepositInformations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<PayoutHistory> PayoutHistories { get; set; }
    private string GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();
        var strConn = config["ConnectionStrings:DefaultConnection"];

        return strConn;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=LAPTOP-GFP4TUFC\\HALAM_SE170579;uid=sa;pwd=12345;database= MerakiDB;TrustServerCertificate=True;");

    [Obsolete]
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new CardProviderConfiguration());
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA586A61F0662");

            entity.ToTable("Account");

            entity.Property(e => e.AccountId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("AC00000001")
                .HasColumnName("AccountID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(255);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CardProvider>(entity =>
        {
            entity.HasKey(e => e.CardProviderName).HasName("PK__CardProv__3B8DEBCC9CD99C33");

            entity.ToTable("CardProvider");

            entity.Property(e => e.CardProviderName).HasMaxLength(255);
            entity.Property(e => e.CpfullName)
                .HasMaxLength(255)
                .HasColumnName("CPFullName");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8B16F035F");

            entity.ToTable("Customer");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("C000000001")
                .HasColumnName("CustomerID");
            entity.Property(e => e.AccountId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AccountID");
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CardName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CardProviderName).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Customers)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_accountid_foreign");

            entity.HasOne(d => d.CardProviderNameNavigation).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CardProviderName)
                .HasConstraintName("customer_cardprovider_foreign");
        });

        modelBuilder.Entity<CustomerWallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Customer__84D4F92E55E9A551");

            entity.ToTable("CustomerWallet");

            entity.Property(e => e.WalletId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("CW00000001")
                .HasColumnName("WalletID");
            entity.Property(e => e.AccountId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AccountID");
            entity.Property(e => e.Balance).HasColumnType("decimal(8, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.CustomerWallets)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customerwallet_accountid_foreign");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6EDEFCE57C4");

            entity.ToTable("Product");

            entity.Property(e => e.ProductId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("P000000001")
                .HasColumnName("ProductID");
            entity.Property(e => e.AccountId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AccountID");
            entity.Property(e => e.CreatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.DamageDetail).HasMaxLength(255);
            entity.Property(e => e.DeletedAt).HasColumnType("datetime");
            entity.Property(e => e.DeletedBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Discount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.PcateId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PCateID");
            entity.Property(e => e.PercentageOfDamage).HasColumnType("decimal(6, 2)");
            entity.Property(e => e.ProductDesc).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ProductPrice).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.PurchaseType)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Products)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_accountid_foreign");

            entity.HasOne(d => d.Pcate).WithMany(p => p.Products)
                .HasForeignKey(d => d.PcateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("product_pcateid_foreign");

            entity.Property(p => p.Images)
            .HasDefaultValue("[]"); // Default to an empty JSON array

        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.PcateId).HasName("PK__ProductC__5DF9FF092873E01E");

            entity.ToTable("ProductCategory");

            entity.Property(e => e.PcateId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("PC00000001")
                .HasColumnName("PCateID");
            entity.Property(e => e.PcateDesc)
                .HasMaxLength(255)
                .HasColumnName("PCateDesc");
            entity.Property(e => e.PcateName)
                .HasMaxLength(255)
                .HasColumnName("PCateName");
            entity.Property(e => e.PcateStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PCateStatus");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.WishId).HasName("PK__Wishlist__64BA6541F2011F05");

            entity.ToTable("Wishlist");

            entity.Property(e => e.WishId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValue("W000000001")
                .HasColumnName("WishID");
            entity.Property(e => e.ProductId)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ProductID");
        });

        // for transaction : deposit information
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.DepositInformation)
            .WithMany()
            .HasForeignKey(t => t.DepositId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        ////if transaction type is barter, the DepositId must be not null and vice versa
        modelBuilder.Entity<Transaction>()
            .HasCheckConstraint("CHK_DepositId_For_Barter",
            "(TransactionType ='Buy' AND DepositId IS NULL) OR (TransactionType = 'Barter' AND DepositId IS NOT NULL)");

        // for feedback
        modelBuilder.Entity<Feedback>()
            .HasOne(t => t.Account)
            .WithMany(f => f.Feedbacks)
            .HasForeignKey(c => c.AccountId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Feedback>()
            .HasOne(t => t.Product)
            .WithMany(f => f.Feedbacks)
            .HasForeignKey(t => t.ProductID)
            .OnDelete(DeleteBehavior.Restrict);

        // for report
        modelBuilder.Entity<Report>()
            .HasOne(t => t.Product)
            .WithMany(r => r.Reports)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // for order
        modelBuilder.Entity<Order>()
            .HasOne(t => t.Account1)
            .WithMany(r => r.OrderAsOwner1)
            .HasForeignKey(t => t.Account1Id)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>()
            .HasOne(t => t.Account2)
            .WithMany(r => r.OrderAsOwner2)
            .HasForeignKey(t => t.Account2Id)
            .OnDelete(DeleteBehavior.Restrict);

        // for order detail
        modelBuilder.Entity<OrderDetail>()
            .HasOne(t => t.Product)
            .WithMany(od => od.OrderDetails)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<OrderDetail>()
            .HasOne(t => t.Order)
            .WithMany(od => od.OrderDetails)
            .HasForeignKey(t => t.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        // for payout history
        modelBuilder.Entity<PayoutHistory>()
            .HasOne(t => t.Account)
            .WithMany(ph => ph.PayoutHistories)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.NoAction);


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
