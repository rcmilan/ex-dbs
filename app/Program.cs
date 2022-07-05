using infra.cache;
using infra.document;
using infra.embed;
using infra.graph;
using Microsoft.EntityFrameworkCore;
using model.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", false, true);
IConfiguration configuration = builder.Configuration;

// Add services to the container.

// Redis
var multiplexer = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { configuration["ConnectionStrings:Redis:Endpoint"] } });
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);

// MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp => new MongoClient(configuration["ConnectionStrings:MongoDB:Endpoint"]));

// SQLite
builder.Services.AddDbContext<EmbedContext>(options => options.UseSqlite(configuration["ConnectionStrings:SQLite:Endpoint"]));

// Neo4j
builder.Services.AddGraphModule(new ModuleDependency.Connection(configuration["ConnectionStrings:Neo4j:Endpoint"], "neo4j", "test"));

// DI
builder.Services.AddScoped<ICacheRepository<User, Guid>, UserCacheService>();
builder.Services.AddScoped<IDocumentRepository<Document, ObjectId>, DocumentService>();
builder.Services.AddScoped<IEmbedRepository<Subscriber, Guid>, EmbedService<Subscriber, Guid>>();

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