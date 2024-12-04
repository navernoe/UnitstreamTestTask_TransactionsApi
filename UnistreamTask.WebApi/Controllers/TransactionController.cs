using Microsoft.AspNetCore.Mvc;
using UnistreamTask.Application.Interfaces;
using UnistreamTask.Contracts.DTO;
using UnistreamTask.WebApi.Mappers;

namespace UnistreamTask.WebApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransactionController
{
    private readonly ITransactionsRepository _transactionsRepository;
    private readonly TransactionMapper _transactionMapper;

    public TransactionController(
        ITransactionsRepository transactionsRepository,
        TransactionMapper transactionMapper)
    {
        _transactionsRepository = transactionsRepository;
        _transactionMapper = transactionMapper;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionDto>> Create(CreateTransactionRequest request, CancellationToken ct)
    {
        var parameters = _transactionMapper.Map(request);
        var transaction = await _transactionsRepository.Create(parameters, ct);

        return _transactionMapper.Map(transaction);
    }

    [HttpGet]
    public async Task<ActionResult<TransactionDto>> GetById([FromQuery] Guid id, CancellationToken ct)
    {
        var transaction = await _transactionsRepository.GetById(id, ct);

        return _transactionMapper.Map(transaction);
    }
}