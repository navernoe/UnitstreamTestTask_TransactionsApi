namespace UnistreamTask.Application.Exceptions;

/// <summary>
/// Исключение, выпадающее при попытке добавить дублирующуюся сущность.
/// </summary>
public class DuplicatedEntityException : Exception
{
    public DuplicatedEntityException(string message) : base(message)
    {
    }
}