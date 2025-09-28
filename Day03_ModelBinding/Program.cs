var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();

app.MapGet("/products/{id:int}", (int id) =>
{
    return Results.Ok($"poducts {id}");
});

app.Run();
