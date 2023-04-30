#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/General/General.API/General.API.csproj", "src/Services/General/General.API/"]
COPY ["src/Services/General/General.Application/General.Application.csproj", "src/Services/General/General.Application/"]
COPY ["src/Services/Shared/Shared.Implementations/Shared.Implementations.csproj", "src/Services/Shared/Shared.Implementations/"]
COPY ["src/AuthManager/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "src/AuthManager/JwtAuthenticationManager/"]
COPY ["src/Services/Shared/Shared.Domain/Shared.Domain.csproj", "src/Services/Shared/Shared.Domain/"]
COPY ["src/Services/General/General.Domain/General.Domain.csproj", "src/Services/General/General.Domain/"]
COPY ["src/Services/General/General.Infrastructure/General.Infrastructure.csproj", "src/Services/General/General.Infrastructure/"]
RUN dotnet restore "src/Services/General/General.API/General.API.csproj"
COPY . .
WORKDIR "/src/src/Services/General/General.API"
RUN dotnet build "General.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "General.API.csproj" -c Release -o /app/publish  

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "General.API.dll"]