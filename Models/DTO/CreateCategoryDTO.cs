namespace CodeBlogAPI.Models.DTO
{
    public class CreateCategoryDTO
    {
        public string Name { get; set; }
        public string UrlHandle { get; set; }
    }

    public class EditCategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UrlHandle { get; set; }
    }
}
