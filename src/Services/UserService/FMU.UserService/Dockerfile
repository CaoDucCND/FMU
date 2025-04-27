FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/UserService/FMU.UserService/FMU.UserService.csproj", "src/Services/UserService/FMU.UserService/"]
COPY ["src/BuildingBlocks/Common/FMU.Common/FMU.Common.csproj", "src/BuildingBlocks/Common/FMU.Common/"]
COPY ["src/BuildingBlocks/EventBus/FMU.EventBus/FMU.EventBus.csproj", "src/BuildingBlocks/EventBus/FMU.EventBus/"]
RUN dotnet restore "src/Services/UserService/FMU.UserService/FMU.UserService.csproj"
COPY . .
WORKDIR "/src/Services/UserService/FMU.UserService"
RUN dotnet build "FMU.UserService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FMU.UserService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FMU.UserService.dll"]