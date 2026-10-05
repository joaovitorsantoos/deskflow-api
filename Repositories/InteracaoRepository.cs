using DeskFlow.API.Data;
using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Repositories;

public class InteracaoRepository
{
    private readonly AppDbContext _context;

    public InteracaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Interacao> CriarAsync(Interacao interacao)
    {
        _context.Interacoes.Add(interacao);
        await _context.SaveChangesAsync();
        return interacao;
    }
}
