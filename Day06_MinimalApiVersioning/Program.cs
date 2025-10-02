using Asp.Versioning;
using M05.UrlPathVersionionMinimal.Endpoints.V1;
using M05.UrlPathVersioningMinimal.Data;
using M05.UrlPathVersionionMinimal.Endpoints.V2;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ProductRepository>();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
    //options.ApiVersionReader = new HeaderApiVersionReader("api-version");
    options.ApiVersionReader = new MediaTypeApiVersionReader("v");
});

var app = builder.Build();
var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

app.MapProductEndpointsV1()
   .WithApiVersionSet(apiVersionSet);

app.MapProductEndpointsV2()
   .WithApiVersionSet(apiVersionSet);


app.Run();
