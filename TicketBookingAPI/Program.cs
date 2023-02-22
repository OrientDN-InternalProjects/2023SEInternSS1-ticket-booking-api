using Microsoft.EntityFrameworkCore;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;
using AutoWrapper;
using Serilog;
using TicketBooking.Common.Middlewares;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddDbContext<TicketBookingDbContext>(op =>
        op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();
    app.UseMiddleware<ExceptionMiddlewareExtension>();

    app.MapControllers();
    app.UseApiResponseAndExceptionWrapper();

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Unhandle exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}