using AuthService.Models;

namespace AuthService.Database
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
			if (!context.Users.Any())
			{
				context.Users.AddRange(
					new User("Vasya", "Pupkin", "1234", "vas.pup")
					);

				context.SaveChanges();
			}
		}
	}
}
