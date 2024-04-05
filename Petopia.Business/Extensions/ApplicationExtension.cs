using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Petopia.Business.Constants;

namespace Petopia.Business.Extensions
{
	public static class ApplicationExtension
	{
		public static void UseSockets(this WebApplication app, IConfiguration configuration)
		{
			string? originConfig = configuration.GetSection(AppSettingKey.CORS_ORIGIN).Get<string>();
			string[] origins = originConfig
				.Split(";", StringSplitOptions.RemoveEmptyEntries)
				.ToArray();
			WebSocketOptions webSocketOptions = new();
			if (!string.IsNullOrEmpty(originConfig))
			{
				foreach(var origin in origins)
				{
					webSocketOptions.AllowedOrigins.Add(origin);
				}
			}
			app.UseWebSockets(webSocketOptions);
		}
	}
}

