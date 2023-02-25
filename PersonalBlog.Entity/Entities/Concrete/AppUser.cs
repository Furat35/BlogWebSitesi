using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Core.Entities.Abstract;

namespace YoutubeBlog.Entity.Entities.Concrete
{
    public class AppUser : IdentityUser<Guid>, IEntityBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }
        public ICollection<Article> Articles { get; set; }
        public bool IsDeleted { get; set; }
    }
}
