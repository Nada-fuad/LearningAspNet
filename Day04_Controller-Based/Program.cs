using Day04_Controller_Based.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
