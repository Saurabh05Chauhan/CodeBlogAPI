using CodeBlogAPI.Data;
using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodeBlogAPI.Repository.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await dbContext.Categories.AddAsync(category);
            await dbContext.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await dbContext.Categories.ToListAsync();

        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            //throw new NotImplementedException();
        }

        public async Task<Category?> EditCategoryByIdAsync(Category category)
        {
            var existingData= await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == (Guid)(category.Id));
            if (existingData != null)
            {
                dbContext.Categories.Entry(existingData).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;

            }

            return null;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var existingData = await dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(existingData != null)
            {
                dbContext.Categories.Remove(existingData);
                await dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
