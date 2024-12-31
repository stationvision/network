using Microsoft.EntityFrameworkCore;
using Monitoring.Db;
using Monitoring.Db.Interfaces;
using Monitoring.Db.Repositories;
using NetworkCommunications.Extentions;
using NetworkCommunications.Handlers;
using NetworkCommunications.Options;
using NetworkCommunications.TcpManager;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration(ConfigureAppSettings);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = JsonSerializerConfig.DefaultOptions.WriteIndented;
    // Add more default options here as needed
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ConfigOption>(builder.Configuration.GetSection("NetworkCommunication"));

var option = builder.Configuration.GetSection("NetworkCommunication").Get<ConfigOption>();
var connectionString = option.IdentityConnectionString;

builder.Services.AddDbContext<IdentityMonitoringDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(IdentityRepository<>));

builder.Services.AddScoped<MessageHandler>();
builder.Services.AddSingleton<MessageProcessingService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageProcessingService>());

builder.Services.AddHostedService<TcpServerPackingService>();


#region NServiceBus
//// Register NServiceBus endpoint configuration
//builder.Host.UseNServiceBus((context) =>
//{
//    var endpointConfiguration = new EndpointConfiguration("TcpNNServiceBusEndpoint");
//    endpointConfiguration.LicensePath("license.xml");
//    var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
//    transport.ConnectionString(connectionString);
//    transport.DefaultSchema("dbo");

//    endpointConfiguration.EnableInstallers();

//    endpointConfiguration.UsePersistence<LearningPersistence>();
//    endpointConfiguration.UseSerialization<SystemJsonSerializer>();



//    return endpointConfiguration;
//}).ConfigureServices(service =>
//{
//    var option = builder.Configuration.GetSection("NetworkCommunication").Get<ConfigOption>();
//    var connectionString = option.IdentityConnectionString;
//    service.AddDbContext<IdentityMonitoringDbContext>(options => options.UseSqlServer(connectionString));
//    service.AddScoped(typeof(IRepository<>), typeof(IdentityRepository<>));

//});
//builder.Services.AddSingleton<IEndpointInstance>(sp =>
//{
//    var endpointConfiguration = new EndpointConfiguration("TcpServiceBusEndpoint");

//    var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
//    transport.ConnectionString(connectionString);
//    transport.DefaultSchema("dbo");

//    // Enable installers
//    endpointConfiguration.EnableInstallers();

//    // Configure persistence and serialization
//    endpointConfiguration.UsePersistence<LearningPersistence>();
//    endpointConfiguration.UseSerialization<SystemJsonSerializer>();

//    endpointConfiguration.RegisterComponents(components =>
//    {
//        // Register IRepository<> as scoped (InstancePerUnitOfWork) and provide the DbContext it depends on
//        components.ConfigureComponent(context => new IdentityRepository<MessageEntity>(context.Build<IdentityMonitoringDbContext>()), DependencyLifecycle.InstancePerUnitOfWork);
//        components.ConfigureComponent(context => new IdentityRepository<Puls>(context.Build<IdentityMonitoringDbContext>()), DependencyLifecycle.InstancePerUnitOfWork); ;
//        components.ConfigureComponent(context => new IdentityRepository<ClientPuls>(context.Build<IdentityMonitoringDbContext>()), DependencyLifecycle.InstancePerUnitOfWork); ;

//        //components.ConfigureComponent(context => new IdentityRepository<ClientData>(context.Build<IdentityMonitoringDbContext>()), DependencyLifecycle.InstancePerUnitOfWork);

//        // Ensure that the DbContext is also registered if not already configured outside
//        components.ConfigureComponent<IdentityMonitoringDbContext>(DependencyLifecycle.InstancePerUnitOfWork);



//        components.ConfigureComponent<IdentityMonitoringDbContext>(componentContext =>
//        {
//            // Create a new DbContextOptionsBuilder
//            var optionsBuilder = new DbContextOptionsBuilder<IdentityMonitoringDbContext>();

//            // Retrieve the configuration or service provider from componentContext if needed
//            var configuration = componentContext.Build<IConfiguration>();


//            // Set up the DbContext with SQL Server
//            optionsBuilder.UseSqlServer(connectionString);

//            // Return a new instance of the DbContext
//            return new IdentityMonitoringDbContext(optionsBuilder.Options);
//        }, DependencyLifecycle.InstancePerUnitOfWork);

//        components.ConfigureComponent<MessageHandler>(DependencyLifecycle.InstancePerUnitOfWork);
//        components.ConfigureComponent<TcpServerPackingService>(DependencyLifecycle.InstancePerUnitOfWork);

//        //builder.Services.AddScoped<NetworkCommunications.Handlers.MessageHandler>();

//        components.ConfigureComponent<MessageProcessingService>(DependencyLifecycle.SingleInstance);

//        var configuration = builder.Configuration;
//        components.ConfigureComponent(context =>
//        {
//            var options = configuration.GetSection("NetworkCommunication").Get<ConfigOption>();
//            return Microsoft.Extensions.Options.Options.Create(options);
//        }, DependencyLifecycle.SingleInstance);


//    });



//    var endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
//    return endpointInstance;

//}).BuildServiceProvider();
#endregion



var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseHttpsRedirection();
//}
app.UseAuthorization();
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
