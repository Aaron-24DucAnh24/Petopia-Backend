using Petopia.Business.Models.Post;

namespace Petopia.Business.Interfaces
{
	public interface IPostService
	{
		public Task<PostResponseModel> CreatePostAsync(CreatePostRequestModel request);
		public Task<List<PostResponseModel>> GetPostsByPetIdAsync(Guid petId);
		public Task<bool> DeletePostAsync(Guid postId);
		public Task<int> LikePostAsync(Guid postId);
	}
}

