# SistemaClinica

API REST para gerenciamento de uma clínica médica, desenvolvida em ASP.NET Core com .NET 10 e utilizado o design pattern CQRS.

O projeto foi construído com foco em organização por camadas e separação de responsabilidades, como autenticação JWT, controle de perfis, criptografia de senha, validação de dados, paginação, filtros, ordenação, persistencia com Entity Framework Core, versionamento da estrutura do banco com Migrations, Auditoria, Middleware global de exceções, serviço de relatório diário e testes de unidade.

## Visão geral

O SistemaClinica permite gerenciar pacientes, médicos, recepcionistas, especialidades e consultas. A aplicação possui autenticação e autorização por perfil de usuário, permitindo separar operações administrativas de operações realizadas por recepcionistas e médicos.

Também existe um serviço em segundo plano que gera diariamente um relatório CSV com as consultas do dia.

## Tecnologias utilizadas

- ASP.NET Core Web API
- .NET 10
- C#
- PostgreSQL
- Entity Framework Core 10
- MediatR
- FluentValidation
- Mapster
- JWT Bearer Authentication
- Swagger / OpenAPI
- Docker
- xUnit
- Moq

## Arquitetura

O projeto segue uma arquitetura em camadas, separando API, regras de aplicação, domínio, infraestrutura, bibliotecas auxiliares e testes.

```text
SistemaClinica
+-- SistemaClinica.API
|   +-- Controllers
|   +-- Extensions
|   +-- Middlewares
|   +-- Services
|   +-- Workers
|   +-- Relatorios
+-- SistemaClinica.Application
|   +-- UseCases
|   |   +-- Auth
|   |   +-- Consultas
|   |   +-- Especialidades
|   |   +-- Medicos
|   |   +-- Pacientes
|   |   +-- Recepcionistas
|   +-- Helpers
|   +-- Interfaces
|   +-- Middlewares
|   +-- Request
|   +-- Response
|   +-- Services
+-- SistemaClinica.Domain
|   +-- Entities
|   +-- Enums
+-- SistemaClinica.Infrastructure
|   +-- Configurations
|   +-- Context
|   +-- Migrations
|   +-- Repositories
|   +-- Security
+-- SistemaClinica.Libs
|   +-- Extensions
+-- SistemaClinica.Tests
|   +-- UseCases
|   +-- Helpers
```

## Como executar o projeto

1. Suba o PostgreSQL:

```bash
docker compose up -d
```

2. Aplique as migrations:

```bash
dotnet ef database update --project SistemaClinica.Infrastructure --startup-project SistemaClinica.API
```

3. Execute a API:

```bash
dotnet run --project SistemaClinica.API
```

4. Acesse o Swagger:

```text
https://localhost:7112/swagger
http://localhost:5277/swagger
```
