ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

FROM ${DOTNET_RUNTIME} AS base
RUN apt-get update && \
    apt-get install wget -y
WORKDIR /app
EXPOSE 8080

FROM ${DOTNET_SDK} AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TechFood.Api/TechFood.Api.csproj", "TechFood.Api/"]
COPY ["src/TechFood.BackOffice.Application/TechFood.BackOffice.Application.csproj", "TechFood.BackOffice.Application/"]
COPY ["src/TechFood.BackOffice.Contracts/TechFood.BackOffice.Contracts.csproj", "TechFood.BackOffice.Contracts/"]
COPY ["src/TechFood.BackOffice.Domain/TechFood.BackOffice.Domain.csproj", "TechFood.BackOffice.Domain/"]
COPY ["src/TechFood.BackOffice.Infra/TechFood.BackOffice.Infra.csproj", "TechFood.BackOffice.Infra/"]
RUN dotnet restore "./TechFood.Api/TechFood.Api.csproj"
COPY /src .
WORKDIR TechFood.Api
RUN dotnet build "./TechFood.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechFood.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechFood.Api.dll"]
