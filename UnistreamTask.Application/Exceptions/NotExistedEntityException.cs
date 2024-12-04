namespace UnistreamTask.Application.Exceptions;

/// <summary>
/// Исключение, выпадающее при попытке работать с сущностью, которой не существует.
/// </summary>
public class NotExistedEntityException : Exception
{
    public NotExistedEntityException(string message) : base(message)
    {
    }
}