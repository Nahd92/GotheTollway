using GotheTollway.Domain.Configuration;
using GotheTollway.Database.ServiceCollectionExtension;
using GotheTollway.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//Swagger configurations.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configuration settings.
builder.Services.Configure<InfrastructureSettings>(builder.Configuration.GetSection("DatabaseSettings"));

//Database configuration. 
builder.Services.AddDatabaseConfiguration(builder.Configuration);

//Dependency injections.
builder.Services.ConfigureServiceDependencyInjection();
builder.Services.ConfigureRepositoryDependencyInjection();
builder.Services.ConfigureInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
