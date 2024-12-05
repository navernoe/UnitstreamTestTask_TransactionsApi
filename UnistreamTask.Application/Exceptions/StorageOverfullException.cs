namespace UnistreamTask.Application.Exceptions;

/// <summary>
/// Исключение, возникающее в случае переполнения хранилища.
/// </summary>
public class StorageOverfullException: Exception
{
    public StorageOverfullException(string message) : base(message)
    {
    }
}