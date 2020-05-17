# VehicleApi
## Framework
- NetCore 3.1

## Como iniciar
 - Rodar o scripts Scripts/init-db.sql em um banco MySql
 - Buildar a Solution
 - Iniciar a Vehicle.API

## Arquitetura
Utiliza uma estrutura baseada em CQRS:
- Queries: Consultas utilizando o dapper para queries mais limpas
- Mutations: Mudanças de estado ulitizando o mapeamento do entity framework

A aplicação possui 4 projetos
- Api: Interface para consumidores externos com padrão RESTFULL
- Domain: Classes de manipulação com regra de negócio
- UnitTests: Testes das mutations utilizando apenas o domain com o banco em memória
- IntegrationTests: Testes das queries utilizando a api e conexão com um banco de testes
