using CodeBlogAPI.Data;
using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Models.DTO;
using CodeBlogAPI.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _category;

        public CategoriesController(ICategoryRepository category)
        {
            this._category = category;
        }

        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO request)
        {
            //create domain from dto
            Category category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle,
            };

           await _category.CreateCategoryAsync(category);

            var response = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle,
            };

            return Ok(response);
        }

        //https://localhost:xxxx/api/Categories?query=html&sortBy=name&sortDirection=desc
        [HttpGet]
        public async Task<IActionResult> GetAllCategoryAsync(
            [FromQuery] string? query,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortDirection,
            [FromQuery] int? pageNumber,
            [FromQuery] int? pageSize)
        {
            var listCategory=await _category.GetAllCategoryAsync(query,sortBy,sortDirection,pageNumber,pageSize);

            var response= new List<CategoryDTO>();
            foreach (var item in listCategory)
            {
                response.Add( new CategoryDTO

                {
                    Id = item.Id,
                    Name = item.Name,
                    UrlHandle = item.UrlHandle,
                });
            }

            return Ok(response);
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            var result=await _category.GetCategoryByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPut]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory(EditCategoryDTO dTO)
        {
            var category = new Category
            {
                Id = dTO.Id,
                Name = dTO.Name,
                UrlHandle = dTO.UrlHandle,
            };
            var result = await _category.EditCategoryByIdAsync(category);

            if (result==null)
            {
                return NotFound();
            }

            var response = new EditCategoryDTO
            {
                Id = result.Id,
                Name = result.Name,
                UrlHandle = result.UrlHandle,
            };
            
                return Ok(response);
           
        }

        [HttpDelete]
       [ Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
        {
            var result =await _category.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }

        //get api/categories/count
        [HttpGet]
        [Route("count")]
        public async Task<IActionResult> GetCategoriesTotal()
        {
            int count = await this._category.GetCategoryCount();

            return Ok(count);
        }

        [HttpOptions]
        public async Task<IActionResult> Options()
        {
            Response.Headers.Add("Allow", "GET, POST, OPTIONS");
            return Ok();
        }
    }
}
