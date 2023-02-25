using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;

namespace YoutubeBlog.Entity.Models.DTOs.Articles
{
    public class ArticleUpdateDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }

        public Image Image{ get; set; }
        public IFormFile? Photo { get; set; }
        public IList<CategoryDto>? Categories { get; set; }
    }
}
