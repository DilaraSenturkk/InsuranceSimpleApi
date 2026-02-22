using InsuranceSimpleApi.Application; // Extension method için
using InsuranceSimpleApi.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. KATMAN KAYITLARI (EXTENSION METHODS) ---

// API sadece Application'ý bilir. 
// Infrastructure kayýtlarý da Application üzerinden veya zincirleme þekilde eklenir.
builder.Services.AddApplication(builder.Configuration);

// --- 2. API SEVÝYESÝ SERVÝSLER ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// --- 3. ERROR HANDLING & MIDDLEWARE ---
builder.Services.AddScoped<ErrorHandlerMiddleware>();
builder.Services.AddScoped<AuthenticatedUserMiddleware>();

// --- 4. AUTHENTICATION - JWT ---
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

// --- 5. SWAGGER AYARLARI ---
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
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// --- 6. HTTP REQUEST PIPELINE (MIDDLEWARES) ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>(); // En dýþta hatalarý yakalar

app.UseAuthentication();
app.UseMiddleware<AuthenticatedUserMiddleware>(); // Auth'dan hemen sonra
app.UseAuthorization();

app.MapControllers();

app.Run();