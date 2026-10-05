using DeskFlow.API.Models.Entities;
using DeskFlow.API.Models.Requests;
using DeskFlow.API.Repositories;

namespace DeskFlow.API.Services;

public class InteracaoService
{
    private readonly InteracaoRepository _repository;
    private readonly ChamadoRepository _chamadoRepository;

    public InteracaoService(
        InteracaoRepository repository,
        ChamadoRepository chamadoRepository)
    {
        _repository = repository;
        _chamadoRepository = chamadoRepository;
    }

    public async Task<Interacao> CriarAsync(int chamadoId, CriarInteracaoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Autor))
            throw new ArgumentException("O autor da interação é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Mensagem))
            throw new ArgumentException("A mensagem da interação é obrigatória.");

        var chamado = await _chamadoRepository.BuscarPorIdAsync(chamadoId)
            ?? throw new KeyNotFoundException("Chamado não encontrado.");

        if (chamado.Status == StatusChamado.Fechado)
            throw new InvalidOperationException("Não é permitido adicionar interações a um chamado fechado.");

        var interacao = new Interacao
        {
            ChamadoId = chamadoId,
            Autor = request.Autor.Trim(),
            Mensagem = request.Mensagem.Trim(),
            DataRegistro = DateTime.Now
        };

        return await _repository.CriarAsync(interacao);
    }
}
