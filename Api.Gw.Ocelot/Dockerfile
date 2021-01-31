FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Api.Gw.Ocelot/Api.Gw.Ocelot.csproj", "Api.Gw.Ocelot/"]
RUN dotnet restore "Api.Gw.Ocelot/Api.Gw.Ocelot.csproj"
COPY . .
WORKDIR "/src/Api.Gw.Ocelot"
RUN dotnet build "Api.Gw.Ocelot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api.Gw.Ocelot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.Gw.Ocelot.dll"]