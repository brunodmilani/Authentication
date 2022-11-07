using Authentication.API.Extensions;
using Authentication.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(builder.Configuration);
builder.Services.AddMvc();
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var serviceDb = serviceScope.ServiceProvider.GetService<AppDbContext>();
    serviceDb.Database.Migrate();
}

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
