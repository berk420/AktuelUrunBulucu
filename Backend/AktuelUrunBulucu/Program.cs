using AktuelUrunBulucu.BLL.Services;
using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Repositories;
using AktuelUrunBulucu.Endpoints;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseWindowsService();

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISearchLogRepository, SearchLogRepository>();
builder.Services.AddScoped<IUserCoordinateRepository, UserCoordinateRepository>();
builder.Services.AddScoped<INotificationRequestRepository, NotificationRequestRepository>();

// Services
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rate Limiting
const int SearchRateLimitPerMinute = 10;
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("search", limiter =>
    {
        limiter.PermitLimit = SearchRateLimitPerMinute;
        limiter.Window = TimeSpan.FromMinutes(1);
        limiter.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiter.QueueLimit = 0;
    });
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();
startupLogger.LogInformation("Uygulama başlatılıyor. Environment: {Env}", app.Environment.EnvironmentName);

// Auto migration + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        startupLogger.LogInformation("Veritabanı migration başlatılıyor...");
        db.Database.Migrate();
        startupLogger.LogInformation("Veritabanı migration tamamlandı.");
    }
    catch (Exception ex)
    {
        startupLogger.LogCritical(ex, "Veritabanı migration sırasında kritik hata oluştu.");
        throw;
    }
}

// CORS — en dışta olmalı ki hata yanıtlarına da header eklenir
app.UseCors();

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        if (ctx.File.Name.Equals("index.html", StringComparison.OrdinalIgnoreCase))
        {
            ctx.Context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            ctx.Context.Response.Headers["Pragma"] = "no-cache";
        }
    }
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseRateLimiter();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapSearchEndpoints();
app.MapLocationEndpoints();
app.MapNotificationEndpoints();
app.MapRecommendationEndpoints();
app.MapProductsEndpoints();

app.Run();
