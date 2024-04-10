using Microsoft.AspNetCore.Mvc;
using Petopia.Business.Interfaces;

namespace Petopia.API.Controllers
{
	[ApiController]
	[Route("api/Authentication")]
	public class BlogController : ControllerBase
	{
		private readonly IBlogService _blogService;

		public BlogController(
			IBlogService blogService
		)
		{
			_blogService = blogService;
		}
	}
}