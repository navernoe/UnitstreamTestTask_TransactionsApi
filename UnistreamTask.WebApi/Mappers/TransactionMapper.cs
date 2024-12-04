using Riok.Mapperly.Abstractions;
using UnistreamTask.Application.Models;
using UnistreamTask.Contracts.DTO;
using UnistreamTask.Data.Entities;

namespace UnistreamTask.WebApi.Mappers;

[Mapper]
public partial class TransactionMapper
{
    public partial CreateTransactionParams Map(CreateTransactionRequest request);
    public partial TransactionDto Map(Transaction transaction);
}