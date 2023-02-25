using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeBlog.Entity.Models.DTOs.Images;
using YoutubeBlog.Entity.Enums;

namespace YoutubeBlog.Service.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadDto> Upload(string name, IFormFile imageFile, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
