## Contexto
<!-- Por que esta mudança existe. Card e change (openspec/changes/<nome>/) de origem. -->

## O que muda
<!-- Lista curta, uma linha por mudança. -->
-

## Como testar
<!-- Comandos executados e resultado real. Não afirme o que não rodou; escreva "não informados". -->
```
dotnet build src/NttBank.Api.slnx --no-restore
dotnet test src/NttBank.Api.slnx --no-build
dotnet format src/NttBank.Api.slnx --verify-no-changes --no-restore
```

## Link de origem
<!-- Link do card do Trello e, se houver, openspec/changes/<nome>/ e a task. -->

## Checklist
- [ ] Título no padrão Conventional Commits: `<tipo>(<escopo>): <descrição no imperativo>`, até 72 caracteres
- [ ] Task pronta conforme a definição de pronto (CONST-009), com cada CA coberto por teste passando (CONST-003)
- [ ] Sem segredo, token ou dado pessoal no diff nem nos logs (CONST-004)
- [ ] Mudança de comportamento protegida por feature flag (CONST-006), ou justificativa registrada
- [ ] Apenas os arquivos da task; PR dentro do limite de tamanho (CONST-007)
- [ ] Revisão humana solicitada; merge feito por humano (CONST-008)
