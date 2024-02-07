using System;

namespace StorageApp
{
    internal class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(string categoryId) : base($"Категория {categoryId} не найдена.")
        {

        }
    }
}
