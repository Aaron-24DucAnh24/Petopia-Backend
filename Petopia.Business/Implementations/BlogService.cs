using Microsoft.Extensions.Logging;
using Petopia.Business.Interfaces;

namespace Petopia.Business.Implementations
{
	public class BlogService : BaseService, IBlogService
	{
		public BlogService(
			IServiceProvider provider,
			ILogger<BlogService> logger
		) : base(provider, logger)
		{
		}
	}
}

