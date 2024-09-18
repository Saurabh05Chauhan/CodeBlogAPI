using CodeBlogAPI.Models.Domains;

namespace CodeBlogAPI.Repository.Interface
{
    public interface IBlogPostRepository
    {
        public Task<BlogPost> AddBlogAsync(BlogPost blogPost);
        public Task<IEnumerable<BlogPost>> GetBlogAsync();
        public Task<BlogPost?> GetBlogByIDAsync(Guid id);
        public Task<BlogPost?> GetBlogByURLAsync(string url);
        public Task<BlogPost?> UpdateBlog(BlogPost blogPost);
        public Task<bool> DeleteBlog(Guid id);
    }
}
