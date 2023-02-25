using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using System.Xml.Linq;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;
using YoutubeBlog.Service.Services.Concrete;
using YoutubeBlog.Web.Const;
using YoutubeBlog.Web.ResultMessages;

namespace YoutubeBlog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{RoleConst.SuperAdmin}")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Category> _validator;
        private readonly IToastNotification _toast;

        public CategoryController(ICategoryService categoryService, IMapper mapper, IValidator<Category> validator, IToastNotification toast)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeleteAsync();
            return View(categories);
        }

        public async Task<IActionResult> DeletedCategory()
        {
            var categories = await _categoryService.GetAllCategoriesDeletedAsync();
            return View(categories);
        }


        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            Category category = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator.ValidateAsync(category);

            if (result.IsValid)
            {
                string name = await _categoryService.CreateCategoryAsync(categoryAddDto);
                
                if(name != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Category.Add(name), new ToastrOptions { Title = Messages.ToastTitle.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.ToastTitle.Error });
                }

                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(categoryAddDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDto categoryAddDto)
        {
            Category category = _mapper.Map<Category>(categoryAddDto);
            var result = await _validator.ValidateAsync(category);

            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsync(categoryAddDto);
                _toast.AddSuccessToastMessage(Messages.Category.Add(category.Name), new ToastrOptions { Title = Messages.GlobalMessage.Success });

                return Json(Messages.Category.Add(categoryAddDto.Name));
            }
            else
            {
                _toast.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = Messages.GlobalMessage.Error });
                return Json(result.Errors.First().ErrorMessage);
            }
        }

        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByGuidAsync(categoryId);

            if (category != null)
            {
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return View(categoryUpdateDto);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {
            var map = _mapper.Map<Category>(categoryUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                var name = await _categoryService.UpdateCategoryAsync(categoryUpdateDto);

                if (name != null)
                {
                    _toast.AddSuccessToastMessage(Messages.Category.Update(name), new ToastrOptions { Title = Messages.GlobalMessage.Success });
                }
                else
                {
                    _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.GlobalMessage.Error });
                }
                 
                return RedirectToAction("Index", "Category", new { Area = "Admin" });
            }

            result.AddToModelState(this.ModelState);
            return View(categoryUpdateDto);
        }

        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var name = await _categoryService.SafeDeleteCategoryAsync(categoryId);

            if (name != null)
            {
                _toast.AddSuccessToastMessage(Messages.Category.Delete(name), new ToastrOptions { Title = Messages.GlobalMessage.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.GlobalMessage.Error });
            }

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var name = await _categoryService.UndoCategoryAsync(categoryId);

            if (name != null)
            {
                _toast.AddSuccessToastMessage(Messages.Category.UndoDelete(name), new ToastrOptions { Title = Messages.GlobalMessage.Success });
            }
            else
            {
                _toast.AddErrorToastMessage(Messages.GlobalMessage.Error, new ToastrOptions { Title = Messages.GlobalMessage.Error });
            }
                
            return RedirectToAction("DeletedCategory", "Category", new { Area = "Admin" });
        }
    }
}
