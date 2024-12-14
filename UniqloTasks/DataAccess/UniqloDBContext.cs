using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniqloTasks.Models;
namespace UniqloTasks.DataAccess
{

	public class UniqloDbContext : IdentityDbContext
    {
		private readonly UniqloDbContext _context;
		public DbSet<Slider> Sliders { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage>? Images { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<ProductRating>? ProductRatings { get; set; }



		public UniqloDbContext(DbContextOptions opt) : base(opt) { }
		
	}
}
