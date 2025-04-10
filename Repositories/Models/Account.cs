using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repositories.Models;

public partial class Account
{
    [Key]public string AccountId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public long Phone { get; set; }

    public DateTime? Birthday { get; set; }

    public string Gender { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual ICollection<CustomerWallet> CustomerWallets { get; set; } = new List<CustomerWallet>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    public ICollection<Order> OrderAsOwner1 { get; set; } = new List<Order>();
    public ICollection<Order> OrderAsOwner2 { get; set; } = new List<Order>();
    public virtual ICollection<PayoutHistory> PayoutHistories { get; set; } = new List<PayoutHistory>();
    
}
