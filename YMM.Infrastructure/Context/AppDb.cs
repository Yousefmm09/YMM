using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YMM.Data.Entities;
using YMM.Data.Entities.Identity;

namespace YMM.Infrastructure.Context
{
    public class AppDb : IdentityDbContext<User>
    {
        // Identity
        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<UserPreferences> UserPreferences => Set<UserPreferences>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<OtpEmail> OtpEmails => Set<OtpEmail>();

        // Catalog
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Category> Categories => Set<Category>();

        // Cart & Wishlist
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();

        // Orders & Shipping
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
        public DbSet<Shipment> Shipments => Set<Shipment>();
        public DbSet<ShipmentTracking> ShipmentTrackings => Set<ShipmentTracking>();

        // Payments
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<SavedPaymentMethod> SavedPaymentMethods => Set<SavedPaymentMethod>();

        // Reviews
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<ReviewHelpful> ReviewHelpfuls => Set<ReviewHelpful>();

        // Coupons
        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponUsage> CouponUsages => Set<CouponUsage>();

        // Notifications & Contact
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

        // Newsletter & Settings
        public DbSet<NewsletterSubscription> NewsletterSubscriptions => Set<NewsletterSubscription>();
        public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDb).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
