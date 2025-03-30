# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DevConfessions/DevConfessions.csproj", "DevConfessions/"]
RUN dotnet restore "DevConfessions/DevConfessions.csproj"
COPY DevConfessions/ DevConfessions/
RUN dotnet publish "DevConfessions/" -c Release -o /app/publish

# Estágio de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Cria diretório para dados persistentes
RUN mkdir -p /app/wwwroot/data && \
    chmod 777 /app/wwwroot/data

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
VOLUME /app/wwwroot/data  # Garante persistência
ENTRYPOINT ["dotnet", "DevConfessions.dll"]
