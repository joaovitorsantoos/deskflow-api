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

    public Task<List<Categoria>> ListarAsync()
    {
        return _repository.ListarAsync();
    }

    public Task<Categoria?> BuscarPorIdAsync(int id)
    {
        return _repository.BuscarPorIdAsync(id);
    }

    public async Task<Categoria> CriarAsync(string nome)
    {
        ValidarNome(nome);

        var categoria = new Categoria
        {
            Nome = nome.Trim()
        };

        return await _repository.CriarAsync(categoria);
    }

    public async Task AtualizarAsync(int id, string nome)
    {
        ValidarNome(nome);

        var categoria = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException("Categoria não encontrada.");

        categoria.Nome = nome.Trim();
        await _repository.AtualizarAsync(categoria);
    }

    public async Task ExcluirAsync(int id)
    {
        var categoria = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException("Categoria não encontrada.");

        if (await _repository.PossuiChamadosAsync(id))
            throw new InvalidOperationException("A categoria não pode ser excluída porque possui chamados associados.");

        await _repository.ExcluirAsync(categoria);
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da categoria é obrigatório.");
    }
}
