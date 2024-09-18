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

        public async Task<IEnumerable<Category>> GetAllCategoryAsync(string? query=null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1, int? pageSize = 100)
        {
            //query the database
            var categories=dbContext.Categories.AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(query) == false) {
               categories = categories.Where(x => x.Name.Contains(query));

            }


            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false) 
            {
                if (string.Equals(sortBy,"Name",StringComparison.OrdinalIgnoreCase)) 
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;

                    categories=isAsc? categories.OrderBy(x => x.Name):categories.OrderByDescending(x=>x.Name);
                }

                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase) ? true : false;

                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
                }
            }


            //pagination
            var skipResult = (pageNumber - 1) * pageSize;
            categories= categories.Skip(skipResult??0).Take(pageSize??100);


            return await categories.ToListAsync();

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

        public async Task<int> GetCategoryCount()
        {
            return await dbContext.Categories.CountAsync();
        }
    }
}
