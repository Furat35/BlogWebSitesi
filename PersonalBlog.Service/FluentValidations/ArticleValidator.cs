using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using FluentValidation;
using FluentValidation.Results;

namespace YoutubeBlog.Service.FluentValidations
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(_ => _.Title)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(150)
                .WithName("Başlık");

            RuleFor(_ => _.Content)
                .NotEmpty()
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(150)
                .WithName("İçerik");

        }
    }
}
