
using BookStoreTM.Models;
using BookStoreTM.Models.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookStoreTM.Models.Entities
{
    public class AppDbContext : IdentityDbContext<AppUserModel>
    {
       
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<News> News { get; set; }
        //public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Account> Accounts { get; set; }
        //public DbSet<Adv> Advs { get; set; }
        //public DbSet<CommonAbstract> CommonAbstracts { get; set; }
        public DbSet<OrderBook> OrderBooks { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //public DbSet<PaymentDetails> PaymentDetails { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<TransactStatus> TransactStatus { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ReceiptDetails> ReceiptDetails { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<UserCart> UserCarts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thêm ràng buộc unique cho các thực thể của bạn
            modelBuilder.Entity<Customer>()
                .HasIndex(c => new { c.Fullname, c.Email })
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Code)
                .IsUnique();
        }
    }
}
