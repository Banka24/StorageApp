using System;

namespace StorageApp
{
    internal class CategoryNotFoundException(string categoryId) : Exception($"Категория {categoryId} не найдена.");

    internal class ReceivedDataException(params string[] errorValues) : Exception($"Были получены некорректные данные из {errorValues}");

}
