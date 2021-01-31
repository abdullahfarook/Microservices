FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["PowerBi.Api/PowerBi.Api.csproj", "PowerBi.Api/"]
COPY ["Microservices.Core/Microservices.Core.csproj", "Microservices.Core/"]
COPY ["EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "EventBus.AzureServiceBus/"]
COPY ["EventBus.Common/EventBus.Common.csproj", "EventBus.Common/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
RUN dotnet restore "PowerBi.Api/PowerBi.Api.csproj"
COPY . .
WORKDIR "/src/PowerBi.Api"
RUN dotnet build "PowerBi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PowerBi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PowerBi.Api.dll"]