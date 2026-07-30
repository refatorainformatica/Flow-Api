# Etapa base, onde o ASP.NET Core estará em execução
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5122
EXPOSE 7282

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Instalar pacotes necessários
RUN apt-get update && apt-get install -y \
    libicu-dev \
    ca-certificates \
    libssl-dev \
    curl && rm -rf /var/lib/apt/lists/*

# Copiar arquivos de projeto
COPY ["Apis/Application/Application.csproj", "Apis/Application/"]
COPY ["Apis/Shared/Shared.csproj", "Apis/Shared/"]
COPY ["Apis/Services/Services.csproj", "Apis/Services/"]


# Restaurar dependências
RUN dotnet restore "Apis/Application/Application.csproj"

# Copiar o restante do código-fonte
COPY . .

# Compilar a aplicação
WORKDIR "/src/Apis/Application"
RUN dotnet build "Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final, onde a aplicação será executada
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Criar usuário não-root
RUN adduser -u 1000 --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

# Copiar o certificado SSL para dentro do contêiner
COPY ["Apis/Docker/certificates/flowapi.pfx", "/app/certificates/flowapi.pfx"]

# Temporariamente use o usuário root para alterar a propriedade e permissões do arquivo
USER root

# Garantir que o usuário appuser tenha permissão para acessar o certificado
RUN chown appuser:appuser /app/certificates/flowapi.pfx
RUN chmod 644 /app/certificates/flowapi.pfx

# Voltar para o usuário não-root
USER appuser

# Configurar variáveis de ambiente para o Kestrel usar o certificado
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificates/flowapi.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=${CERT_PASSWORD:-REPLACE_ME}
ENV ASPNETCORE_URLS=https://+:7282;http://+:5122


# Definir a healthcheck para verificar a saúde do serviço
HEALTHCHECK --interval=30s --timeout=3s \
    CMD wget --no-verbose --tries=1 --spider http://localhost:5122/health || exit 1

# Definir o entrypoint da aplicação
ENTRYPOINT ["dotnet", "Application.dll"]
