#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Orange-Finance/OrangeFinance.csproj", "Orange-Finance/"]
COPY ["OrangeFinance.Adapters/OrangeFinance.Adapters.csproj", "OrangeFinance.Adapters/"]
COPY ["OrangeFinance.Application/OrangeFinance.Application.csproj", "OrangeFinance.Application/"]
COPY ["OrangeFinance.Contracts/OrangeFinance.Contracts.csproj", "OrangeFinance.Contracts/"]
COPY ["OrangeFinance.Domain/OrangeFinance.Domain.csproj", "OrangeFinance.Domain/"]
COPY ["OrangeFinance.Infrastructure/OrangeFinance.Infrastructure.csproj", "OrangeFinance.Infrastructure/"]

RUN dotnet restore "./Orange-Finance/OrangeFinance.csproj"
COPY . .

WORKDIR "/src/Orange-Finance"
RUN dotnet build "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orange-Finance.dll"]