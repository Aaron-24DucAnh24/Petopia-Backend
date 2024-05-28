using Petopia.Business.Extensions;
using Petopia.BackgroundJobs.Extensions;
using Petopia.API.Middlewares;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
  .AddControllersWithViews()
  .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling
    = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddBackgroundServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseMiddleware<ExceptionHandlerMiddleware>();

// app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
