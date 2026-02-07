using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Text;
using YMM.Api.Middleware;
using YMM.Application;
using YMM.Application.Abstract.Services;
using YMM.Application.Dto.Auth;
using YMM.Application.Immplementation;
using YMM.Application.Seeder;
using YMM.Infrastructure;
using YMM.Infrastructure.Caching;
using YMM.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// PERFORMANCE: Database Configuration
// ============================================
builder.Services.AddDbContext<AppDb>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            // Performance: Enable command timeout and retry logic
            //sqlOptions.CommandTimeout(30);
            //sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
            //// Performance: Use split queries for better performance with large includes
            //sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
    
    // Performance: Disable sensitive data logging in production
    if (!builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging(false);
    }
});

// ============================================
// PERFORMANCE: Caching Configuration
// ============================================
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 1024; // Limit cache size
    options.CompactionPercentage = 0.25; // Remove 25% when limit reached
});

// ============================================
// PERFORMANCE: Response Compression
// ============================================
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// ============================================
// PERFORMANCE: Output Caching
// ============================================
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Cache());
    
    // Cache product lists for 5 minutes
    options.AddPolicy("ProductList", builder => 
        builder.Expire(TimeSpan.FromMinutes(5))
               .Tag("products"));
    
    // Cache category data for 10 minutes
    options.AddPolicy("CategoryList", builder => 
        builder.Expire(TimeSpan.FromMinutes(10))
               .Tag("categories"));
});

// ============================================
// CORS Configuration for React Frontend
// ============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactAppPolicy", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",  // Next.js default dev port
            "http://localhost:3001",
            "https://localhost:3000",
            "https://localhost:3001"
        )
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithExposedHeaders("Content-Disposition"); // For file downloads
    });
});

builder.Services.AddControllers();
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "YMM API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter:  {your JWT token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthorization();

// ============================================
// PERFORMANCE: Register Services
// ============================================
builder.Services
    .AddServiceRegistration(builder.Configuration)
    .ApplicationRegist();

// Performance: Register caching service

// Performance: Register metrics
builder.Services.AddSingleton<YMM.Api.Performance.PerformanceMetrics>();

var app = builder.Build();

// ============================================
// PERFORMANCE: Middleware Pipeline
// ============================================
// Response compression should be first
app.UseResponseCompression();

// Output cache
app.UseOutputCache();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AdminSeeder.SeedAsync(services);
    await RoleSeeder.AddRoles(services);
}
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("ReactAppPolicy");

// Performance monitoring middleware
app.UseMiddleware<YMM.Api.Performance.PerformanceMiddleware>();
app.UseMiddleware<RequestTrackMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();