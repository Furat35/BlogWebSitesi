using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Data.Mappings
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(new Image
            {
                Id = Guid.Parse("54349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                FileName = "~/images/testimage",
                FileType = "jpg",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                isDeleted = false
            }, new Image
            {
                Id = Guid.Parse("77349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                FileName = "~/images/testimage",
                FileType = "jpg",
                CreatedBy = "SuperAdmin",
                CreatedDate = DateTime.Now,
                isDeleted = false
            }, new Image
            {
                Id = Guid.Parse("44349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                FileName = "~/images/testimage",
                FileType = "jpg",
                CreatedBy = "Admin Test",
                CreatedDate = DateTime.Now,
                isDeleted = false
            },
            new Image
            {
                Id = Guid.Parse("242a5458-7d57-4dd6-abea-c5e99e626e87"),
                FileName = "~/images/user-images/FiratOrtac_2439664.png",
                FileType = "image/png",
                CreatedBy = "SuperAdmin",
                CreatedDate = DateTime.Now,
                isDeleted = false
            },
            new Image
            {
                Id = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                FileName = "~/images/article-images/emptyImage.jpg",
                FileType = "image/jpg",
                CreatedBy = "SuperAdmin",
                CreatedDate = DateTime.Now,
                isDeleted = false
            });
        }
    }
}
