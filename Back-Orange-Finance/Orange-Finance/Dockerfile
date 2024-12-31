FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Vamos usar /src como diretório de trabalho dentro do container
WORKDIR /src

# Copia somente o .csproj primeiro
COPY ["OrangeFinance.csproj", "."]
RUN dotnet restore "OrangeFinance.csproj"

# Copia todo o restante do código
COPY . .
RUN dotnet build "OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orange-Finance.dll"]
