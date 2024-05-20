﻿namespace CodeBlogAPI.Models.Domains
{
    public class BlogPost
    {
        public Guid Id{ get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public string FeaturedImageURL { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }
    }
}
