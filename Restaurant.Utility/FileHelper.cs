using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Utility
{
   public static class FileHelper
    {
        private static readonly string _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Utilities", "UploadedFiles");

        static FileHelper()
        {
            // Ensure the folder exists
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public static async Task<string> SaveFileAsync(Stream fileStream, string fileName)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            return uniqueFileName; // Return only the file name
        }

        public static string GetFilePath(string fileName)
        {
            return Path.Combine(_uploadsFolder, fileName);
        }

        public static bool FileExists(string fileName)
        {
            return File.Exists(GetFilePath(fileName));
        }

        public static void DeleteFile(string fileName)
        {
            var filePath = GetFilePath(fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}