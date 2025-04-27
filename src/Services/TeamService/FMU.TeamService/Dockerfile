FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/TeamService/FMU.TeamService/FMU.TeamService.csproj", "src/Services/TeamService/FMU.TeamService/"]
COPY ["src/BuildingBlocks/Common/FMU.Common/FMU.Common.csproj", "src/BuildingBlocks/Common/FMU.Common/"]
COPY ["src/BuildingBlocks/EventBus/FMU.EventBus/FMU.EventBus.csproj", "src/BuildingBlocks/EventBus/FMU.EventBus/"]
RUN dotnet restore "src/Services/TeamService/FMU.TeamService/FMU.TeamService.csproj"
COPY . .
WORKDIR "/src/Services/TeamService/FMU.TeamService"
RUN dotnet build "FMU.TeamService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FMU.TeamService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FMU.TeamService.dll"]