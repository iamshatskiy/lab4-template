#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8050

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Rating.csproj", "Rating/"]
RUN dotnet restore "Rating/Rating.csproj"

WORKDIR "/src/Rating"
COPY . .
RUN dotnet build "Rating.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Rating.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rating.dll", "--urls=http://0.0.0.0:8050"]