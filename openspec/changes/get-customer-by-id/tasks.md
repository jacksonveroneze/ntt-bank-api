# Tasks

Cada task é um PR (CONST-007). Teste cita o CA no nome (CONST-003). Ao fim de cada task: build, test e format verdes (comandos do AGENTS.md).

## 1. Domain

- [ ] 1.1 [Skill: create-value-object] Value object `Email`, namespace `NttBankApi.Domain.ValueObjects`. Campo: valor string; regras: trim, minúsculas, formato válido; erro `Email.Invalid`. Verificar: build verde.
- [ ] 1.2 [Skill: write-dotnet-tests] Testes do `Email`, classe alvo `Email`. Cobrem CA-12 e CA-13. Verificar: `dotnet test` verde.
- [ ] 1.3 [Skill: create-entity] Aggregate `Customer`, namespace `NttBankApi.Domain.Entities`. Campos: Id (int, gerado pelo banco), Name (string), Email (`Email`), CreatedAt (DateTimeOffset, recebido de `TimeProvider`), AccountType (enum Current, Nri, Savings, FixedDeposit, Salary), Status (enum Active, Blocked, Closed); enums em `NttBankApi.Domain.Enums`. Regras: trim em Name, 1 a 256 caracteres; erros `Customer.NameInvalid`. Verificar: build verde.
- [ ] 1.4 [Skill: write-dotnet-tests] Testes de `Customer`, classe alvo `Customer`. Cobrem CA-08, CA-09, CA-10, CA-11 e CA-14. Verificar: `dotnet test` verde.

## 2. Application: porta e repositório

- [ ] 2.1 [Skill: create-efcore-repository] Agregado `Customer`; porta `ICustomerRepository` em `NttBankApi.Application.Abstractions.Repositories`, implementação na Infrastructure; operação: obter por id (`GetByIdAsync`, NoTracking, sem escrita). Verificar: build verde.
- [ ] 2.2 [Manual] Mapeamento EF de `Customer` (tabela `customers`, id identity, enums como texto, Email convertido, name máx. 256), schema `bank` no `DefaultDbContext` e registro do repositório em `AddApplicationServices`. Justificativa: mapeamento EF e DI estão na lista fechada. Verificar: build verde.
- [ ] 2.3 [Manual] Migration inicial criando `bank.customers`, revisada. Justificativa: migration está na lista fechada. Verificar: script SQL gerado contém só `bank.customers`.

## 3. Use case e mapper

- [ ] 3.1 [Skill: create-read-use-case] GetById; agregado `Customer`; request com `Id` (int); response `CustomerResponse` (Id, Name, Email, CreatedAt, AccountType, Status); namespace `NttBankApi.Application.Features.Customers.GetCustomerById`; ausência devolve `DomainErrors.CustomerError.NotFound`; log só do id. Verificar: build verde.
- [ ] 3.2 [Skill: create-mapper] Mapster `IRegister`, `Customer` para `CustomerResponse`, namespace da feature acima. Verificar: build verde.
- [ ] 3.3 [Skill: write-dotnet-tests] Testes do use case, classe alvo `GetCustomerByIdUseCase`, repositório mockado. Cobrem CA-01, CA-02, CA-03 e, para o log sem dado pessoal, CA-15. Verificar: `dotnet test` verde.
- [ ] 3.4 [Manual] Registro do use case em `AddApplicationServices`. Justificativa: DI está na lista fechada. Verificar: build verde.

## 4. Api

- [ ] 4.1 [Skill: create-endpoint] `GET /customers/{id}`, use case `GetCustomerById`, policy `CustomerRead` (scope `customers.read`); id recebido como texto e validado como inteiro positivo (400); 404 para ausente; sem dado pessoal no erro. Verificar: build verde.
- [ ] 4.2 [Manual] `Program.cs`: chamar `AddDatabase`, `AddMapper` e mapear o endpoint de customers. Justificativa: Program.cs está na lista fechada. Verificar: `dotnet run` sobe na porta 7000 e `/customers/1` sem token responde 401.
- [ ] 4.3 [Skill: write-dotnet-tests] Testes do endpoint e do validator de id. Cobrem CA-04, CA-05, CA-06, CA-07 e CA-15 (corpo de erro sem dado pessoal). Verificar: `dotnet test` verde.

## 5. Fechamento

- [ ] 5.1 [Manual] Conferir que todo CA (CA-01 a CA-15) tem teste que o cita e rodar build, test e format. Justificativa: verificação de integração da change. Verificar: comandos verdes registrados.
