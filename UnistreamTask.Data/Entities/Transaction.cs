namespace UnistreamTask.Data.Entities;

/// <summary>
/// Транзакция.
/// </summary>
public record Transaction
{
    public Guid Id { get; init; }
    public DateTime TransactionDate { get; init; }
    public decimal Amount { get; init; }
}