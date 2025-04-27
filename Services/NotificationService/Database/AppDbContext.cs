using Microsoft.EntityFrameworkCore;
using NotificationService.Models;

namespace NotificationService.Database
{
	public class AppDbContext : DbContext
	{
		public DbSet<Notification> Notifications { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
	}
}
