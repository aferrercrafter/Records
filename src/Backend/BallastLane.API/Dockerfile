#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BallastLane.API/BallastLane.API.csproj", "BallastLane.API/"]
COPY ["BallastLane.Application/BallastLane.Application.csproj", "BallastLane.Application/"]
COPY ["BallastLane.Domain/BallastLane.Domain.csproj", "BallastLane.Domain/"]
COPY ["BallastLane.Infrastructure/BallastLane.Infrastructure.csproj", "BallastLane.Infrastructure/"]
RUN dotnet restore "BallastLane.API/BallastLane.API.csproj"
COPY . .
WORKDIR "/src/BallastLane.API"
RUN dotnet build "BallastLane.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BallastLane.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BallastLane.API.dll"]