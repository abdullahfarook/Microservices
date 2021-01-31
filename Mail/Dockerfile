FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Mail/Mail.csproj", "Mail/"]
COPY ["Microservices.Core/Microservices.Core.csproj", "Microservices.Core/"]
COPY ["EventBus.AzureServiceBus/EventBus.AzureServiceBus.csproj", "EventBus.AzureServiceBus/"]
COPY ["EventBus.Common/EventBus.Common.csproj", "EventBus.Common/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
RUN dotnet restore "Mail/Mail.csproj"
COPY . .
WORKDIR "/src/Mail"
RUN dotnet build "Mail.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Mail.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mail.dll"]