using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;

namespace YoutubeBlog.Entity.Models.DTOs.Articles
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public CategoryDto Category { get; set; }
        public int ViewCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public Image Image { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
