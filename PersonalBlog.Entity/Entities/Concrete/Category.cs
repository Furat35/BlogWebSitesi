using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Core.Entities.Abstract;
using YoutubeBlog.Core.Entities.Concrete;

namespace YoutubeBlog.Entity.Entities.Concrete
{
    public class Category : EntityBase
    {
        public Category()
        {

        }
        public Category(string name, string createdBy)
        {
            Name = name;
            CreatedBy = createdBy;
        }
        public string Name { get; set; }
        public ICollection<Article>? Articles { get; set; }


    }

}
