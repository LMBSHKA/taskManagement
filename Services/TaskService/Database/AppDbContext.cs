using Microsoft.EntityFrameworkCore;
using TaskService.Models;

namespace TaskService.Database
{
	public class AppDbContext : DbContext
	{
		public DbSet<Job> Jobs { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	}
}
