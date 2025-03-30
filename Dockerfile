# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o .csproj primeiro (ajuste o caminho para a subpasta)
COPY ["DevConfessions/DevConfessions.csproj", "DevConfessions/"]
RUN dotnet restore "DevConfessions/DevConfessions.csproj"

# Copia o resto dos arquivos
COPY DevConfessions/ DevConfessions/
RUN dotnet publish "DevConfessions/" -c Release -o /app/publish

# Estágio de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configurações essenciais
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "DevConfessions.dll"]
