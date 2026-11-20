# Design

## Context

Estado observado no repo (ver proposal.md para a motivação):

- Já existem: autenticação JWT, policy `CustomerRead` com o scope `customers.read` (`Infrastructure/Security`), `DefaultDbContext` (sem entidades), `AddDatabase`, `AddMapper`, `IUseCase<TRequest,TResponse>`, `DomainErrors.CustomerError.NotFound` e `AddApplicationServices` vazio.
- `DefaultDbContext` usa o schema placeholder `"schema"`; precisa virar `bank`.
- `Program.cs` não chama `AddDatabase`, `AddMapper` nem mapeia endpoints de negócio.
- Não existem entidade, value object, enum, repositório, use case, mapper, endpoint, validator nem migration.

## Goals / Non-Goals

**Goals:**
- Entregar `GET /customers/{id}` ponta a ponta, em fatias pequenas e revisáveis.

**Non-Goals:**
- Escrita, listagem, cache da consulta, paginação.

## Decisions

1. **Agregado `Customer` com criação validada.** Factory estática devolve `Result<Customer>`; sanitização e validação de name e email vivem no Domain. Base: CONST-002, CONV-020, CONV-021.
2. **`Email` como value object.** Trim + minúsculas + validação de formato na criação. Sem regex própria; se for necessária, segue CONV-086. Base: CONV-021.
3. **`createdAt` vem de `TimeProvider`.** A factory recebe o instante, nunca lê relógio global. Base: CONV-060.
4. **Id inteiro gerado pelo banco** (identity); o Domain não atribui id. Persistência em `bank.customers`: o schema `bank` é definido no `DefaultDbContext`, o mapeamento da tabela em configuração EF própria. Enums gravados como texto. Base: CONST-001, CONV-084.
5. **Validação do id no ponto de entrada.** O endpoint recebe o id como texto e um validator rejeita o que não for inteiro positivo (400) antes do use case. Alternativa descartada: constraint de rota `{id:int}`, que devolveria 404 em vez de 400. Base: CONV-029, CONV-038.
6. **Autorização no endpoint com a policy `CustomerRead`** (já existente), que exige JWT e scope `customers.read`: 401 sem token, 403 sem scope. Base: CONST-005, CONV-079. Posse (CONV-056) não se aplica: a permissão é por papel, não por dono do customer.
7. **Use case `GetCustomerById`** devolve `Result<CustomerResponse>`; ausência vira `DomainErrors.CustomerError.NotFound`. O endpoint traduz o `Result` por um único tradutor para 200/404 (CONV-022, CONV-036). Consulta com `NoTracking`, sem expor a entidade (CONV-070). `CancellationToken` ponta a ponta (CONV-044).
8. **Mapeamento entidade → Response com Mapster explícito** (`RequireExplicitMapping` já ativo). Enums serializados pelo nome.
9. **Registro de DI explícito**, sem scanning (CONV-042), em `AddApplicationServices`; `Program.cs` passa a chamar `AddDatabase`, `AddMapper` e o mapeamento da rota.
10. **Dado pessoal fora de log e de erro.** O use case loga só o id (CONST-004, CONV-054). Erros da API usam códigos estáveis, sem name/email (CONV-057).
11. **Geração de código só por skills** do `skills/net` (config.yaml). `[Manual]` apenas para a lista fechada: mapeamento EF, DI, migration, helpers/policy da Api e `Program.cs`.

## Risks / Trade-offs

- [Skill `create-entity` pode não gerar os enums] → se parar por pré-requisito, reportar; não criar fora de task `[Manual]` (apply guidance).
- [Enum como texto no banco vs inteiro] → texto é legível e estável a reordenação; custo de espaço irrelevante aqui.
- [Migration em ambiente real] → só gerada e revisada nesta change; aplicar fica para o fluxo de deploy do time.
- [Id ≤ 0 tratado como 400] → assumido como "formato inválido"; muda só se o time disser o contrário.
