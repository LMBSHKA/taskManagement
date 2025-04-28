using Microsoft.EntityFrameworkCore;
using TaskService.Database;
using TaskService.Repositories;
using TaskService.Requests;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddScoped<IJobRepo, JobRepo>();
		builder.Services.AddScoped<IRequestsToNotificationService, RequestsToNotificationService>();

		//Connect Db
		builder.Services.AddDbContext<AppDbContext>(opt =>
		{
			opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
		});

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}