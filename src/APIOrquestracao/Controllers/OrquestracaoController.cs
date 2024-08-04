using Microsoft.AspNetCore.Mvc;
using APIOrquestracao.Models;
using APIOrquestracao.Clients;
using System.Diagnostics;
using APIOrquestracao.Tracing;

namespace APIOrquestracao.Controllers;

[ApiController]
[Route("[controller]")]
public class OrquestracaoController : ControllerBase
{
    private readonly ILogger<OrquestracaoController> _logger;
    private readonly IConfiguration _configuration;
    private readonly ContagemClient _contagemClient;

    public OrquestracaoController(ILogger<OrquestracaoController> logger,
        IConfiguration configuration, ContagemClient contagemClient)
    {
        _logger = logger;
        _configuration = configuration;
        _contagemClient = contagemClient;
    }

    [HttpGet]
    public async Task<ResultadoOrquestracao> Get()
    {
        var resultado = new ResultadoOrquestracao
        {
            Horario = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        var urlApiContagem = _configuration["ApiContagem"]!;
        resultado.ContagemPostgres =
            await _contagemClient.ObterContagemAsync(urlApiContagem);
        _logger.LogInformation($"Valor contagem Redis: {resultado.ContagemPostgres!.ValorAtual}");
        
        return resultado;
    }
}
