using CodeBlogAPI.Data;
using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace CodeBlogAPI.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private ApplicationDbContext _context;
        public BlogPostRepository(ApplicationDbContext dbContext) {
            this._context = dbContext;
        }

        public async Task<BlogPost> AddBlogAsync(BlogPost blogPost)
        {
            await this._context.BlogPosts.AddAsync(blogPost);
            await this._context.SaveChangesAsync();
            return blogPost;
             
        }

        public async Task<IEnumerable<BlogPost>> GetBlogAsync()
        {
           return await this._context.BlogPosts.Include(x=>x.categories).ToListAsync();
        }

        public async Task<BlogPost?> GetBlogByIDAsync(Guid id)
        {
            return await this._context.BlogPosts.Where(x=> x.Id==id).Include(x => x.categories).FirstOrDefaultAsync();
        }

        public async Task<BlogPost?> UpdateBlog(BlogPost blog)
        {
            var existingData = await _context.BlogPosts.Include(x=>x.categories).FirstOrDefaultAsync(c => c.Id == (Guid)(blog.Id));
            if (existingData != null)
            {
                _context.BlogPosts.Entry(existingData).CurrentValues.SetValues(blog);
                existingData.categories=blog.categories;
                await _context.SaveChangesAsync();
                return blog;

            }

            return null;
        }

        public async Task<bool> DeleteBlog(Guid id)
        {
            var existingData = await _context.BlogPosts.Include(x => x.categories).FirstOrDefaultAsync(c => c.Id == id);
            if (existingData != null)
            {
                _context.BlogPosts.Remove(existingData);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
