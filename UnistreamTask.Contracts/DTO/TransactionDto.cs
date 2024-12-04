namespace UnistreamTask.Contracts.DTO;

/// <summary>
/// Транзакция.
/// </summary>
public record TransactionDto
{
    public required Guid Id { get; init; }
    public required DateTime TransactionDate { get; init; }
    public required decimal Amount { get; init; }
}