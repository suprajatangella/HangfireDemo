using Hangfire;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseHangfireDashboard("test/job-dashboard",
    new DashboardOptions
    {
        DashboardTitle = "Hang Fire Job Demo Application",
        DarkModeEnabled = false,
        DisplayStorageConnectionString = false
        Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter
            {
                User = "admin",
                Pass = "Admin@123"
            }
        }
    }
    );
app.Run();
