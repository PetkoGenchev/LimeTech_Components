using LimeTech_Components.Server.Data;
using LimeTech_Components.Server.Data.Models;
using LimeTech_Components.Server.Infrastructure;
using LimeTech_Components.Server.Repositories.Components;
using LimeTech_Components.Server.Repositories.Customers;
using LimeTech_Components.Server.Services.Components;
using LimeTech_Components.Server.Services.Customers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LimeTech_Components.Server;


var builder = WebApplication.CreateBuilder(args);

// Read JWT settings securely
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") ?? throw new InvalidOperationException("JWT Key is missing!");
var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") ?? throw new InvalidOperationException("JWT Issuer is missing!");
var jwtAudience = builder.Configuration.GetValue<string>("Jwt:Audience") ?? throw new InvalidOperationException("JWT Audience is missing!");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Configuration
builder.Services.AddDbContext<LimeTechDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity Configuration
builder.Services.AddIdentity<Customer, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<LimeTechDbContext>()
    .AddDefaultTokenProviders();

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "127.0.0.1")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(LimeTechDbContext));

// Dependency Injection for Services & Repositories
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IComponentRepository, ComponentRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero, // Prevents delayed expiry
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Custom Middleware for Role & User Initialization
builder.Services.AddScoped<IRoleAndAdminInitializer, RoleAndAdminInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Initialize roles & admin user in a cleaner way
    var roleAndAdminInitializer = services.GetRequiredService<IRoleAndAdminInitializer>();
    await roleAndAdminInitializer.InitializeRolesAndAdminAsync();

    await DatabaseSeeder.SeedAsync(services);
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
