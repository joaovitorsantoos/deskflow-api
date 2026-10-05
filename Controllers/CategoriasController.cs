using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeskFlow.API.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly CategoriaService _service;

    public CategoriasController(CategoriaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<Categoria>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Categoria>> BuscarPorId(int id)
    {
        var categoria = await _service.BuscarPorIdAsync(id);

        if (categoria is null)
            return NotFound(new { erro = "Categoria não encontrada." });

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Criar([FromBody] CategoriaRequest request)
    {
        var categoria = await _service.CriarAsync(request.Nome);
        return CreatedAtAction(nameof(BuscarPorId), new { id = categoria.Id }, categoria);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Atualizar(int id, [FromBody] CategoriaRequest request)
    {
        await _service.AtualizarAsync(id, request.Nome);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Excluir(int id)
    {
        await _service.ExcluirAsync(id);
        return NoContent();
    }
}
