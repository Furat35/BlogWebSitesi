using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Service.FluentValidations
{
    public class RoleValidator : AbstractValidator<AppRole>
    {
        public RoleValidator()
        {
            RuleFor(_ => _.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(2)
                .WithName("Rol");
        }
    }
}
