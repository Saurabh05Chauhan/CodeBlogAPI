using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Models.DTO;
using CodeBlogAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostRepository repository;
        private readonly ICategoryRepository categoryRepository;

        public BlogPostController(IBlogPostRepository repository,ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
        }
        [HttpPost]
        [Authorize(Roles="Writer")]
        public async Task<IActionResult> AddBlogPost(CreateBlogPostDTO createBlogPost)
        {
            var req = new BlogPost
            {
                Title = createBlogPost.Title,
                ShortDescription = createBlogPost.ShortDescription,
                Content = createBlogPost.Content,
                FeaturedImageURL = createBlogPost.FeaturedImageURL,
                UrlHandle = createBlogPost.UrlHandle,
                Author = createBlogPost.Author,
                PublishedDate = createBlogPost.PublishedDate,
                IsVisible = createBlogPost.IsVisible,
                categories = new List<Category>()
            };

            foreach (var item in createBlogPost.Categories)
            {
                var exsits = await this.categoryRepository.GetCategoryByIdAsync(item);
                if (exsits != null)
                {
                    req.categories.Add(exsits);
                }
            }

            var result=await this.repository.AddBlogAsync(req);
            var res = new BlogPostDTO
            {
                Id = result.Id,
                Title = result.Title,
                ShortDescription = result.ShortDescription,
                Content = result.Content,
                FeaturedImageURL = result.FeaturedImageURL,
                UrlHandle = result.UrlHandle,
                Author = result.Author,
                PublishedDate = result.PublishedDate,
                IsVisible = result.IsVisible,
                Categories = result.categories.Select(x => new CategoryDTO
                {
                    Id = x.Id,
                    Name=x.Name,
                    UrlHandle=x.UrlHandle
                }).ToList()

            };
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> getBlogPost()
        {
            var result = await this.repository.GetBlogAsync();
            List<BlogPostDTO> posts = new List<BlogPostDTO>();
            if(result != null)
            {
                foreach (var blogPost in result)
                {
                    var res = new BlogPostDTO
                    {
                        Id = blogPost.Id,
                        Title = blogPost.Title,
                        ShortDescription = blogPost.ShortDescription,
                        Content = blogPost.Content,
                        FeaturedImageURL = blogPost.FeaturedImageURL,
                        UrlHandle = blogPost.UrlHandle,
                        Author = blogPost.Author,
                        PublishedDate = blogPost.PublishedDate,
                        IsVisible = blogPost.IsVisible,
                        Categories= blogPost.categories.Select(x => new CategoryDTO
                        {
                            Id=x.Id, Name=x.Name, UrlHandle=x.UrlHandle
                        }).ToList()
                        
                    };

                    posts.Add(res);
                }

                return Ok(posts);
            }
            

            return NotFound();
            
        }
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetBlogByID(Guid id)
        {
            var blogPost= await this.repository.GetBlogByIDAsync(id);
            if(blogPost != null)
            {
                var res = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    FeaturedImageURL = blogPost.FeaturedImageURL,
                    UrlHandle = blogPost.UrlHandle,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                };

                return Ok(res);
            }
            return NotFound();
        }
        
        [HttpGet]
        [Route("{url}")]
        public async Task<IActionResult> GetBlogByURL([FromRoute]string url)
        {
            var blogPost= await this.repository.GetBlogByURLAsync(url);
            if(blogPost != null)
            {
                var res = new BlogPostDTO
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    ShortDescription = blogPost.ShortDescription,
                    Content = blogPost.Content,
                    FeaturedImageURL = blogPost.FeaturedImageURL,
                    UrlHandle = blogPost.UrlHandle,
                    Author = blogPost.Author,
                    PublishedDate = blogPost.PublishedDate,
                    IsVisible = blogPost.IsVisible,
                    Categories = blogPost.categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                };

                return Ok(res);
            }
            return NotFound();
        }

        [HttpPut]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditBlogPost(editBlogPostDto blogPostDTO)
        {
            //convertBlogPost DTO to domain
            var blog = new BlogPost
            {
                Id = blogPostDTO.Id,
                Title = blogPostDTO.Title,
                ShortDescription = blogPostDTO.ShortDescription,
                Content = blogPostDTO.Content,
                FeaturedImageURL = blogPostDTO.FeaturedImageURL,
                UrlHandle = blogPostDTO.UrlHandle,
                Author = blogPostDTO.Author,
                PublishedDate = blogPostDTO.PublishedDate,
                IsVisible = blogPostDTO.IsVisible,
                categories = new List<Category>()


            };

            foreach (var category in blogPostDTO.Categories)
            {
                var exsits = await this.categoryRepository.GetCategoryByIdAsync(category);
                if (exsits != null)
                {
                    blog.categories.Add(exsits);
                }
            }

            var result= await this.repository.UpdateBlog(blog);
            if (result != null)
            {
                var res = new BlogPostDTO
                {
                    Id = result.Id,
                    Title = result.Title,
                    ShortDescription = result.ShortDescription,
                    Content = result.Content,
                    FeaturedImageURL = result.FeaturedImageURL,
                    UrlHandle = result.UrlHandle,
                    Author = result.Author,
                    PublishedDate = result.PublishedDate,
                    IsVisible = result.IsVisible,
                    Categories = result.categories.Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle
                    }).ToList()

                };

                return Ok(res);
            }

            return BadRequest();

        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteBlog([FromRoute] Guid id)
        {
            var result = await repository.DeleteBlog(id);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
