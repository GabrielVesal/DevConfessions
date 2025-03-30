FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Cria diretório para dados persistentes
RUN mkdir -p /app/data && \
    chmod 777 /app/data  # Garante permissão de escrita

COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Define o volume (opcional no Dockerfile, mas documenta a intenção)
VOLUME /app/data

ENTRYPOINT ["dotnet", "DevConfessions.dll"]
