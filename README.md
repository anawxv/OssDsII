# OS DSII
> Web API para ordens de serviço de determinado produto

## Tecnologias utilizadas
- ASP .NET 7.0
- SQL Server


## como criar o projeto
```shell
$ dotnet new sln -o osdsii
$ dotnet new webapi -o OsDsII.api
$ dotnet sln add ./OsDsII.api/OsDsII.api.csproj
$ dotnet new xunit -o OsDsII.Tests
$ dotnet sln add ./OsDsII.Tests/OsDsII.Tests.csproj
$ dotnet add ./OsDsII.api/OsDsII.csproj reference ./OsDsII.Tests/OsDsII.Tests.csproj
```

## TODO
- Adicionar Xunit para testes unitários
- Adicionar InMemoryDatabase para testes de integração e e2e
- Adicionar workflow .net asp no github
- Adicionar Sonarqube para quality gates
- Adicionar padrões de projeto pós controllers criadas
- Adicionar EF para migrations e seeds
- Adicionar model mapper
- Documentar swagger openapi3.0