# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura dependências
COPY ["DevConfessions.csproj", "."]
RUN dotnet restore

# Copia todo o código e publica
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Cria pasta para dados persistentes
RUN mkdir -p /app/data && chmod 777 /app/data

# Copia os arquivos publicados
COPY --from=build /app/publish .

# Configurações essenciais
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
VOLUME /app/data  # Garante persistência

ENTRYPOINT ["dotnet", "DevConfessions.dll"]
