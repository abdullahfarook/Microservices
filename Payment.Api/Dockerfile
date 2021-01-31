FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Payment.Api/Payment.Api.csproj", "Payment.Api/"]
COPY ["Payment.Api/nuget.config", "Payment.Api/"]
COPY ["Microservices.Core/Microservices.Core.csproj", "Microservices.Core/"]
COPY ["EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "EventBus.AzureServiceBus/"]
COPY ["EventBus.Common/EventBus.Common.csproj", "EventBus.Common/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
RUN dotnet restore "Payment.Api/Payment.Api.csproj"
COPY . .
WORKDIR "/src/Payment.Api"
RUN dotnet build "Payment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Payment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.Api.dll"]