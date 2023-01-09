using Coodesh.Challenge.API.IoC;
using Coodesh.Challenge.Domain.Services.IoC;
using Coodesh.Challenge.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDomainServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddJobs();

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