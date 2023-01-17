using MagicVilla_VillaApi;
using MagicVilla_VillaApi.Data;
using MagicVilla_VillaApi.Logging;
using MagicVilla_VillaApi.Repository;
using MagicVilla_VillaApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Serilog Configurations 
// Commented as we are using custom logging
//Log.Logger= new LoggerConfiguration().MinimumLevel.Debug().WriteTo
//            .File("logs/villaLogs.txt",rollingInterval:RollingInterval.Day).CreateLogger();
//builder.Host.UseSerilog();
/////////


builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable = false;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddAutoMapper(typeof(MappingConfig));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
//builder.Services.AddSingleton<ILogging, Logging>();

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
