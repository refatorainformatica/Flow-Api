# Architecture — Flow-Api

> Gerado automaticamente em 2026-08-24. Revise e refine.

## Estilo

- Clean Architecture / CQRS (.NET)

## Estrutura (topo do repositório)

```text
Flow-Api/
├── Apis/
│   ├── Application/
│   │   ├── Configuration/
│   │   ├── Controllers/
│   │   ├── Properties/
│   │   ├── Application.csproj
│   │   ├── Application.csproj.user
│   │   ├── appsettings.json
│   │   ├── appsettings.json.example
│   │   ├── Dockerfile
│   │   ├── Flow.http
│   │   ├── Program.cs
│   │   ├── SECRETS.md
│   │   ├── ServiceAccountKey.json
│   │   ├── ServiceAccountKey.json.example
│   │   ├── ServiceAccountKeyDev.json
│   │   └── ServiceAccountKeyDev.json.example
│   ├── Docker/
│   │   ├── docker-compose.dcproj
│   │   ├── docker-compose.yml
│   │   └── launchSettings.json
│   ├── Services/
│   │   ├── Features/
│   │   ├── Mappings/
│   │   ├── ServiceExtensions.cs
│   │   └── Services.csproj
│   ├── Shared/
│   │   ├── Domain/
│   │   ├── Infrastructure/
│   │   ├── ServiceRegistration.cs
│   │   └── Shared.csproj
│   └── README.md
├── banner.jpg
├── dotnet-tools.json
```

## Raiz de código principal

`Apis`

## Stack detectada

| Camada | Tecnologia |
|--------|------------|
| Runtime | .NET (Flow-Api.sln) |
| Harness | `.ai-harness/` |

## Relação harness ↔ código

| Spec feature | Código |
|--------------|--------|
| `Specification/features/<id>/` | módulo/pasta correspondente |

## Cross-cutting (preencher)

| Peça | Papel |
|------|--------|
| DI | ver README / código |
| Auth | ver README / código |
| Persistência | ver README / código |
| Observability | ver README / código |

## Fora de escopo arquitetural (atual)

- Consultar README.md e issues abertas
