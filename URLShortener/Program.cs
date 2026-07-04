using Microsoft.AspNetCore.Authentication.Negotiate;
using URLShortener.Application.Services;
using URLShortener.Domain.Interfaces;
using URLShortener.Infrastructure.Data;
using Sqids;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var webRoot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(webRoot))
{
    Directory.CreateDirectory(webRoot);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddRazorPages();

// services
builder.Services.AddTransient<IHelperMethods, HelperMethods>();
builder.Services.AddTransient<IUrlShorteningService, UrlShorteningService>();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton<SqidsEncoder<long>>();
builder.Services.AddMemoryCache();
//builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
//   .AddNegotiate();

//builder.Services.AddAuthorization(options =>
//{
//    // By default, all incoming requests will be authorized according to the default policy.
//    options.FallbackPolicy = options.DefaultPolicy;
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// Enable serving index.html and static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseAuthorization();

app.MapControllers();

app.Run();
