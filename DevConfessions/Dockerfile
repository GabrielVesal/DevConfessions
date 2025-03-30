# Estágio 1: Compilação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto e restaura as dependências
COPY *.csproj .
RUN dotnet restore

# Copia todo o código e compila
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio 2: Execução (imagem final otimizada)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copia os binários publicados
COPY --from=build /app/publish .

# Define a porta que o container usará
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Comando de inicialização
ENTRYPOINT ["dotnet", "DevConfessions.dll"]
