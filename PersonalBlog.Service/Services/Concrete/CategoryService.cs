using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Categories;
using YoutubeBlog.Service.Extensions;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Service.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public CategoryService(IUnitOfWork unitWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.User;
        }

        public async Task<List<CategoryDto>> GetAllCategoriesNonDeleteAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(predicate: _ => !_.isDeleted);
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<List<CategoryDto>> GetFirstNCategoriesAsync(int count)
        {
            var categories = await _unitOfWork.GetRepository<Category>().TakeFirstNItemsAsync(count);
            return _mapper.Map<List<CategoryDto>>(categories);
        }
        public async Task<List<CategoryDto>> GetAllCategoriesDeletedAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(predicate: _ => _.isDeleted);
            return _mapper.Map<List<CategoryDto>>(categories);
        }

        public async Task<string> CreateCategoryAsync(CategoryAddDto categoryAddDto)
        {
            Category category = new(categoryAddDto.Name, _user.GetLoggedInEmail());
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            var effectedRows = await _unitOfWork.SaveAsync();

            return effectedRows > 0 ? categoryAddDto.Name : null;
            
        }

        public async Task<Category> GetCategoryByGuidAsync(Guid id)
        {
            Category category = await _unitOfWork.GetRepository<Category>().GetByGuidAsync(id);
            return category;
        }

        private async Task<Category?> CategoryExistAsync(Guid categoryId, bool isDeleted)
        {
            try
            {
                Category category = await _unitOfWork.GetRepository<Category>().GetAsync(_ => _.Id == categoryId && _.isDeleted == isDeleted);
                return category;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto)
        {
            string userEmail = _user.GetLoggedInEmail();
            Category category = await CategoryExistAsync(categoryUpdateDto.Id, false);

            if (category != null)
            {
                string categoryName = category.Name;

                category.Name = categoryUpdateDto.Name;
                category.ModifiedBy = userEmail;
                category.ModifiedDate = DateTime.Now;

                await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                int effectedRows = await _unitOfWork.SaveAsync();

                if (effectedRows > 0)
                {
                    return categoryName;
                }
            }

            return null;
        }

        public async Task<string> SafeDeleteCategoryAsync(Guid categoryId)
        {
            Category category = await CategoryExistAsync(categoryId, false);

            if (category != null)
            {
                category.isDeleted = true;
                category.DeletedDate = DateTime.Now;
                category.DeletedBy = _user.GetLoggedInEmail();

                await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                int effectedRows = await _unitOfWork.SaveAsync();

                if(effectedRows > 0)
                {
                    return category.Name;
                }
            }

            return null;
        }

        public async Task<string> UndoCategoryAsync(Guid categoryId)
        {
            Category category = await CategoryExistAsync(categoryId, true);

            if(category != null)
            {
                category.isDeleted = false;
                category.DeletedDate = null;
                category.DeletedBy = null;

                await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
                int effectedRows = await _unitOfWork.SaveAsync();
                
                if(effectedRows > 0)
                {
                    return category.Name;
                }
            }

            return null;
        }
    }
}
