using DeskFlow.API.Models.Entities;

namespace DeskFlow.API.Models.Requests;

public record CriarChamadoRequest(
    string Titulo,
    string Descricao,
    PrioridadeChamado Prioridade,
    string SolicitanteNome,
    int CategoriaId
);
