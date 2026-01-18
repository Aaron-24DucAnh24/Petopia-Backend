using Petopia.Business.Extensions;
using Petopia.BackgroundJobs.Extensions;
using Petopia.API.Middlewares;
using Serilog;
using Serilog.Events;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
  .AddControllersWithViews()
  .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddBackgroundServices(builder.Configuration);

builder.Logging.ClearProviders();
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Information()
  .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
  .Enrich.FromLogContext()
  .WriteTo.File(
    path: Petopia.Business.Constants.LOG_PATH_INFO,
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Information,
    outputTemplate: Petopia.Business.Constants.LOG_OUTPUT_TEMPLATE
  )
  .WriteTo.File(
    path: Petopia.Business.Constants.LOG_PATH_WARNING,
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Warning,
    outputTemplate: Petopia.Business.Constants.LOG_OUTPUT_TEMPLATE
  )
  .WriteTo.File(
    path: Petopia.Business.Constants.LOG_PATH_ERROR,
    rollingInterval: RollingInterval.Day,
    restrictedToMinimumLevel: LogEventLevel.Error,
    outputTemplate: Petopia.Business.Constants.LOG_OUTPUT_TEMPLATE
  )
  .CreateLogger();
builder.Host.UseSerilog();

WebApplication app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
