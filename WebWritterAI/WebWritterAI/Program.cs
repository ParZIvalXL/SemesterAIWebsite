using DataBase;
using DataBase.Configurations;
using DataBase.Models;
using DataBase.Repository;
using Microsoft.EntityFrameworkCore;
using WebWritterAI.Middleware;
using WebWritterAI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BlogConfigurator>();
builder.Services.AddScoped<PricingConfiguration>();
builder.Services.AddScoped<UseCaseConfiguration>();

builder.Services.AddScoped<BlogModel>();
builder.Services.AddScoped<PricingModel>();
builder.Services.AddScoped<UseCaseModel>();

builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<PricingRepository>();
builder.Services.AddScoped<UseCaseRepository>();

builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<UseCaseService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();