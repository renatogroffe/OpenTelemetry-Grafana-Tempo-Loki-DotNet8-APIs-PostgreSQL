using Microsoft.AspNetCore.Mvc;
using APIContagem.Data;
using APIContagem.Models;
using APIContagem.Logging;

namespace APIContagem.Controllers;

[ApiController]
[Route("[controller]")]
public class ContadorController : ControllerBase
{
    private static readonly Contador _CONTADOR = new();
    private readonly ILogger<ContadorController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ContagemRepository _repository;

    public ContadorController(ILogger<ContadorController> logger,
        IConfiguration configuration,
        ContagemRepository repository)
    {
        _logger = logger;
        _configuration = configuration;
        _repository = repository;
    }

    [HttpGet]
    public ResultadoContador Get()
    {
        int valorAtualContador;

        lock (_CONTADOR)
        {
            _CONTADOR.Incrementar();
            valorAtualContador = _CONTADOR.ValorAtual;
        }

        _logger.LogValorAtual(valorAtualContador);

        var resultado = new ResultadoContador()
        {
            ValorAtual = valorAtualContador,
            Producer = _CONTADOR.Local,
            Kernel = _CONTADOR.Kernel,
            Framework = _CONTADOR.Framework,
            Mensagem = _configuration["MensagemVariavel"]
        };
        _repository.Insert(resultado);
        _logger.LogInformation($"Registro inserido com sucesso! Valor: {valorAtualContador}");

        return resultado;
    }
}