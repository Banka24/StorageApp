using System;

namespace StorageApp;

internal class CategoryNotFoundException(string categoryId) : Exception($"Категория {categoryId} не найдена.");

internal class DataException(params string[] errorValues) : Exception($"Были получены некорректные данные из {errorValues}");

internal class NullDataException() : Exception("Были получены пустые данные из Базы Данных");

internal class RemoveNullDataException() : Exception("Попытка удалить пустой ");