using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using UnistreamTask.Application.Exceptions;
using UnistreamTask.Application.Models;
using UnistreamTask.Application.Repositories;
using UnistreamTask.Application.Settings;
using UnistreamTask.Application.Validation;
using UnistreamTask.Data;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.Tests.TransactionsRepositoryTests;

public class CreateTransactionTests : IDisposable
{
    private readonly TransactionsRepository _transactionsRepository;
    private readonly InMemoryDbContext _dbContext;

    public CreateTransactionTests()
    {
        var appSettings = new AppSettings
        {
            TransactionsCountLimit = 2
        };
        var appSettingsOptsMock = new Mock<IOptionsSnapshot<AppSettings>>();
        appSettingsOptsMock.Setup(m => m.Value).Returns(appSettings);
        _dbContext = new InMemoryDbContext(new DbContextOptions<InMemoryDbContext>());
        var createTransactionParamsValidator = new CreateTransactionParamsValidator(_dbContext, appSettingsOptsMock.Object);
        _transactionsRepository = new TransactionsRepository(_dbContext, createTransactionParamsValidator);
    }

    [Fact]
    public async Task CreateNewTransactionsSuccessfully()
    {
        var parameters = new CreateTransactionParams
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };

        var newTransaction = await _transactionsRepository.Create(parameters, CancellationToken.None);

        newTransaction.Should().BeEquivalentTo(parameters);
    }

    [Fact]
    public async Task CreateMoreThanTransactionsCountLimit()
    {
        var transaction1 = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };
        var transaction2 = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };
        await _dbContext.Transactions.AddRangeAsync(transaction1, transaction2);
        await _dbContext.SaveChangesAsync();

        var parameters = new CreateTransactionParams
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 100
        };
        var createTransactionAction = async () =>  await _transactionsRepository.Create(parameters, CancellationToken.None);

        await createTransactionAction.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task CreateDuplicatedTransaction()
    {
        var existedTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };
        await _dbContext.Transactions.AddAsync(existedTransaction);
        await _dbContext.SaveChangesAsync();

        var parameters = new CreateTransactionParams
        {
            Id = existedTransaction.Id,
            TransactionDate = DateTime.UtcNow,
            Amount = 100
        };
        var createTransactionAction = async () =>  await _transactionsRepository.Create(parameters, CancellationToken.None);

        await createTransactionAction.Should().ThrowAsync<DuplicatedEntityException>();
    }

    public async void Dispose()
    {
        await _dbContext.DisposeAsync();
    }
}