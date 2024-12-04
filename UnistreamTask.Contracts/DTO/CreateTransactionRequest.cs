namespace UnistreamTask.Contracts.DTO;

/// <summary>
/// Запрос на создание транзакции
/// </summary>
public record CreateTransactionRequest
{
    public required Guid Id { get; init; }
    public required DateTime TransactionDate { get; init; }
    public required decimal Amount { get; init; }
};