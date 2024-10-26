using BooksManagementService.Extensions;
using Library_Web_Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();

//app.UseHttpsRedirection();

app.UseRouting();

app.ConfigureExceptionHandler();

app.MapControllers();
app.MapRazorPages();

app.Run();
