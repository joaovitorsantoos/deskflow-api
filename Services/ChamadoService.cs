using DeskFlow.API.Models.Entities;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class ChamadoService
{
    private readonly ChamadoRepository _repository;
    private readonly CategoriaRepository _categoriaRepository;

    public ChamadoService(
        ChamadoRepository repository,
        CategoriaRepository categoriaRepository)
    {
        _repository = repository;
        _categoriaRepository = categoriaRepository;
    }

    public async Task<List<Chamado>> ListarAsync(
        StatusChamado? status,
        PrioridadeChamado? prioridade,
        int? categoriaId)
    {
        return await _repository.ListarAsync(status, prioridade, categoriaId);
    }

    public async Task<Chamado?> BuscarPorIdAsync(int id)
    {
        return await _repository.BuscarPorIdAsync(id);
    }

    public async Task<Chamado> CriarAsync(Chamado chamado)
    {
        var categoria = await _categoriaRepository.BuscarPorIdAsync(chamado.CategoriaId);

        if (categoria == null)
            throw new Exception("Categoria não encontrada.");

        chamado.Status = StatusChamado.Aberto;
        chamado.DataAbertura = DateTime.Now;
        chamado.DataFechamento = null;
        chamado.Solucao = null;

        return await _repository.CriarAsync(chamado);
    }

    public async Task<bool> IniciarAsync(int id)
    {
        var chamado = await _repository.BuscarPorIdAsync(id);

        if (chamado == null)
            return false;

        if (chamado.Status != StatusChamado.Aberto)
            throw new Exception("Somente chamados abertos podem ser iniciados.");

        chamado.Status = StatusChamado.EmAndamento;

        await _repository.AtualizarAsync(chamado);
        return true;
    }

    public async Task<bool> EncerrarAsync(int id, string solucao)
    {
        var chamado = await _repository.BuscarPorIdAsync(id);

        if (chamado == null)
            return false;

        if (chamado.Status != StatusChamado.EmAndamento)
            throw new Exception("Somente chamados em andamento podem ser encerrados.");

        if (string.IsNullOrWhiteSpace(solucao))
            throw new Exception("A solução é obrigatória.");

        chamado.Status = StatusChamado.Fechado;
        chamado.Solucao = solucao;
        chamado.DataFechamento = DateTime.Now;

        await _repository.AtualizarAsync(chamado);
        return true;
    }
}