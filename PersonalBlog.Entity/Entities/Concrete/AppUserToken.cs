using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Core.Entities.Abstract;

namespace YoutubeBlog.Entity.Entities.Concrete
{
    public class AppUserToken : IdentityUserToken<Guid>, IEntityBase
    {
    }
}
