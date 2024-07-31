using Microsoft.EntityFrameworkCore;
using Hospital.Context;


var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavivor", true);
builder.Services.AddDbContext<HospitalSchema>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionStr")));

SchemaFactory.ConnectionString=builder.Configuration.GetConnectionString("ConnectionStr");
SchemaFactory.ApplyMigrations();     

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title="hospital api" , Version = "v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c=> {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "hospital api v1");
        c.RoutePrefix = "";
    });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
