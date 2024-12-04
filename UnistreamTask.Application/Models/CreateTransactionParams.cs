namespace UnistreamTask.Application.Models;

/// <summary>
/// Параметры для создания транзакции.
/// </summary>
public record CreateTransactionParams
{
    public Guid Id { get; init; }
    public DateTime TransactionDate { get; init; }
    public decimal Amount { get; init; }
}