FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/AuthService/FMU.AuthService/FMU.AuthService.csproj", "src/Services/AuthService/FMU.AuthService/"]
COPY ["src/BuildingBlocks/Common/FMU.Common/FMU.Common.csproj", "src/BuildingBlocks/Common/FMU.Common/"]
COPY ["src/BuildingBlocks/EventBus/FMU.EventBus/FMU.EventBus.csproj", "src/BuildingBlocks/EventBus/FMU.EventBus/"]
RUN dotnet restore "src/Services/AuthService/FMU.AuthService/FMU.AuthService.csproj"
COPY . .
WORKDIR "/src/Services/AuthService/FMU.AuthService"
RUN dotnet build "FMU.AuthService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FMU.AuthService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FMU.AuthService.dll"]