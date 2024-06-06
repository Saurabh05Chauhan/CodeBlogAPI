using CodeBlogAPI.Models.Domains;

namespace CodeBlogAPI.Repository.Interface
{
    public interface IImageRepository
    {
        Task<BlogImage> Upload(IFormFile file, BlogImage blogImage);
    }
}
