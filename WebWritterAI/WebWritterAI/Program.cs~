using System.Reflection;
using DataBase;
using DataBase.Configurations;
using DataBase.Models;
using DataBase.Repository;
using FastkartAPI.AuthCheck;
using FastkartAPI.DataBase.Repositories;
using FastkartAPI.Infrastructure.Password;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.EntityFrameworkCore;
using React.AspNet;
using WebWriterAI.Infrastructure.Password;
using WebWritterAI.Middleware;
using WebWritterAI.Services;
using WebWritterAI.Services.Mapping;
using WebWritterAI.Services.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<BlogConfigurator>();
builder.Services.AddScoped<PricingConfiguration>();
builder.Services.AddScoped<UseCaseConfiguration>();
builder.Services.AddScoped<UserModelConfiguration>();

builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<PricingRepository>();
builder.Services.AddScoped<UseCaseRepository>();
builder.Services.AddScoped<UserModelRepository>();
builder.Services.AddScoped<ChatRepository>();
builder.Services.AddScoped<MessageRepository>();

builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PricingService>();
builder.Services.AddScoped<UseCaseService>();

builder.Services.AddScoped<JwtOption>();
builder.Services.AddScoped<JwtProvider>();
builder.Services.Configure<JwtOption>(builder.Configuration.GetSection(nameof(JwtOption)));

builder.Services.AddScoped<PasswordHasher>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MessageService>();

builder.Services.AddAutoMapper(typeof(AutoMapping));
builder.Services.AddAuthOption(builder.Configuration);

builder.Services.AddHttpClient("Ollama", client => 
{
    client.BaseAddress = new Uri(builder.Configuration["Ollama:Url"]);
    client.Timeout = TimeSpan.FromMinutes(10);
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddReact();
builder.Services
    .AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
    .AddChakraCore();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();


app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();