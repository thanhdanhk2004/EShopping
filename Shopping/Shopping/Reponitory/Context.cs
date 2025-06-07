using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Reponitory
{
    public class Context : IdentityDbContext<AppUserModel>
    {
        public Context() { }

        public Context(DbContextOptions<Context> options) : base(options) { }

        public virtual DbSet<BrandModel> Brands { get; set; }
        public virtual DbSet<CategoryModel> Categories { get; set; }
        public virtual DbSet<ProductModel> Products { get; set; }
        public virtual DbSet<OrderModel> Orders { get; set; }
        public virtual DbSet<OrderDetailModel> OrderDetails { get; set; }
        public virtual DbSet<RatingModel> Ratings { get; set; }
        public virtual DbSet<ContactModel> Contacts { get; set; }
        public virtual DbSet<CompareModel> Compares { get; set; }
        public virtual DbSet<WishListModel> WishList { get; set; }
    }
}
