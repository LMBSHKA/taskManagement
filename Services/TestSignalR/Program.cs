
using System.Net.Http.Json;

var connection = new HubConnectionBuilder()
	.WithUrl("https://ваш-сервер/myHub")
	.Build();

connection.On<string>("ReceiveMessage", message =>
	Console.WriteLine($"SignalR: {message}"));

await connection.StartAsync();
Console.WriteLine("Подключено к SignalR. Нажмите Enter для отправки POST...");
Console.ReadLine();

// Отправка POST-запроса (используйте HttpClient или Postman)
var httpClient = new HttpClient();
var response = await httpClient.PostAsJsonAsync(
	"https://ваш-сервер/api/my",
	new { Data = "Тест из консоли" }
);
Console.WriteLine(await response.Content.ReadAsStringAsync());