# Proposal

## Why

A API ainda não expõe nenhum dado de customer. Consumidores autorizados precisam consultar um customer pelo identificador. Esta é a primeira capacidade do domínio de customers e cria a base técnica (entidade, persistência, use case e endpoint) que as próximas changes reaproveitam.

## What Changes

- Novo `GET /customers/{id}` readonly, que retorna todos os dados do customer: id, name, email, createdAt, accountType e status.
- Id inexistente responde 404; id com formato inválido responde 400.
- Acesso restrito a quem tem a permissão de leitura de customers (CONST-005); sem token, 401; sem permissão, 403.
- O customer é retornado em qualquer status (Active, Blocked ou Closed).
- Regras dos dados aplicadas na criação da entidade: name e email sanitizados e validados, createdAt definido na criação.
- Base técnica: entidade, value object de e-mail, persistência em `bank.customers`, use case, mapeamento e endpoint.
- Base legal de name e email: execução de contrato (CONST-004), confirmada pelo humano. Retenção segue `a definir` no nível do produto e não bloqueia esta change, que não grava nem apaga dados.

## Capabilities

### New Capabilities
- `customer-query`: consulta de um customer pelo identificador, com regras de dados, erros esperados e controle de acesso.

### Modified Capabilities

Nenhuma.

## Fora de escopo

- Listagem paginada de customers (outra change).
- Qualquer operação de escrita: criar, alterar ou excluir.
- Endpoint de cadastro; a criação existe só na entidade.
- Política de retenção de dados pessoais.

## Impact

- Código novo em `Domain`, `Application`, `Infrastructure` e `Api`; testes em `UnitTests`.
- Banco: schema `bank`, tabela `customers`, com migration.
- Sem novos pacotes previstos (CONV-087); se algum for necessário, pede confirmação antes.
- Dado pessoal (name, email) só trafega na resposta; nunca em log (CONST-004).
