using UnistreamTask.Application.Models;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.Application.Interfaces;

/// <summary>
/// Интерфейс для репозитория, работающего с транзакциями.
/// </summary>
public interface ITransactionsRepository
{

    /// <summary>
    /// Создать транзакцию.
    /// </summary>
    Task<Transaction> Create(CreateTransactionParams parameters, CancellationToken ct);

    /// <summary>
    /// Получить транзакцию по идентификатору.
    /// </summary>
    Task<Transaction> GetById(Guid id, CancellationToken ct);
}