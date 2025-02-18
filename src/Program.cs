using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PatientApi.Features.Auth.Interfaces;
using PatientApi.Features.Auth.Services;
using PatientApi.Features.Patient.Interfaces;
using PatientApi.Features.Patient.Repositories;
using PatientApi.Features.Patient.Services;
using PatientApi.Infrastructure.Security;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET")
    ?? throw new InvalidOperationException("Missing JWT_SECRET!");
var dbConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION")
    ?? throw new InvalidOperationException("Missing DATABASE_CONNECTION!");
var encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")
    ?? throw new InvalidOperationException("Missing ENCRYPTION_KEY!");

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEncryptionHelper, EncryptionHelper>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateAudience = false
        };
    });

var app = builder.Build();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();