﻿namespace CodeBlogAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
        public string Token { get; set; }
    }
}
