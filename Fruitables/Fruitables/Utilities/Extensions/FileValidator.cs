﻿using Fruitables.Utilities.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Fruitables.Utilities.Extensions
{
    public static class FileValidator
    {

        public static bool ValidateType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static bool ValidateSize(this IFormFile file,FileSize filesize, int size)
        {
            switch (filesize)
            {
                case FileSize.KB:
                    return file.Length <= size * 1024;
                case FileSize.MB:
                    return file.Length <= size * 1024 * 1024;
                case FileSize.GB:
                    return file.Length <= size * 1024 * 1024 * 1024;
            }
            return false;
        }

        private static string _getPath(params string[] roots)
        {
            string path = string.Empty;

            for (int i = 0; i < roots.Length; i++)
            {
                path = Path.Combine(path, roots[i]);
            }

            return path;
        }

        public static async Task<string> CreateFileAsync(this IFormFile file, params string[] roots)
        {
            string extension = Path.GetExtension(file.FileName);
            string fileName = string.Concat(Guid.NewGuid().ToString(), extension);

            string path = _getPath(roots);
            path = Path.Combine(path, fileName);

            using (FileStream fl = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fl);
            }

            return fileName;
        }

        public static string DeleteFile(this string fileName, params string[] roots)
        {
            string path = _getPath(roots);
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            return path;
        }


    }
}
