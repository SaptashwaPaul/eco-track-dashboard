using EcoTrack.Api.Hubs;
using EcoTrack.Api.Workers;
using EcoTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add these two lines to register Swagger:
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add SignalR Services
builder.Services.AddSignalR();

// Register the Background Worker
builder.Services.AddHostedService<DisruptionSimulationWorker>();

// Configure EF Core (from our previous steps)
builder.Services.AddDbContext<EcoTrackDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure CORS so our Angular UI can connect to the API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularUI", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Crucial for SignalR WebSockets
    });
});

var app = builder.Build();

// Add these four lines to enable the Swagger UI:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularUI");
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

// Map the SignalR Hub endpoint
app.MapHub<DisruptionHub>("/hubs/disruptions");

// Seed Database 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // 1. Grab the database context
    var context = services.GetRequiredService<EcoTrack.Infrastructure.Persistence.EcoTrackDbContext>();

    // 2. Automatically apply all pending EF Core migrations (creates the tables!)
    await Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions.MigrateAsync(context.Database);

    // 3. Now that tables exist, seed the data
    await EcoTrackDbSeeder.SeedAsync(services);
}

app.Run();
