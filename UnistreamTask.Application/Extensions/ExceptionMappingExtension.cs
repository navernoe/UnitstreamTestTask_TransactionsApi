namespace UnistreamTask.Application.Extensions;

public static class ExceptionMappingExtension
{
    public static bool IsDuplicatedEntityException(this Exception ex)
    {
        if (ex is not (ArgumentException or InvalidOperationException))
            return false;

        return ex.TargetSite?.Name == "ThrowAddingDuplicateWithKeyArgumentException" || ex.TargetSite?.Name == "ThrowIdentityConflict";
    }
}