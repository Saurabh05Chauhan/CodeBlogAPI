using Microsoft.AspNetCore.Identity;

namespace CodeBlogAPI.Repository.Interface
{
    public interface ITokenRepository
    {
        string CreateToken(IdentityUser user, List<string> roles);
    }
}
