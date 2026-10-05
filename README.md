# DeskFlow API — Gestão de Chamados e Helpdesk de TI

API RESTful desenvolvida em C# com .NET 10, Entity Framework Core 10 e SQL Server para gerenciamento de chamados de suporte técnico.

## Funcionalidades

- CRUD de categorias.
- Validação de exclusão de categoria com chamados associados.
- Abertura de chamados com status `Aberto` e data automática.
- Transição de status `Aberto` → `EmAndamento` → `Fechado`.
- Encerramento com solução e data de fechamento.
- Histórico de interações por chamado.
- Bloqueio de interação em chamado fechado.
- Consulta detalhada com Categoria e Interações.
- Filtros por status, prioridade e categoria.
- Tratamento global de erros em JSON.
- Persistência com EF Core e SQL Server.

## Tecnologias

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server LocalDB
- OpenAPI
- Git

## Arquitetura

```text
Controller -> Service -> Repository -> Entity Framework Core -> SQL Server
```

```text
DeskFlow.API/
├── Controllers/
├── Data/
├── Middlewares/
├── Migrations/
├── Models/
│   ├── Entities/
│   └── Requests/
├── Repositories/
├── Scripts/
├── Services/
├── Program.cs
└── appsettings.json
```

## Pré-requisitos

- .NET SDK 10
- SQL Server LocalDB, SQL Server Express ou SQL Server
- Ferramenta `dotnet-ef`

Caso necessário:

```bash
dotnet tool install --global dotnet-ef --version 10.*
```

## Como executar

Na pasta do projeto:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

A URL será exibida no terminal, por exemplo:

```text
http://localhost:5219
```

O documento OpenAPI é disponibilizado em ambiente de desenvolvimento em:

```text
/openapi/v1.json
```

O arquivo `DeskFlow.API.http` contém requisições prontas para testar todos os endpoints pelo VS Code.

## Banco de dados

A conexão padrão está em `appsettings.json`:

```text
Server=(localdb)\MSSQLLocalDB;Database=DeskFlowDb;Trusted_Connection=True;TrustServerCertificate=True;
```

A estrutura pode ser criada pelas migrations:

```bash
dotnet ef database update
```

Também existe o script SQL em:

```text
Scripts/DeskFlowDb.sql
```

## Endpoints

| Método | Endpoint | Função |
|---|---|---|
| GET | `/api/categorias` | Listar categorias |
| GET | `/api/categorias/{id}` | Buscar categoria |
| POST | `/api/categorias` | Criar categoria |
| PUT | `/api/categorias/{id}` | Atualizar categoria |
| DELETE | `/api/categorias/{id}` | Excluir categoria |
| GET | `/api/chamados` | Listar e filtrar chamados |
| GET | `/api/chamados/{id}` | Detalhes completos do chamado |
| POST | `/api/chamados` | Abrir chamado |
| PATCH | `/api/chamados/{id}/iniciar` | Iniciar atendimento |
| PATCH | `/api/chamados/{id}/encerrar` | Encerrar chamado |
| POST | `/api/chamados/{id}/interacoes` | Adicionar interação |

### Exemplo: criar categoria

```json
{
  "nome": "Hardware"
}
```

### Exemplo: abrir chamado

```json
{
  "titulo": "Computador não liga",
  "descricao": "Equipamento não apresenta sinal.",
  "prioridade": "Alta",
  "solicitanteNome": "Joao",
  "categoriaId": 1
}
```

### Filtros

Os filtros podem ser combinados:

```text
GET /api/chamados?status=Aberto&prioridade=Alta&categoriaId=1
```

### Exemplo: interação

```json
{
  "autor": "Tecnico",
  "mensagem": "Iniciado diagnóstico do equipamento."
}
```

### Exemplo: encerrar chamado

```json
{
  "solucao": "Fonte substituída e equipamento testado."
}
```

## Regras principais

- Todo chamado novo começa como `Aberto`.
- Apenas chamado `Aberto` pode ir para `EmAndamento`.
- Apenas chamado `EmAndamento` pode ser `Fechado`.
- O encerramento exige uma solução.
- Chamados fechados não aceitam novas interações.
- Categorias com chamados associados não podem ser removidas.

## Tratamento de erros

O `ExceptionHandlingMiddleware` centraliza falhas e retorna respostas JSON sem expor stack trace.

Exemplo:

```json
{
  "status": 400,
  "erro": "Somente chamados com status Aberto podem ser iniciados."
}
```
