using Day02_Routing;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("validMonth", typeof(MonthRouteConstraint));
    options.ConstraintMap["Slugify"] = typeof(SlugifyTransformer);
});
var app = builder.Build();
app.UseRouting();
app.Use(async
    (context, next) =>
{
    var endpoints = context.GetEndpoint()?.DisplayName ?? "keine Endpoint";
    Console.WriteLine("Middleware");
    await next();

});
app.MapControllers();

app.MapGet("/products-minimal", () =>
{
    return Results.Ok(new[]
    {
        "products 1",
        "products 2",
        "products 3"
    });
});


app.MapGet("/route-table", (IServiceProvider sp) =>
{
    var endpoints = sp.GetRequiredService<EndpointDataSource>().Endpoints.Select(p => p.DisplayName);

    return Results.Ok(endpoints);


});

//app.UseRouting();
//app.UseEndpoints(ep =>
//{
//    ep.MapControllers();
//    ep.MapGet("/product-route", () =>
//    {
//        return Results.Ok("Erfolg");
//    });
//});

//Rout Template

app.MapGet("/products/p/{id:int}", (int id) =>
{
    return Results.Ok($"products {id}");
});

app.MapGet("/products/date/{day:int}/{month:int}/{year:int}",(int day,int month, int year) =>
{
    var date = new DateOnly(year, month, day);
    return Results.Ok(date);
});
app.MapGet("/{controller=Home}",(string? controller)=>controller);

app.MapGet("/order/{id?}", (int? id) => $"Order {id}");

app.MapGet("/slug/{*slug}", (string slug) => $"Slug {slug}");
app.MapGet("/month/{m:validMonth}", (int m) => $"month is : {m}"
);

app.MapGet("/order/{id:int}", (int id) =>
{
    return Results.Ok(new
    {
        orderId=id,
        status="Pending"
    });

}).WithName("orderbyId");

app.MapGet("/generate/order", (HttpContext context, LinkGenerator links,int id) =>
{
    var link = links.GetPathByName("orderbyId", new {Id=id});

    return Results.Ok(link);

});

app.MapGet("/order/o/{id:int}", (int id,LinkGenerator links, HttpContext context) =>
{
    var link = links.GetUriByName("EditOrder", new { id }, context.Request.Scheme,context.Request.Host);
    
    return Results.Ok(new
    {
        orderId = id,
        status = "Pending",
        links= new
        {
          self=  new {context.Request.Path},
          updateLink=  new { link}
        }

    });
});

app.MapPut("/order/{id:int}", (int id) => {

    return Results.NoContent();
}).WithName("EditOrder");


app.MapGet("/book/{title:Slugify}", (string title) =>
{
    return Results.Ok(new { title });
}).WithName("bookByTitle");

app.MapGet("/generate", (HttpContext context, LinkGenerator links) =>
{

    var link = links.GetPathByName("bookByTitle", new { title="My Book Is Big" });

    return Results.Ok(link);

});
app.Run();
