using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Data.UnitOfWorks;
using YoutubeBlog.Entity.Entities.Concrete;
using YoutubeBlog.Entity.Models.DTOs.Roles;
using YoutubeBlog.Service.Services.Abstract;

namespace YoutubeBlog.Service.Services.Concrete
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> GetAllRolesAsync(Expression<Func<AppRole, bool>> predicate = null)
        {
            var roles = (await _unitOfWork.GetRepository<AppRole>().GetAllAsync(predicate)).Where(_ => _.NormalizedName != "SUPERADMIN");
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<string> CreateRoleAsync(RoleAddDto roleDto)
        {
            var roleExist = (await _unitOfWork.GetRepository<AppRole>().GetAllAsync(_ => !_.IsDeleted && _.NormalizedName == roleDto.Name.ToUpper())).FirstOrDefault();

            if(roleExist == null)
            {
                var map = _mapper.Map<AppRole>(roleDto);
                map.NormalizedName = roleDto.Name.ToUpper();
                map.IsDeleted = false;

                await _unitOfWork.GetRepository<AppRole>().AddAsync(map);
                var effectedRows = await _unitOfWork.SaveAsync();

                if (effectedRows > 0)
                {
                    return roleDto.Name;
                }
            }
            else
            {
                var map = _mapper.Map<AppRole>(roleDto);
                map.IsDeleted = false;
                map.ConcurrencyStamp = Guid.NewGuid().ToString();

                await _unitOfWork.GetRepository<AppRole>().UpdateAsync(map);
                var effectedRows = await _unitOfWork.SaveAsync();

                if (effectedRows > 0)
                {
                    return roleDto.Name;
                }
            }


            return null;
        }


        public async Task<string> SafeDeleteRoleAsync(Guid roleId)
        {
            var role = await _unitOfWork.GetRepository<AppRole>().GetByGuidAsync(roleId);
            role.IsDeleted = true;
            await _unitOfWork.GetRepository<AppRole>().UpdateAsync(role);
            var effectedRows = await _unitOfWork.SaveAsync();

            return effectedRows > 0 ? role.Name : null;
        }

        public async Task<string> DeleteRoleAsync(Guid roleId)
        {
            var role = await _unitOfWork.GetRepository<AppRole>().GetByGuidAsync(roleId);
            await _unitOfWork.GetRepository<AppRole>().DeleteAsync(role);
            var effectedRows = await _unitOfWork.SaveAsync();

            return effectedRows > 0 ? role.Name : null;
        }

        public async Task<string> UpdateRoleAsync(RoleUpdateDto roleUpdateDto)
        {
            var role = await _unitOfWork.GetRepository<AppRole>().GetByGuidAsync(roleUpdateDto.Id);
            
            if (role != null)
            {
                role.Name = roleUpdateDto.Name;
                role.NormalizedName = roleUpdateDto.Name.ToUpper();
                role.ConcurrencyStamp = Guid.NewGuid().ToString();

                await _unitOfWork.GetRepository<AppRole>().UpdateAsync(role);
                int effectedRows = await _unitOfWork.SaveAsync();

                if(effectedRows > 0)
                {
                    return role.Name;
                }
            }

            return null;
        }

        public async Task<AppRole> GetRoleByGuidAsync(Guid roleId)
        {
            var role = await _unitOfWork.GetRepository<AppRole>().GetByGuidAsync(roleId);
            return role;
        }

        public async Task<string> GetRoleGuidAsync(string name)
        {
            var role = await _unitOfWork.GetRepository<AppRole>().GetAsync(_ => _.NormalizedName == name.ToUpper());
            return role != null ? role.Id.ToString() : null;
        }
    }
}
