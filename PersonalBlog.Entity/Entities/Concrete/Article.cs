using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Core.Entities.Abstract;
using YoutubeBlog.Core.Entities.Concrete;

namespace YoutubeBlog.Entity.Entities.Concrete
{
    public class Article : EntityBase
    {
        public Article()
        {

        }
        public Article(string title, string content, Guid userId, string createdBy, Guid categoryId, Guid imageId)
        {
            Title = title;
            Content = content;
            UserId = userId;
            CategoryId = categoryId;
            ImageId = imageId;
            CreatedBy = createdBy;
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public int ViewCount { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid? ImageId { get; set; } 
        public Image Image { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
    
}

 