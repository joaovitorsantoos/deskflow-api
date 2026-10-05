IF DB_ID(N'DeskFlowDb') IS NULL
BEGIN
    CREATE DATABASE [DeskFlowDb];
END;
GO

USE [DeskFlowDb];
GO

IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

IF OBJECT_ID(N'[Categorias]') IS NULL
BEGIN
    CREATE TABLE [Categorias] (
        [Id] int NOT NULL IDENTITY,
        [Nome] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
    );
END;
GO

IF OBJECT_ID(N'[Chamados]') IS NULL
BEGIN
    CREATE TABLE [Chamados] (
        [Id] int NOT NULL IDENTITY,
        [Titulo] nvarchar(max) NOT NULL,
        [Descricao] nvarchar(max) NOT NULL,
        [Prioridade] int NOT NULL,
        [Status] int NOT NULL,
        [SolicitanteNome] nvarchar(max) NOT NULL,
        [DataAbertura] datetime2 NOT NULL,
        [DataFechamento] datetime2 NULL,
        [Solucao] nvarchar(max) NULL,
        [CategoriaId] int NOT NULL,
        CONSTRAINT [PK_Chamados] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Chamados_Categorias_CategoriaId]
            FOREIGN KEY ([CategoriaId]) REFERENCES [Categorias] ([Id]) ON DELETE NO ACTION
    );

    CREATE INDEX [IX_Chamados_CategoriaId] ON [Chamados] ([CategoriaId]);
END;
GO

IF OBJECT_ID(N'[Interacoes]') IS NULL
BEGIN
    CREATE TABLE [Interacoes] (
        [Id] int NOT NULL IDENTITY,
        [ChamadoId] int NOT NULL,
        [Autor] nvarchar(max) NOT NULL,
        [Mensagem] nvarchar(max) NOT NULL,
        [DataRegistro] datetime2 NOT NULL,
        CONSTRAINT [PK_Interacoes] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Interacoes_Chamados_ChamadoId]
            FOREIGN KEY ([ChamadoId]) REFERENCES [Chamados] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_Interacoes_ChamadoId] ON [Interacoes] ([ChamadoId]);
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20261002233555_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20261002233555_InitialCreate', N'10.0.12');
END;
GO
