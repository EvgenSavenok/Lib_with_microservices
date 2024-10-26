using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AuthPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5148");    
        policy.AllowAnyMethod(); 
        policy.AllowAnyHeader();    
    });
    options.AddPolicy("BooksPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5023");
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

var app = builder.Build();

app.UseCors("AuthPolicy");
app.UseCors("BooksPolicy");

app.UseOcelot().Wait();

app.Run();
