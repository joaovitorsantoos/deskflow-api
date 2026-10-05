using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class CategoriaService
{
    private readonly CategoriaRepository _repository;

    public CategoriaService(CategoriaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _repository.ListarAsync();
    }

    public async Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return await _repository.BuscarPorIdAsync(id);
    }

    public async Task<Categoria> CriarAsync(Categoria categoria)
    {
        return await _repository.CriarAsync(categoria);
    }

    public async Task<bool> AtualizarAsync(int id, Categoria dados)
    {
        var categoria = await _repository.BuscarPorIdAsync(id);

        if (categoria == null)
            return false;

        categoria.Nome = dados.Nome;
        await _repository.AtualizarAsync(categoria);

        return true;
    }

    public async Task<bool> ExcluirAsync(int id)
    {
        var categoria = await _repository.BuscarPorIdAsync(id);

        if (categoria == null)
            return false;

        await _repository.ExcluirAsync(categoria);
        return true;
    }
}