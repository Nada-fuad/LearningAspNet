var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
//vom Framework Built

//Custom Middleware
//Die Antwort-Header und StatusCode  können nicht mehr geändert werden, da die Antwort bereits gestartet wurde.

//auch wenn es in eine andere funktion  nicht nach next() ändern, ContentLength muss gleich sein;
//app.Use((RequestDelegate next) =>
//{
//    return async (HttpContext context) =>
//    {

//        await context.Response.WriteAsync("my MiddleWare");

//        context.Response.StatusCode = StatusCodes.Status206PartialContent;
//        context.Response.Headers.Append("version","x1");
//        await next(context);


//    };
//});


app.Use((RequestDelegate next) =>
{
    return async (HttpContext context) =>
    {
//        context.Response.StatusCode = StatusCodes.Status206PartialContent;
        context.Response.Headers.Append("version", "x1");

        //await context.Response.WriteAsync("my MiddleWare");

       
        await next(context);


    };
});

app.Use(async
    (context, next) =>
{
    if (!context.Response.HasStarted)
    {
        context.Response.StatusCode = StatusCodes.Status206PartialContent;
    }

    //await context.Response.WriteAsync("2");
    await next();
});



app.Use(async (context, next) =>
{
   await context.Response.WriteAsync(" Vor Erste Middle");
    await next();

    await context.Response.WriteAsync(" Nach Erste Middle");


});


app.Use(async (context, next) =>
{
    await context.Response.WriteAsync(" Vor Zweite Middle");
    await next();

    await context.Response.WriteAsync(" Nach Zweite Middle");


});

static void GetCommonBranch(IApplicationBuilder app) {
    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My First Middleware");
        await next();
    });

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My Second Middleware");
        await next();
    });

};

static void GetBranch1(IApplicationBuilder app) {

    GetCommonBranch(app);

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My Third Middleware");
        await next();
    });

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My 4Th Middleware");
        await next();
    });

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("die erste Map für Middleware ist fertig");
    });
};


static void GetBranch2(IApplicationBuilder app)
{
    GetCommonBranch(app);

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My 6Th Middleware");
        await next();
    });

    app.Use(async (context, next) =>
    {
        await context.Response.WriteAsync("My 7Th Middleware");
        await next();
    });

    app.Run(async (context) =>
    {
        await context.Response.WriteAsync("die zweite Map für Middleware ist fertig");
    });

};
app.Map("/branch1", GetBranch1);

app.Map("/branch2",GetBranch2);
app.MapWhen(context =>
context.Request.Path.Equals("/process", StringComparison.OrdinalIgnoreCase) &&
 context.Request.Query["mood"] == "new",
    b =>
    {
        b.Run ( async context =>
        {
            await context.Response.WriteAsync("process ");
        });
   
});

app.UseWhen(context => context.Request.Path.Equals("/usewhen", StringComparison.OrdinalIgnoreCase),
b => {

    b.Run(async context =>
    {
        await context.Response.WriteAsync("usewhen ");
    });
});
app.Run(async context =>
{
    await context.Response.WriteAsync("We are Completed");

});

app.Run();
