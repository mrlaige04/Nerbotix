using RoboTasker.Api;
using RoboTasker.Api.Extensions;
using RoboTasker.Application;
using RoboTasker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUi();

var app = builder.Build();

await app.MigrateDatabase();
await app.EnsureSuperAdminCreated();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseExceptionHandler(options => {});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();