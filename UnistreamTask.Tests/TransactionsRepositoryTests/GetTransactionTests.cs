using FluentAssertions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using UnistreamTask.Application.Exceptions;
using UnistreamTask.Application.Models;
using UnistreamTask.Application.Repositories;
using UnistreamTask.Data;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.Tests.TransactionsRepositoryTests;

public class GetTransactionTests : IDisposable
{
    private readonly TransactionsRepository _transactionsRepository;
    private readonly InMemoryDbContext _dbContext;

    public GetTransactionTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<InMemoryDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        _dbContext = new InMemoryDbContext(dbContextOptions);
        var createTransactionParamsValidatorMock = new Mock<IValidator<CreateTransactionParams>>();
        _transactionsRepository = new TransactionsRepository(_dbContext, createTransactionParamsValidatorMock.Object);
    }

    [Fact]
    public async Task GetTransactionByIdSuccessfully()
    {
        var expectedTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };
        var unexpectedTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.UtcNow,
            Amount = 10
        };
        await _dbContext.Transactions.AddRangeAsync(expectedTransaction, unexpectedTransaction);
        await _dbContext.SaveChangesAsync();

        var actualTransaction = await _transactionsRepository.GetById(expectedTransaction.Id, CancellationToken.None);

        actualTransaction.Should().BeEquivalentTo(expectedTransaction);
    }

    [Fact]
    public async Task GetNotExistedTransactionById()
    {
        var notExistedTransactionId = Guid.NewGuid();

        var getTransactionAction = async () => await _transactionsRepository.GetById(notExistedTransactionId, CancellationToken.None);

        await getTransactionAction.Should().ThrowAsync<NotExistedEntityException>();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}