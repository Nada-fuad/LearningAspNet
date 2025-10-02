using Day04_Controller_Based.Data;
using Day04_Controller_Based.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();
var app = builder.Build();

app.MapProductsEndpoints();
app.Run();
