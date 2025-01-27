FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["../Orange-Finance/OrangeFinance.csproj", "Orange-Finance/"]
COPY ["../OrangeFinance.Adapters/OrangeFinance.Adapters.csproj", "OrangeFinance.Adapters/"]
COPY ["../OrangeFinance.Application/OrangeFinance.Application.csproj", "OrangeFinance.Application/"]
COPY ["../OrangeFinance.Contracts/OrangeFinance.Contracts.csproj", "OrangeFinance.Contracts/"]
COPY ["../OrangeFinance.Domain/OrangeFinance.Domain.csproj", "OrangeFinance.Domain/"]
COPY ["../OrangeFinance.Infrastructure/OrangeFinance.Infrastructure.csproj", "OrangeFinance.Infrastructure/"]

RUN dotnet restore "./Orange-Finance/OrangeFinance.csproj"

COPY . .

WORKDIR "/src/Orange-Finance"
RUN dotnet build "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OrangeFinance.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OrangeFinance.dll"]
