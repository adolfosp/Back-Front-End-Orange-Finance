# Etapa base (imagem do runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar arquivos de projeto
COPY ["Orange-Finance/OrangeFinance.csproj", "Orange-Finance/"]
COPY ["OrangeFinance.Adapters/OrangeFinance.Adapters.csproj", "OrangeFinance.Adapters/"]
COPY ["OrangeFinance.Application/OrangeFinance.Application.csproj", "OrangeFinance.Application/"]
COPY ["OrangeFinance.Contracts/OrangeFinance.Contracts.csproj", "OrangeFinance.Contracts/"]
COPY ["OrangeFinance.Domain/OrangeFinance.Domain.csproj", "OrangeFinance.Domain/"]
COPY ["OrangeFinance.Infrastructure/OrangeFinance.Infrastructure.csproj", "OrangeFinance.Infrastructure/"]

# Restaurar dependências
RUN dotnet restore "./Orange-Finance/OrangeFinance.csproj"

# Copiar todo o código para a imagem
COPY . .

# Construir o aplicativo
WORKDIR "/src/Orange-Finance"
RUN dotnet build "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etapa de publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app

# Copiar os arquivos publicados
COPY --from=publish /app/publish .

# Configurar o usuário no final (opcional)
USER app

# Definir o ponto de entrada
ENTRYPOINT ["dotnet", "Orange-Finance.dll"]
