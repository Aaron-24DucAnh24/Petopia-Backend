﻿using Petopia.Business.Models.Blog;
using Petopia.Business.Models.Common;

namespace Petopia.Business.Interfaces
{
	public interface IBlogService
	{
		public Task<PaginationResponseModel<BlogResponseModel>> GetBlogsAsync(PaginationRequestModel request);
		public Task<BlogDetailResponseModel> GetBlogByIdAsync(Guid id);
		public Task<Guid> CreateBlogAsync(CreateBlogRequestModel request);
		public Task<BlogDetailResponseModel> UpdateBlogAsync(UpdateBlogRequestModel request);
	}
}
