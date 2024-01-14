#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8060

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Library.csproj", "Library/"]
RUN dotnet restore "Library/Library.csproj"

WORKDIR "/src/Library"
COPY . .
RUN dotnet build "Library.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Library.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Library.dll", "--urls=http://0.0.0.0:8060"]