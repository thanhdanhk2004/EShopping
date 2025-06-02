using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Reponitory
{
    public class Context : IdentityDbContext   
    {
        public Context() { }

        public Context(DbContextOptions<Context> options) : base(options) { }

        public virtual DbSet<BrandModel> Brands { get; set; }
        public virtual DbSet<CategoryModel> Categories { get; set; }
        public virtual DbSet<ProductModel> Products { get; set; }


    }
}
