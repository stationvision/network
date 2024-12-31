using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Monitoring.Api.Extentions;
using Monitoring.Api.Filters;
using Monitoring.Api.Options;
using Monitoring.Db;
using Monitoring.Db.IdentityModels;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddProblemDetails(ConfigureProblemDetails).AddControllers().AddFluentValidation().AddProblemDetailsConventions().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverterWithAttributeSupport()));
builder.Host.ConfigureAppConfiguration(ConfigureAppSettings);

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddDbContext<IdentityMonitoringDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<ConfigOption>(builder.Configuration.GetSection("MonitoringApi"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(IdentityRepository<>));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<IdentityMonitoringDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Redirect to login page if not authenticated
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddMvc(
    option =>
    {
        option.Filters.Add(typeof(ExceptionFilters));
    }
);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .WithOrigins("http://localhost:3000") // Specify your frontend URL
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonConfig());
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use exact property names
    options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty printing
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull; // Optional: ignore null values
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Monitoring API",
        Version = "v1",
        Description = "An API for monitoring system",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Name",
            Email = "your-email@example.com"
        }
    });

    // Add security definition for Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddSingleton(typeof(ExceptionFilters));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Monitoring API V1");

});

app.MapRazorPages();
app.MapControllers();

app.Run();

static void ConfigureAppSettings(IConfigurationBuilder configurationBuilder)
{
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    configurationBuilder.SetBasePath(AppContext.BaseDirectory);
    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
    configurationBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true);

    if (environment == Environments.Development)
    {
        configurationBuilder.AddUserSecrets<Program>();
    }
    configurationBuilder.AddEnvironmentVariables();
}
//void ConfigureProblemDetails(Hellang.Middleware.ProblemDetails.ProblemDetailsOptions options)
//{
//    options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
//    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
//    options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
//    options.MapToStatusCode<DbUpdateConcurrencyException>(StatusCodes.Status404NotFound);
//    options.OnBeforeWriteDetails = (httpContext, details) =>
//    {
//        details.Type = null;
//        details.Extensions.Remove("traceId");
//    };


//}