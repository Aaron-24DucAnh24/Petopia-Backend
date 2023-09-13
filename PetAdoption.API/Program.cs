using PetAdoption.API.Middlewares;
using PetAdoption.Business.Extensions;
using PetAdoption.BackgroundJobs.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
	.AddControllersWithViews()
	.AddNewtonsoftJson(options =>
		options.SerializerSettings.ReferenceLoopHandling
		= Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddBusinessServices();
builder.Services.AddBackgroundServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
