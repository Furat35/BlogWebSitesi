using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;

namespace YoutubeBlog.Service.Services.Abstract
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllCategoriesNonDeleteAsync();
        Task<List<CategoryDto>> GetAllCategoriesDeletedAsync();
        Task<string> CreateCategoryAsync(CategoryAddDto categoryAddDto);
        Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto);
        Task<Category> GetCategoryByGuidAsync(Guid id);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoCategoryAsync(Guid categoryId);
        Task<List<CategoryDto>> GetFirstNCategoriesAsync(int count);

    }
}
