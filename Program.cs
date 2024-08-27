using FinalProject_APIServer.DAL;
using FinalProject_APIServer.Middelware;
using FinalProject_APIServer.Middelware.Global;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FinalProjectDbContext>(options => options.UseSqlServer(connectionString));

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
app.UseWhen(
    context =>
    !context.Request.Path.StartsWithSegments("/Login"),
    appbuilder =>
    {
        appbuilder.UseMiddleware<JWTvalidation>();
    }


    );


app.UseAuthorization();

app.MapControllers();

app.Run();
