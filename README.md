# Desafio InMetrics


[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=coverage)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)

[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=InMetricChallenger_InMetrics&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=InMetricChallenger_InMetrics)

## Principais tecnologias e padrões utilizados: <a name="tecnologies"></a>
* [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
* [ASP.NET Core 6](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [CQRS](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* [FluentValidation](https://fluentvalidation.net/)
* [Mapster](https://github.com/MapsterMapper/Mapster)
* [MediatR](https://github.com/jbogard/MediatR)
* [Polly](https://github.com/App-vNext/Polly)
* [AspNetCoreRateLimit](https://github.com/stefanprodan/AspNetCoreRateLimit)
* [ProblemDetails](https://github.com/khellang/Middleware)
* [DistributedCache](https://github.com/dotnet/runtime)
* [Serilog](https://serilog.net/)
* [Swagger](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)
* [Throw](https://github.com/amantinband/throw)
* [UnitOfWork](https://www.devmedia.com.br/unit-of-work-o-padrao-de-unidade-de-trabalho-net/25811)
* [xUnit](https://github.com/xunit/xunit), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq)
* [Prometheus](https://prometheus.io/) 
* [Grafana](https://grafana.com/) 
* [Docker](https://www.docker.com/) 
* [DockerCompose](https://docs.docker.com/compose/) 

## Recursos: <a name="recursos"></a>
* <b>MediatR Pipeline Behaviors:</b> diversos pipeline behaviors centralizando a lógica para Caching, Logging, Polly Retry e Validation.
* <b>Logging:</b> log estruturado com Serilog e plugado ao Logging Behavior. O Logging Behavior permite que você configure via appsettings se vocês quer ligar/desligar individualmente os logs de request e response.
* <b>Polly Retry:</b> bahavior do Polly para retentativas em caso de alguma exception de banco de dados. Também pode ser configurável via appsettings, além de poder ser estendido para outros tipos de cenários.
* <b>Caching:</b> behavior de cache tanto em memória como distribuído com o Redis. O tipo do cache é configurável via appsettings.
* <b>CQRS:</b> padrão CQRS+MediatR com separação de bancos de dados de leitura e escrita e eventos de domínio para sincronização das bases de dados.
* <b>Documentação interativa:</b> o Swagger é utilizado para fornecer uma documentação interativa da API, facilitando a exploração e teste dos endpoints disponíveis.
* <b>Prometheus:</b> Sendo usado para monitoria da aplicação em tempo real
* <b>Grafana:</b> Exibindo de forma user friendly os dados capturados do Prometheus

## Diagrama da solução
![Diagram](./docs/diagram/diagram.png)

## Como rodar a aplicação: <a name="comorodar"></a>
> docker compose up

## Instruções e links: <a name="links"></a>

### links

* https://localhost:8080/swagger Swagger da aplicação
* https://localhost:8080/metrics Métricas da aplicação
* https://localhost:3000 Grafana, usuario admin, senha @admin
* https://localhost:9090 Prometheus, usuario admin, senha @admin

### Instruções de uso
Todos os endpoints estão bloqueados para uso anônimo (guest), então é necessário estar logado para que a aplicação funcione. Existe um endpoint somente para gerar o token, ele pode ser acessado da seguinte forma:
>curl -X 'POST' \
  'https://localhost:8080/api/v1/login' \
  -H 'accept: application/json' \
  -d ''


Para lançar um crédito, chame o endpoint /api/cashflow utilizando o post com o seguinte body
>{ "amount": 150, "transactionType": "Credit", "date": "2023-08-14T14:53:57.141Z" }

Para lançar um débito, chame o endpoint /api/cashflow utilizando o post com o seguinte body
>{ "amount": 150, "transactionType": "Debit", "date": "2023-08-14T14:53:57.141Z" }

Para consultar o saldo diário, chame o seguinte get:
>curl -X 'GET' 'http://localhost:8080/api/dailysummary/2023-08-14?api-version=1.0' -H 'accept: application/json'
