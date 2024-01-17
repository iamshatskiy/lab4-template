#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["LibrarySystem.csproj", "LibrarySystem/"]
RUN dotnet restore "LibrarySystem/LibrarySystem.csproj"

WORKDIR "/src/LibrarySystem"
COPY . .
RUN dotnet build "LibrarySystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibrarySystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibrarySystem.dll", "--urls=http://0.0.0.0:8080"]