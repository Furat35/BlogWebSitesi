using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;

namespace YoutubeBlog.Service.FluentValidations
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(_ => _.Name)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(100)
                .WithName("Kategori Adı");
        }
    }
}
