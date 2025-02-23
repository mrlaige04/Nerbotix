using RoboTasker.Api;
using RoboTasker.Api.Extensions;
using RoboTasker.Application;
using RoboTasker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddUi();

var app = builder.Build();

await app.MigrateDatabase();
await app.EnsureSeedData();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseExceptionHandler(options => {});

app.MapControllers();

app.Run();
