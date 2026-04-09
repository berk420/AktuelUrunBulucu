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

// Auto migration + seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseRateLimiter();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapSearchEndpoints();
app.MapLocationEndpoints();
app.MapNotificationEndpoints();

app.MapFallbackToFile("index.html");

app.Run();
