using System.Text.Json.Serialization;
using BookManagementSystemAuthorizationSystem.Contracts;
using Library_Web_Application.Extensions;
using Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureIdentity();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureExceptionHandler();

app.MapControllers();
app.MapRazorPages();

app.Run();

