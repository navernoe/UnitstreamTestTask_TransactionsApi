using FluentValidation;
using Microsoft.EntityFrameworkCore;
using UnistreamTask.Application.Exceptions;
using UnistreamTask.Application.Extensions;
using UnistreamTask.Application.Interfaces;
using UnistreamTask.Application.Models;
using UnistreamTask.Data;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.Application.Repositories;

/// <summary>
/// Репозиторий для работы с транзакциями.
/// </summary>
public class TransactionsRepository : ITransactionsRepository
{
    private readonly InMemoryDbContext _dbContext;
    private readonly IValidator<CreateTransactionParams> _createTransactionParamsValidator;

    public TransactionsRepository(
        InMemoryDbContext dbContext,
        IValidator<CreateTransactionParams> createTransactionParamsValidator)
    {
        _dbContext = dbContext;
        _createTransactionParamsValidator = createTransactionParamsValidator;
    }

    public async Task<Transaction> Create(CreateTransactionParams parameters, CancellationToken ct)
    {
        await _createTransactionParamsValidator.ValidateWithThrowAsync(parameters, ct);

        var newTransaction = new Transaction
        {
            Id = parameters.Id,
            TransactionDate = parameters.TransactionDate,
            Amount = parameters.Amount,
        };

        try
        {
            await _dbContext.AddAsync(newTransaction, ct);
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception e)
        when(e.IsDuplicatedEntityException())
        {
            throw new DuplicatedEntityException($"Transaction already exists, id = {newTransaction.Id}");
        }

        return newTransaction;
    }

    public async Task<Transaction> GetById(Guid id, CancellationToken ct)
    {
        return await _dbContext.Transactions.SingleOrDefaultAsync(t => t.Id == id, ct)
               ?? throw new NotExistedEntityException($"Transaction doesn't exist, id = {id}");
    }
}