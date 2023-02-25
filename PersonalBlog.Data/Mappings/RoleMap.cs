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
    public class RoleMap : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            // Primary key
            builder.HasKey(r => r.Id);

            // Index for "normalized" role name to allow efficient lookups
            builder.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();

            // Maps to the AspNetRoles table
            builder.ToTable("AspNetRoles");

            // A concurrency token for use with the optimistic concurrency checking
            builder.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

            // Limit the size of columns to use efficient database types
            builder.Property(u => u.Name).HasMaxLength(256);
            builder.Property(u => u.NormalizedName).HasMaxLength(256);

            // The relationships between Role and other entity types
            // Note that these relationships are configured with no navigation properties

            // Each Role can have many entries in the UserRole join table
            builder.HasMany<AppUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();

            // Each Role can have many associated RoleClaims
            builder.HasMany<AppRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();

            builder.HasData(
                new AppRole
                {
                    Id = Guid.Parse("C35E880E-17C4-4726-A20E-FF817FBB16AE"),
                    Name = "Superadmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    IsDeleted = false
                },
                new AppRole
                {
                    Id = Guid.Parse("C6992CA2-86D3-40BE-A85D-257FB72BBBEB"),
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    IsDeleted = false
                },
                new AppRole
                {
                    Id = Guid.Parse("8DA9A416-CDA8-4D0D-9087-6EA08174E2E2"),
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    IsDeleted = false
                }
                );
        }
    }
}
