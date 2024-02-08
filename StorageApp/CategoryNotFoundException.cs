using System;

namespace StorageApp
{
    internal class CategoryNotFoundException(string categoryId) : Exception($"Категория {categoryId} не найдена.");

    internal class DataException(params string[] errorValues) : Exception($"Были получены некорректные данные из {errorValues}");

    internal class NullDataException() : Exception("Был получен пустой массив из Базы Данных");
}
