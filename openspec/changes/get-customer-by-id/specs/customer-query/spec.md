# Spec Delta

## Purpose

Permite que consumidores autorizados consultem um customer pelo identificador, com os dados validados na criação e erros esperados bem definidos.

## ADDED Requirements

### Requirement: Consultar customer por id
O sistema SHALL retornar, para um id existente, todos os dados do customer (id, name, email, createdAt, accountType, status), independentemente do status.

#### Scenario: CA-01 Customer ativo existente
- **GIVEN** um customer Active com id 1, name "Maria Teste" e email "maria.teste@example.com"
- **WHEN** um consumidor autorizado consulta o customer de id 1
- **THEN** a resposta é 200 com id, name, email, createdAt, accountType e status do customer

#### Scenario: CA-02 Customer bloqueado ou encerrado
- **GIVEN** um customer Blocked e um customer Closed
- **WHEN** um consumidor autorizado consulta cada um pelo id
- **THEN** cada resposta é 200 com o status correspondente

### Requirement: Id inexistente
O sistema SHALL responder 404 quando não existir customer com o id informado.

#### Scenario: CA-03 Id não cadastrado
- **GIVEN** que não existe customer com id 999
- **WHEN** um consumidor autorizado consulta o id 999
- **THEN** a resposta é 404

### Requirement: Id com formato inválido
O sistema SHALL responder 400, sem consultar a base, quando o id não for um inteiro positivo válido.

#### Scenario: CA-04 Id não numérico
- **WHEN** um consumidor autorizado consulta o id "abc"
- **THEN** a resposta é 400

#### Scenario: CA-05 Id zero, negativo ou fora do intervalo
- **WHEN** um consumidor autorizado consulta o id 0, -1 ou 99999999999
- **THEN** a resposta é 400

### Requirement: Controle de acesso à consulta
O sistema SHALL permitir a consulta somente a requisições autenticadas por JWT válido e com a permissão de leitura de customers.

#### Scenario: CA-06 Sem autenticação
- **WHEN** a consulta chega sem token válido
- **THEN** a resposta é 401 e nenhum dado de customer é retornado

#### Scenario: CA-07 Sem permissão de leitura
- **GIVEN** um token válido sem a permissão de leitura de customers
- **WHEN** a consulta é feita
- **THEN** a resposta é 403 e nenhum dado de customer é retornado

### Requirement: Nome do customer
O sistema SHALL, na criação do customer, remover espaços nas extremidades do name e rejeitar name vazio ou com mais de 256 caracteres.

#### Scenario: CA-08 Name com espaços nas extremidades
- **WHEN** um customer é criado com name "  Maria Teste  "
- **THEN** o name do customer é "Maria Teste"

#### Scenario: CA-09 Name vazio
- **WHEN** um customer é criado com name vazio ou só com espaços
- **THEN** a criação falha com erro de name inválido

#### Scenario: CA-10 Name acima do limite
- **WHEN** um customer é criado com name de 257 caracteres
- **THEN** a criação falha com erro de name inválido

#### Scenario: CA-11 Name no limite
- **WHEN** um customer é criado com name de 256 caracteres
- **THEN** a criação é aceita

### Requirement: E-mail do customer
O sistema SHALL, na criação do customer, remover espaços nas extremidades do email, convertê-lo para minúsculas e rejeitar e-mail inválido.

#### Scenario: CA-12 E-mail normalizado
- **WHEN** um customer é criado com email "  Maria.Teste@Example.com "
- **THEN** o email do customer é "maria.teste@example.com"

#### Scenario: CA-13 E-mail inválido
- **WHEN** um customer é criado com email "maria.teste" ou vazio
- **THEN** a criação falha com erro de email inválido

### Requirement: Data de criação do customer
O sistema SHALL definir createdAt no momento da criação do customer, em UTC.

#### Scenario: CA-14 createdAt definido na criação
- **GIVEN** que o relógio marca um instante conhecido
- **WHEN** um customer é criado
- **THEN** o createdAt do customer é esse instante

### Requirement: Proteção de dados pessoais
O sistema SHALL NOT registrar name nem email de customer em log nem em mensagem de erro (CONST-004).

#### Scenario: CA-15 Erro sem dado pessoal
- **WHEN** a consulta resulta em 400 ou 404
- **THEN** o corpo da resposta e os logs não contêm name nem email de customer
