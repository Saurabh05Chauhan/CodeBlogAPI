using CodeBlogAPI.Models.Domains;
using CodeBlogAPI.Models.DTO;
using CodeBlogAPI.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase

    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost]

        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
             ValidateFileUpload(file);
            if (ModelState.IsValid) 
            {
                var blogImage = new BlogImage
                {
                    FileExtension = Path.GetExtension(fileName).ToLower(),
                    Title = title,
                    DateCreated = DateTime.Now,
                    FileName = fileName,

                };

                var res =await imageRepository.Upload(file,blogImage);

                //Convert domain to dto

                var result = new BlogImageDTO
                {
                    Id= res.Id,
                    Url = res.Url,
                    Title = res.Title,
                    DateCreated = res.DateCreated,
                    FileName = res.FileName,
                    FileExtension= res.FileExtension,

                };

                return Ok(result);
            }
            return BadRequest();
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName.ToLower())) )
            {
                ModelState.AddModelError("file", "Unsupported");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10mb");
            }
        }
    }
}
