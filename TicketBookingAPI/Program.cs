using AutoMapper;
using AutoWrapper;
using ErrorManagement.Configurations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TicketBooking.Data.DataModel;
using TicketBooking.Data.DbContext;
using TicketBooking.Data.Infrastructure;
using TicketBooking.Data.Repository;
using TicketBooking.Service.AuthenticateService;

using TicketBooking.Service;
using TicketBooking.Service.AircraftService;
using TicketBookingAPI.Controller.AircraftController;
using ILogger = Microsoft.Extensions.Logging.ILogger;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<TicketBookingDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddDbContext<TicketBookingDbContext>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
builder.Services.AddScoped<IAircraftSerivce, AircraftService>();
    
var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new AutoMapperProfile());
    }
);
IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<HandleExceptionMiddleware>();

app.MapControllers();
app.UseApiResponseAndExceptionWrapper();

app.Run();