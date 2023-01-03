using Coodesh.Challenge.API.Jobs;
using Coodesh.Challenge.Domain.Services.IoC;
using Coodesh.Challenge.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddHostedService<ImporterProductsJob>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();