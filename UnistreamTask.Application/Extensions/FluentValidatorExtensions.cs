using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;

namespace UnistreamTask.Application.Extensions;

public static class FluentValidatorExtensions
{
    /// <summary>
    /// To add all validators from the same namespace by one code-string,
    /// you should add this extension class in specified namespace and use this method.
    /// </summary>
    public static void AddFluentValidators(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);
    }

    public static async Task ValidateWithThrowAsync<T>(this IValidator<T> validator, T obj, CancellationToken ct)
    {
        var result = await validator.ValidateAsync(obj, ct);

        if (!result.IsValid)
            throw new ValidationException(
                $"Validation of {typeof(T).Name} completed with errors: {SerializeValidationErrors(result)}");
    }

    private static string SerializeValidationErrors(ValidationResult result)
    {
        var serializedErrors = new StringBuilder("\n");

        foreach (var error in result.Errors)
        {
            serializedErrors.Append($"{error.PropertyName}: {error.ErrorMessage},\n");
        }

        return serializedErrors.ToString();
    }
}