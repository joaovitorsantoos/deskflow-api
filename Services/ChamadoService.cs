using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
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

    public Task<List<Chamado>> ListarAsync(
        StatusChamado? status,
        PrioridadeChamado? prioridade,
        int? categoriaId)
    {
        return _repository.ListarAsync(status, prioridade, categoriaId);
    }

    public Task<Chamado?> BuscarPorIdAsync(int id)
    {
        return _repository.BuscarPorIdAsync(id);
    }

    public async Task<Chamado> CriarAsync(CriarChamadoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Titulo))
            throw new ArgumentException("O título do chamado é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Descricao))
            throw new ArgumentException("A descrição do chamado é obrigatória.");

        if (string.IsNullOrWhiteSpace(request.SolicitanteNome))
            throw new ArgumentException("O nome do solicitante é obrigatório.");

        if (!Enum.IsDefined(request.Prioridade))
            throw new ArgumentException("Prioridade inválida.");

        var categoria = await _categoriaRepository.BuscarPorIdAsync(request.CategoriaId);
        if (categoria is null)
            throw new ArgumentException("A categoria informada não existe.");

        var chamado = new Chamado
        {
            Titulo = request.Titulo.Trim(),
            Descricao = request.Descricao.Trim(),
            Prioridade = request.Prioridade,
            Status = StatusChamado.Aberto,
            SolicitanteNome = request.SolicitanteNome.Trim(),
            DataAbertura = DateTime.Now,
            DataFechamento = null,
            Solucao = null,
            CategoriaId = request.CategoriaId
        };

        await _repository.CriarAsync(chamado);

        return await _repository.BuscarPorIdAsync(chamado.Id) ?? chamado;
    }

    public async Task IniciarAsync(int id)
    {
        var chamado = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException("Chamado não encontrado.");

        if (chamado.Status != StatusChamado.Aberto)
            throw new InvalidOperationException("Somente chamados com status Aberto podem ser iniciados.");

        chamado.Status = StatusChamado.EmAndamento;
        await _repository.AtualizarAsync(chamado);
    }

    public async Task EncerrarAsync(int id, string solucao)
    {
        var chamado = await _repository.BuscarPorIdAsync(id)
            ?? throw new KeyNotFoundException("Chamado não encontrado.");

        if (chamado.Status != StatusChamado.EmAndamento)
            throw new InvalidOperationException("Somente chamados EmAndamento podem ser encerrados.");

        if (string.IsNullOrWhiteSpace(solucao))
            throw new ArgumentException("A solução é obrigatória para encerrar o chamado.");

        chamado.Status = StatusChamado.Fechado;
        chamado.Solucao = solucao.Trim();
        chamado.DataFechamento = DateTime.Now;

        await _repository.AtualizarAsync(chamado);
    }
}
