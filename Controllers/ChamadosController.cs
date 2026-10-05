using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/chamados")]
public class ChamadosController : ControllerBase
{
    private readonly ChamadoService _chamadoService;
    private readonly InteracaoService _interacaoService;

    public ChamadosController(
        ChamadoService chamadoService,
        InteracaoService interacaoService)
    {
        _chamadoService = chamadoService;
        _interacaoService = interacaoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Chamado>>> Listar(
        [FromQuery] StatusChamado? status,
        [FromQuery] PrioridadeChamado? prioridade,
        [FromQuery] int? categoriaId)
    {
        return Ok(await _chamadoService.ListarAsync(status, prioridade, categoriaId));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Chamado>> BuscarPorId(int id)
    {
        var chamado = await _chamadoService.BuscarPorIdAsync(id);

        if (chamado is null)
            return NotFound(new { erro = "Chamado não encontrado." });

        return Ok(chamado);
    }

    [HttpPost]
    public async Task<ActionResult<Chamado>> Criar([FromBody] CriarChamadoRequest request)
    {
        var chamado = await _chamadoService.CriarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = chamado.Id }, chamado);
    }

    [HttpPatch("{id:int}/iniciar")]
    public async Task<IActionResult> Iniciar(int id)
    {
        await _chamadoService.IniciarAsync(id);
        return NoContent();
    }

    [HttpPatch("{id:int}/encerrar")]
    public async Task<IActionResult> Encerrar(int id, [FromBody] EncerrarChamadoRequest request)
    {
        await _chamadoService.EncerrarAsync(id, request.Solucao);
        return NoContent();
    }

    [HttpPost("{id:int}/interacoes")]
    public async Task<ActionResult<Interacao>> AdicionarInteracao(
        int id,
        [FromBody] CriarInteracaoRequest request)
    {
        var interacao = await _interacaoService.CriarAsync(id, request);
        return Created($"/api/chamados/{id}", interacao);
    }
}
