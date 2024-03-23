using System.Text.Json;
using Petopia.Data;
using Petopia.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new();
string settingFileDir = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ?
	"appsettings.Development.json" : "appsettings.json";
var configuration = new ConfigurationBuilder()
	.SetBasePath($"{Directory.GetCurrentDirectory()}/../Petopia.API")
	.AddJsonFile(settingFileDir)
	.Build();
optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"));
ApplicationDbContext db = new(optionsBuilder.Options);

List<User>? admins = JsonSerializer.Deserialize<List<User>>(
	new StreamReader("Admins.json").ReadToEnd()
);
if (admins != null)
{
	foreach (var admin in admins)
	{
		admin.Id = Guid.NewGuid();
		UserIndividualAttributes? attributes = JsonSerializer.Deserialize<UserIndividualAttributes>(
			JsonSerializer.Serialize(admin.UserIndividualAttributes)
		);
		if (attributes == null)
		{
			break;
		}
		attributes.Id = admin.Id;
		admin.UserIndividualAttributes = null;
		db.Users.Add(admin);
		db.UserIndividualAttributes.Add(attributes);
	}
}

List<EmailTemplate>? emails = JsonSerializer.Deserialize<List<EmailTemplate>>(
	new StreamReader("Emails.json").ReadToEnd()
);
if (emails != null)
{
	foreach (var email in emails)
	{
		email.Id = Guid.NewGuid();
	}
	await db.EmailTemplates.AddRangeAsync(emails);
}

List<Province>? provinces = JsonSerializer.Deserialize<List<Province>>(
	new StreamReader("Provinces.json").ReadToEnd()
);
if (provinces != null)
{
	foreach (var a in provinces)
	{
		a.Id = Guid.NewGuid();
	}
	await db.Provinces.AddRangeAsync(provinces);
}

List<District>? districts = JsonSerializer.Deserialize<List<District>>(
	new StreamReader("Districts.json").ReadToEnd()
);
if (districts != null)
{
	foreach (var a in districts)
	{
		a.Id = Guid.NewGuid();
	}
	await db.Districts.AddRangeAsync(districts);
}

List<Ward>? wards = JsonSerializer.Deserialize<List<Ward>>(
	new StreamReader("Wards.json").ReadToEnd()
);
if (wards != null)
{
	foreach (var a in wards)
	{
		a.Id = Guid.NewGuid();
	}
	await db.Wards.AddRangeAsync(wards);
}

db.SaveChanges();
Console.WriteLine("Done");