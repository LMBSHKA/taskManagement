using TaskService.Models;

namespace TaskService.Database
{
	public class PrepDb
	{
		public static void PrepPopulation(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.CreateScope())
			{
				SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
			}
		}

		public static void SeedData(AppDbContext context)
		{
			if (!context.Jobs.Any())
			{
				context.Jobs.AddRange(
					new Job("test1", Status.Created, "Vasay", "test test", new DateTime(2025, 4, 30), Priority.Major, false)
					);

				context.SaveChanges();
			}
		}
	}
}
