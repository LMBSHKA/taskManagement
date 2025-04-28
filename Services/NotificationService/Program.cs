using Microsoft.EntityFrameworkCore;
using NotificationService.Database;
using NotificationService.Repositories;
using NotificationService.SignalR;

internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddScoped<INotificationRepo, NotificationRepo>();

		//Connect SignalR
		builder.Services.AddSignalR();

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

		app.MapHub<NotificationHub>("/notify");

		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}