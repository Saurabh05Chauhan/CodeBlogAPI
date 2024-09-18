using CodeBlogAPI.Data;
using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace CodeBlogAPI.Repository.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ApplicationDbContext applicationDbContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,IHttpContextAccessor httpContextAccessor,ApplicationDbContext applicationDbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<BlogImage>> GetAllImages()
        {
            return await applicationDbContext.BlogImages.ToListAsync();
            
        }

        public async Task<BlogImage> Upload(IFormFile file, BlogImage blogImage)
        {
            //upload the image to API/Images
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images",$"{blogImage.FileName}{blogImage.FileExtension}");

            using var stream = new FileStream(localPath,FileMode.Create);
            await file.CopyToAsync(stream);

            //update database
            var http = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{http.Scheme}://{http.Host}{http.PathBase}/Images/{blogImage.FileName}{blogImage.FileExtension}";

            blogImage.Url = urlPath;

            await applicationDbContext.BlogImages.AddAsync(blogImage);
            await applicationDbContext.SaveChangesAsync();
            return blogImage;

            
        }
    }
}
