using CodeBlogAPI.Models.Domains;

namespace CodeBlogAPI.Repository.Interface
{
    public interface ICategoryRepository
    {
        public Task<Category> CreateCategoryAsync(Category category);
        public Task<IEnumerable<Category>> GetAllCategoryAsync(string? query=null,string? sortBy=null,string? sortDirection=null,int? pageNumber=1,int? pageSize=100);
        public Task<Category?> GetCategoryByIdAsync(Guid id);
        public Task<Category?> EditCategoryByIdAsync(Category category);
        public Task<bool> DeleteCategoryAsync(Guid id);
        public Task<int> GetCategoryCount();

    }
}
