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
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(
                new AppUserRole
                {
                    UserId = Guid.Parse("20461BA2-1457-4303-AEF9-15173DDBB9B5"),
                    RoleId = Guid.Parse("C35E880E-17C4-4726-A20E-FF817FBB16AE")
                },
                new AppUserRole
                {
                    UserId = Guid.Parse("84461BA2-1457-4303-AEF9-15173DDBB9B5"),
                    RoleId = Guid.Parse("C6992CA2-86D3-40BE-A85D-257FB72BBBEB")
                },
                 new AppUserRole
                 {
                     UserId = Guid.Parse("99461BA2-1457-4303-AEF9-15173DDBB9B5"),
                     RoleId = Guid.Parse("C6992CA2-86D3-40BE-A85D-257FB72BBBEB")
                 });
        }
    }
}
