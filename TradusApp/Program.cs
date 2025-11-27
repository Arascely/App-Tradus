using Microsoft.EntityFrameworkCore;
using TradusApp.Domain.Abstractions;
using TradusApp.Infrastructure.Persistence;
using TradusApp.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// DbContext registration (SQL Server)
builder.Services.AddDbContext<TradusAppDbContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// UoW
builder.Services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(sp.GetRequiredService<TradusAppDbContext>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
 app.UseExceptionHandler("/Error");
 app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Root -> Books/Manage
app.MapGet("/", () => Results.Redirect("/Books/Manage"));

app.MapRazorPages();

app.Run();
