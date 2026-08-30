using Clientes.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Clientes.Api;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext contexto,
        Exception excecao,
        CancellationToken cancellationToken)
    {
        var (status, titulo) = excecao switch
        {
            ClienteNaoEncontradoException => (StatusCodes.Status404NotFound, "Cliente não encontrado"),
            CpfJaCadastradoException => (StatusCodes.Status409Conflict, "CPF já cadastrado"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Requisição inválida"),
            _ => (StatusCodes.Status500InternalServerError, "Erro interno")
        };

        if (status == StatusCodes.Status500InternalServerError)
            logger.LogError(excecao, "Erro não tratado ao processar {Caminho}", contexto.Request.Path);

        contexto.Response.StatusCode = status;

        await contexto.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = status,
            Title = titulo,
            // Mensagem interna não vaza em erro 500.
            Detail = status == StatusCodes.Status500InternalServerError ? null : excecao.Message,
            Instance = contexto.Request.Path
        }, cancellationToken);

        return true;
    }
}
