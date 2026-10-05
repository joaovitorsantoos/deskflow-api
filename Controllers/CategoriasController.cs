using DeskFlow.API.Models.Entities;
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

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> BuscarPorId(int id)
    {
        var categoria = await _service.BuscarPorIdAsync(id);

        if (categoria == null)
            return NotFound();

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> Criar(Categoria categoria)
    {
        var criada = await _service.CriarAsync(categoria);

        return CreatedAtAction(
            nameof(BuscarPorId),
            new { id = criada.Id },
            criada
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, Categoria categoria)
    {
        var atualizado = await _service.AtualizarAsync(id, categoria);

        if (!atualizado)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Excluir(int id)
    {
        var excluido = await _service.ExcluirAsync(id);

        if (!excluido)
            return NotFound();

        return NoContent();
    }
}