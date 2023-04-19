#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Calculator/Calculator.API/Calculator.API.csproj", "src/Services/Calculator/Calculator.API/"]
COPY ["src/Services/Calculator/Calculator.Application/Calculator.Application.csproj", "src/Services/Calculator/Calculator.Application/"]
COPY ["src/Services/Shared/Shared.Implementations/Shared.Implementations.csproj", "src/Services/Shared/Shared.Implementations/"]
COPY ["src/AuthManager/JwtAuthenticationManager/JwtAuthenticationManager.csproj", "src/AuthManager/JwtAuthenticationManager/"]
COPY ["src/Services/Shared/Shared.Domain/Shared.Domain.csproj", "src/Services/Shared/Shared.Domain/"]
COPY ["src/Services/Calculator/Calculator.Domain/Calculator.Domain.csproj", "src/Services/Calculator/Calculator.Domain/"]
COPY ["src/Services/Calculator/Calculator.Infrastructure/Calculator.Infrastructure.csproj", "src/Services/Calculator/Calculator.Infrastructure/"]
RUN dotnet restore "src/Services/Calculator/Calculator.API/Calculator.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Calculator/Calculator.API"
RUN dotnet build "Calculator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Calculator.API.csproj" -c Release -o /app/publish  

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Calculator.API.dll"]