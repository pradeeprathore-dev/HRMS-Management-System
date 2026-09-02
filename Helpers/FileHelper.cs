using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public static class FileHelper
    {
        public static readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".jpg",
            ".jpeg",
            ".png",
            ".doc",
            ".docx"
        };

        public static bool IsValidExtension(string extension)
        {
            return AllowedExtensions.Contains(extension.ToLower());
        }

        public static bool IsValidFileSize(IFormFile file)
        {
            return file.Length <= 5 * 1024 * 1024;
        }

        public static string GenerateUniqueFileName(string fileName)
        {
            return Guid.NewGuid().ToString() +
                   Path.GetExtension(fileName);
        }
    }
}
