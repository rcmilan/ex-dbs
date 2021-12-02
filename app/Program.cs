using infra.cache;
using model.Entities;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true);

IConfiguration configuration = builder.Configuration;

// Add services to the container.
var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { configuration["ConnectionStrings:Redis:Endpoint"] } });
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

builder.Services.AddScoped<ICacheService<User, Guid>, UserCacheService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
