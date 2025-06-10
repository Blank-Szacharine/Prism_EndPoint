using Prism_EndPoint.Models;
using Prism_EndPoint.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<PrismDbContext>(); 
builder.Services.AddTransient<IqmsProgram, qmsProgram>();
builder.Services.AddTransient<IqmsPlan, qmsPlan>();
builder.Services.AddTransient<IqmsChecklist, qmsChecklist>();
builder.Services.AddTransient<IqmsReport, qmsReport>();
var key = Encoding.UTF8.GetBytes("NpSoHmbjMRpX6SjvQ4wWhnmpvVKdgC0djDMBBBHI4jt2uHghZ5lge3FYtfUlmxke7KXY8LbarZ3zUA7ufqn3P8gGoJbEE0wTqBGXfrdktFJ0DuaO36VKcwYP0x8lDsaI");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
         policy =>
         {
             policy.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
         });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
