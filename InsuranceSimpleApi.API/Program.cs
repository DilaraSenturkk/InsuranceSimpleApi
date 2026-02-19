using FluentValidation;
using FluentValidation.AspNetCore;
using InsuranceSimpleApi.API.Middleware;
using InsuranceSimpleApi.Application.Interfaces;
using InsuranceSimpleApi.Application.Models;
using InsuranceSimpleApi.Application.Services;
using InsuranceSimpleApi.Application.Validators;
using InsuranceSimpleApi.Domain.Models;
using InsuranceSimpleApi.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- CONTROLLERS ---
builder.Services.AddControllers();

// --- FLUENT VALIDATION ---
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterDtoValidator>();

// --- ERROR HANDLING ---
builder.Services.AddScoped<ErrorHandlerMiddleware>();

// --- AUTHENTICATION - JWT ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

// --- DATABASE ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- DEPENDENCY INJECTION (DI) ---

// 1. Database Context Arayüz Eþlemesi
builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

// 2. Kimlik Doðrulama Bilgisi (Middleware için somut sýnýf, servisler için interface)
builder.Services.AddScoped<AuthenticatedUser>();
builder.Services.AddScoped<IAuthenticatedUser>(sp =>
    sp.GetRequiredService<AuthenticatedUser>());

// 3. Servis Katmanlarý (Arayüzleri ile birlikte)
builder.Services.AddScoped<PasswordService>(); 
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// 4. Middleware Kayýtlarý
builder.Services.AddScoped<AuthenticatedUserMiddleware>();

// --- SWAGGER ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token'ý buraya 'Bearer {token}' formatýnda girin."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// --- HTTP REQUEST PIPELINE ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Hata yönetimi en üstte olmalý
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseAuthentication();

// AuthenticatedUserMiddleware, UseAuthentication'dan SONRA gelmelidir
app.UseMiddleware<AuthenticatedUserMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();