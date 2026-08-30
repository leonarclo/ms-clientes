using Clientes.Application.Dtos;
using Clientes.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.Api.Controllers;

[ApiController]
[Route("api/v1/clientes")]
public sealed class ClientesController(
    CriarClienteUseCase criarCliente,
    ObterClienteUseCase obterCliente) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ClienteResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Criar(
        CriarClienteRequest request,
        CancellationToken cancellationToken)
    {
        var cliente = await criarCliente.ExecutarAsync(request, cancellationToken);

        return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ClienteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var cliente = await obterCliente.ExecutarAsync(id, cancellationToken);

        return Ok(cliente);
    }
}
