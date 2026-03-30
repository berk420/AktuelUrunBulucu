using AktuelUrunBulucu.BLL.Services;
using AktuelUrunBulucu.DAL.Context;
using AktuelUrunBulucu.DAL.Repositories;
using AktuelUrunBulucu.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:5173")
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

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapSearchEndpoints();
app.MapLocationEndpoints();
app.MapNotificationEndpoints();

app.Run();
