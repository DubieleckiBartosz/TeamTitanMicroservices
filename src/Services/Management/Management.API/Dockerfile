#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Management/Management.API/Management.API.csproj", "src/Services/Management/Management.API/"]
COPY ["src/Services/Management/Management.Application/Management.Application.csproj", "src/Services/Management/Management.Application/"]
COPY ["src/Services/Shared/Shared.Implementations/Shared.Implementations.csproj", "src/Services/Shared/Shared.Implementations/"]
COPY ["src/AuthManager/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "src/AuthManager/JwtAuthenticationManager/"]
COPY ["src/Services/Shared/Shared.Domain/Shared.Domain.csproj", "src/Services/Shared/Shared.Domain/"]
COPY ["src/Services/Management/Management.Domain/Management.Domain.csproj", "src/Services/Management/Management.Domain/"]
COPY ["src/Services/Management/Management.Infrastructure/Management.Infrastructure.csproj", "src/Services/Management/Management.Infrastructure/"]
RUN dotnet restore "src/Services/Management/Management.API/Management.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Management/Management.API"
RUN dotnet build "Management.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Management.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Management.API.dll"]