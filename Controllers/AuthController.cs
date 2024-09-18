using CodeBlogAPI.Models.DTO;
using CodeBlogAPI.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CodeBlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository token;

        public AuthController(UserManager<IdentityUser> user,ITokenRepository token)
        {
            this.userManager = user;
            this.token = token;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO login)
        {
            var identityUser= await userManager.FindByEmailAsync(login.email);

            if (identityUser is not null) 
            { 
                var checkPassword = await userManager.CheckPasswordAsync(identityUser, login.password);

                if (checkPassword) 
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    var token = this.token.CreateToken(identityUser, roles.ToList());
                    var req = new LoginResponseDTO
                    {
                        Email = login.email,
                        Roles= roles.ToList(),
                        Token= token

                    };

                    return Ok(req);
                }

            }
            ModelState.AddModelError("", "Email or Password Incorrect");
            return ValidationProblem(ModelState);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO register)
        {
            var user = new IdentityUser
            {
                UserName = register.email.Split('@')[0].Trim(),
                Email = register.email.Trim()
            };

            var identityUser = await userManager.CreateAsync(user, register.password);
            if (identityUser.Succeeded)
            {
                identityUser = await userManager.AddToRoleAsync(user, "Reader");

                if (identityUser.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityUser.Errors.Any())
                    {
                        foreach (var error in identityUser.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityUser.Errors.Any())
                {
                    foreach (var error in identityUser.Errors) 
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}
