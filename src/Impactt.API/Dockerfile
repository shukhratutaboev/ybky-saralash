FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Impactt.API/Impactt.API.csproj", "src/Impactt.API/"]
RUN dotnet restore "src/Impactt.API/Impactt.API.csproj"
COPY . .
WORKDIR "/src/src/Impactt.API"
RUN dotnet build "Impactt.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Impactt.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Impactt.API.dll"]
