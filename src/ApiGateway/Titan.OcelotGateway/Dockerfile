#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ApiGateway/Titan.OcelotGateway/Titan.OcelotGateway.csproj", "src/ApiGateway/Titan.OcelotGateway/"]
COPY ["src/AuthManager/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "src/AuthManager/JwtAuthenticationManager/"]
RUN dotnet restore "src/ApiGateway/Titan.OcelotGateway/Titan.OcelotGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway/Titan.OcelotGateway"
RUN dotnet build "Titan.OcelotGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Titan.OcelotGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Titan.OcelotGateway.dll"]