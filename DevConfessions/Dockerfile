# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DevConfessions.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Pasta persistente (com permissões)
RUN mkdir -p /persistent/data && chmod 777 /persistent/data

# Copia apenas os bins necessários
COPY --from=build /app/publish .

# Configurações para o Render (gratuito)
ENV ASPNETCORE_URLS=http://+:8080
ENV JSON_STORAGE_PATH=/persistent/data/confessions.json
EXPOSE 8080

# Volume para persistência (Render Free tem /persistent)
VOLUME /persistent

ENTRYPOINT ["dotnet", "DevConfessions.dll"]
