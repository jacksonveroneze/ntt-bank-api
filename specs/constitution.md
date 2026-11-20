# Constitution do repositório

Regras fixas deste repositório. Toda change do OpenSpec (proposal, specs, design e tasks) herda estas regras; nenhuma pode contrariar uma delas. Regra mudou: altere este arquivo por PR próprio, com revisão humana, antes de qualquer change que dependa da mudança. Até lá, vale o texto atual.

Aqui ficam só regras de produto, arquitetura e processo. Regra de código vive em `dotnet-conventions.md` (`CONV-nnn`); uma regra não se repete em dois arquivos: cite o ID.

Valores marcados `a definir` devem ser preenchidos pelo time antes do primeiro uso pelos agentes.

## Como citar (agentes de especificação, de código e de revisão)

- Cite pelo identificador `CONST-nnn`; nunca copie o texto da regra.
- CA (critério de aceite) é um cenário Dado/Quando/Então de um requisito da spec.
- Na proposal e nas specs, cite só o que for regra de negócio ou de produto (ex.: CONST-004 dados pessoais; CONST-006 rollout), sem tecnologia.
- No `design.md`, cada decisão técnica que decorre de uma regra traz "Base: CONST-nnn". Cada teste planejado por CA cita CONST-003.
- No `tasks.md`, cada task que aplica uma regra cita o identificador; o limite de tamanho (CONST-007) decide o fatiamento.
- Conflito entre o pedido e uma regra: não resolva sozinho. Registre como item `a definir` com a pergunta e pare para o humano decidir (a regra vence até a decisão).
- Identificadores são estáveis: não renumere nem reaproveite. Regra aposentada fica marcada como `(aposentada)`.

## Arquitetura

- **CONST-001** Clean Architecture: `Domain` não depende de nada; `Application` depende só de `Domain`; `Infrastructure` e a camada de entrada (API, worker) dependem de `Application`. A dependência aponta sempre para dentro.
- **CONST-002** Regra de negócio vive em `Domain`/`Application`, nunca no ponto de entrada. Falha esperada e demais regras de código: `CONV-nnn` (dotnet-conventions).

## Testes

- **CONST-003** Todo critério de aceite (CA) tem ao menos um teste automatizado que o cobre, e o teste cita o CA no nome ou em comentário. Task sem teste passando não está pronta. Stack e nomes: CONV-048 a CONV-053.

## Segurança e dados pessoais

- **CONST-004** Dado pessoal (CPF, e-mail, telefone, endereço, dados financeiros) nunca aparece em log (CONV-054), mensagem de erro, comentário, fixture de teste real nem exemplo de spec. Use dado sintético. Coleta nova de dado pessoal exige base legal registrada na spec (item `a definir` até o humano confirmar). Retenção: a definir.
- **CONST-005** Segredo fora do repositório, entrada externa validada e todo endpoint com controle de acesso explícito são invariantes. A regra de código está em CONV-055, CONV-038 e CONV-079.

## Rollout

- **CONST-006** Mudança de comportamento visível a usuário ou a sistema integrado entra atrás de feature flag, desligada por padrão, com plano de ativação e de reversão no `design.md`. Mudança puramente interna (refatoração sem efeito observável) dispensa flag, com a justificativa registrada. Biblioteca de flags: a definir.

## Tamanho de PR

- **CONST-007** Um PR por task do `tasks.md`. Limite orientador: até 400 linhas de diff, até 10 arquivos, sem misturar refatoração e mudança de comportamento. Acima disso, a task é dividida no `tasks.md` antes de codificar. (valores a definir pelo time)
- **CONST-008** Merge sempre humano, após revisão humana. Nenhum agente faz merge, push em `main`/`develop` ou force-push.

## Definição de pronto

- **CONST-009** Uma task está pronta somente quando: build verde; testes verdes, incluindo os dos CAs da task; `dotnet format --verify-no-changes` sem diferença; nenhum aviso novo; arquivos alterados iguais aos previstos na task; PR descrito com os comandos realmente executados; CI verde; revisão humana aprovada.
- **CONST-010** Uma change está pronta para codificar somente quando: todo requisito tem ao menos um CA; todo CA tem teste e task; nenhum item `a definir` ou `(proposto, confirmar)`; proposal, specs, design e tasks aprovados por humano; PR dos artefatos mergeado. Ao fim da implementação, o `verify` não pode ter pendências.