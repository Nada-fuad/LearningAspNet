using System.ComponentModel.DataAnnotations;
using Day03_ModelBinding;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddXmlSerializerFormatters();
var app = builder.Build();
app.MapControllers();

app.MapGet("/products/{id:int}", (int id) =>
{
    return Results.Ok($"poducts {id}");
});


app.MapGet("/products", (string name, decimal price) =>
{
    return Results.Ok($"{name} :{price}");
});
app.MapGet("/products-minimal", (string name, decimal price) =>
{
    return Results.Ok($"{name} :{price}");
});
app.MapGet("/products/details", ([FromQuery(Name ="page")]int p ,[FromQuery(Name="pageSize")]int ps) =>
{
    return Results.Ok($"page:{p} pageSize: {ps}");

});

app.MapGet("/search-request", ([AsParameters]SearchRequest request) =>
{
    return Results.Ok(request);
});

app.MapGet("/array", ([FromQuery]int[] ids) =>
{
    return Results.Ok(ids);
});

app.MapGet("/date-rang", (DataRangeQuery query) =>
{
    return Results.Ok(query);
});

app.MapPost("/form-minimal", ([FromForm] string name , decimal price) =>
{
    return Results.Ok(new { name, price });
}).DisableAntiforgery();

app.MapPost("/upload-minimal", async (IFormFile file) =>
{
    if (file is null || file.Length == 0)
    {
        return Results.BadRequest("No file uploaded");
    }

    var uploads = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

    if (!Directory.Exists(uploads))
    {
        Directory.CreateDirectory(uploads);
    }

    var path = Path.Combine(uploads, file.FileName);

    using var stream = new FileStream(path, FileMode.Create);
    await file.CopyToAsync(stream);

    return Results.Ok("Uploaded successfully");
}).DisableAntiforgery();


app.MapPost("/body-minimal", (Product product) =>
{
  return  Results.Ok(product);
});


app.MapGet("/cookie-minimal",( HttpContext context) =>
{
    var theme = context.Request.Cookies["theme"];
    var language = context.Request.Cookies["language"];
    var timeZone = context.Request.Cookies["timeZone"];
    return new
    {
        theme= theme,
        language = language,
        timeZone = timeZone
    };
});
    
app.Run();
