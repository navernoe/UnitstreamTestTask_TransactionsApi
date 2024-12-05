using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UnistreamTask.Application.Exceptions;
using UnistreamTask.Application.Models;
using UnistreamTask.Application.Settings;
using UnistreamTask.Data;

namespace UnistreamTask.Application.Validation;

public class CreateTransactionParamsValidator : AbstractValidator<CreateTransactionParams>
{
    public CreateTransactionParamsValidator(InMemoryDbContext dbContext, IOptionsSnapshot<AppSettings> appSettingsOpts)
    {
        var transactionsCountLimit = appSettingsOpts.Value.TransactionsCountLimit;
        RuleFor(p => p)
            .MustAsync(async (_, ct) =>
            {
                var transactionsCount = await dbContext.Transactions.CountAsync(ct);
                var wasLimitReached = transactionsCount >= transactionsCountLimit;

                if (wasLimitReached)
                    throw new StorageOverfullException(
                        $"Transactions storage is full, can't be more than {transactionsCountLimit} transactions.");

                return !wasLimitReached;
            });
    }
}