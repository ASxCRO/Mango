using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new List<Coupon>
            {
                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 40
                },
                new Coupon
                {
                    CouponId = 2,
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20
                }
            });
        }
    }
}
