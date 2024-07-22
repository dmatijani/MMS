using Microsoft.EntityFrameworkCore;
using MMS.Models;

namespace MMS.Data
{
	public class AppDbContext : DbContext
	{

        public IConfiguration _config { get; set; }

        public AppDbContext(IConfiguration config)
		{
			_config = config;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(_config.GetConnectionString("DatabaseConnection"));
		}

		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserData> UserData { get; set; }
		public DbSet<Payment> Payments { get; set; }
	}
}
