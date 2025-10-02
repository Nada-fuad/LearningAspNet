using M01.UrlPathVersioningController.Data;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ProductRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    //options.ApiVersionReader = new UrlSegmentApiVersionReader();//api/v1/product
    //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    options.ApiVersionReader = new MediaTypeApiVersionReader("v");
});
var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
