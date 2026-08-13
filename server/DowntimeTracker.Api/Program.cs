using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DowntimeTracker.Api.Data;
using DowntimeTracker.Api.Services;
using DowntimeTracker.Api.Endpoints;
using Microsoft.AspNetCore.SignalR;
using DowntimeTracker.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// Services
// ============================================

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.ParameterLocation.Header,
            Description = "Enter your JWT token."
        });

    options.AddSecurityRequirement(document =>
        new Microsoft.OpenApi.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.OpenApiSecuritySchemeReference(
                    "Bearer",
                    document),
                new List<string>()
            }
        });
});

// ============================================
// Database
// ============================================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ============================================
// Application Services
// ============================================

builder.Services.AddScoped<AuthService>();
builder.Services.AddSingleton<TokenService>();

// ============================================
// Authentication - JWT
// ============================================

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]!))
        };
    });

// ============================================
// Authorization
// ============================================

builder.Services.AddAuthorization();

builder.Services.AddSignalR();

// ============================================
// Build Application
// ============================================

var app = builder.Build();

// ============================================
// Database Migration & Seeding
// ============================================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.Migrate();

    DbSeeder.Seed(db);
}

// ============================================
// Middleware
// ============================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ============================================
// Endpoints
// ============================================

app.MapAuthEndpoints();
app.MapLookupEndpoints();
app.MapPlannedDowntimeEndpoints();
app.MapUnplannedDowntimeEndpoints();

app.MapHub<DowntimeHub>("/hubs/downtime");

// ============================================
// Run Application
// ============================================

app.Run();