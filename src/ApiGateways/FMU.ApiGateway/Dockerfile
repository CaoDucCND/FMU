FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/FMU.ApiGateway/FMU.ApiGateway.csproj", "src/ApiGateways/FMU.ApiGateway/"]
RUN dotnet restore "src/ApiGateways/FMU.ApiGateway/FMU.ApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/FMU.ApiGateway"
RUN dotnet build "FMU.ApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FMU.ApiGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FMU.ApiGateway.dll"]