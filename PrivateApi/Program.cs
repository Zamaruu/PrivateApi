using Microsoft.Extensions.Options;
using PrivateApi.MongoDB;
using PrivateApi.Sections.WhiskyDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

#region Connection Settings

var mongoSettings = builder.Configuration.GetSection("MongoDBSettings");
builder.Services.Configure<MongoConnectionSettings>(mongoSettings);
builder.Services.AddSingleton<IMongoConnectionSettings>(sp => sp.GetRequiredService<IOptions<MongoConnectionSettings>>().Value);
builder.Services.AddSingleton<MongoWhiskyService>();

var whiskyDeUrl = builder.Configuration.GetSection("ScrapingUrls")["WhiskyDe"];
builder.Services.AddSingleton<WebScraper>(new WebScraper(whiskyDeUrl));

#endregion

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
