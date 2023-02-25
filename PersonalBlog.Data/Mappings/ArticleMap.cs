    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Data.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(_ => _.Title).HasMaxLength(150);

            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "Visual Studio'ya Giriş",
                Content = "Visual studio, Blog orem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("66349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Firat Ortac",
                CreatedDate = new DateTime(2023, 2, 15),
                UserId = Guid.Parse("20461BA2-1457-4303-AEF9-15173DDBB9B5")
                
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Javascript Nedir?",
                Content = "Javascript Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("58349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 4, 10),
                UserId = Guid.Parse("99461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "C# Hakkinda",
                Content = "C# Hakkinda Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("98349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 6, 22),
                UserId = Guid.Parse("20461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = ".Net Developer Olmak",
                Content = ".Net Developer Olmak Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("98349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 10, 7),
                UserId = Guid.Parse("84461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Identity Framework Kullanımı",
                Content = "Identity Framework Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("98349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 2, 2),
                UserId = Guid.Parse("99461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Python Fonksiyon Yazımı",
                Content = "Python Fonksiyon Yazımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("53349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 12, 11),
                UserId = Guid.Parse("84461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Web Geliştiricilerin Bilmesi Gerekenler",
                Content = "Web Geliştiricilerin Bilmesi Gerekenler Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("58349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023,7, 25),
                UserId = Guid.Parse("99461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = ".Net'de Api Kullanımı",
                Content = ".Net'de Api Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("21349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 3, 1),
                UserId = Guid.Parse("99461BA2-1457-4303-AEF9-15173DDBB9B5")
            },
            new Article
            {
                Id = Guid.NewGuid(),
                Title = "Veritabanında Procedure Kullanımı",
                Content = "Veritabanında Procedure Kullanımı Web Lorem Ipsum, dizgi ve baskı endüstrisinde kullanılan mıgır metinlerdir. Lorem Ipsum, adı bilinmeyen bir matbaacının bir hurufat numune kitabı oluşturmak üzere bir yazı galerisini alarak karıştırdığı",
                ViewCount = 0,
                CategoryId = Guid.Parse("13349229-D8AA-4430-8F3A-2BEC4B6816AE"),
                ImageId = Guid.Parse("342a5458-7d57-4dd6-abea-c5e99e626e87"),
                CreatedBy = "Admin test",
                CreatedDate = new DateTime(2023, 9, 9),
                UserId = Guid.Parse("20461BA2-1457-4303-AEF9-15173DDBB9B5")
            });
        }
    }
}
