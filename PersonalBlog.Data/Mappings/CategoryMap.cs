using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category
            {
                Id = Guid.Parse("98349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "Asp.net Core",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            },new Category
            {
                Id = Guid.Parse("66349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "Visual Studio",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            }, new Category
            {
                Id = Guid.Parse("53349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "Python",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            },
            new Category
            {
                Id = Guid.Parse("21349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "C#",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            },
            new Category
            {
                Id = Guid.Parse("13349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "SQL",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            },
            new Category
            {
                Id = Guid.Parse("58349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                Name = "Javascript",
                CreatedBy = "Firat Ortac",
                CreatedDate = DateTime.Now,
                isDeleted = false,
            });
        }
    }
}
