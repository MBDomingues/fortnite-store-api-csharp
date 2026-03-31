# Estágio de Build: Usa o SDK 10 para restaurar e compilar
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["Fortinite-Project.Web.csproj", "./"]
RUN dotnet restore

# Copia o restante dos arquivos e publica
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Estágio de Runtime: Usa apenas o necessário para rodar o app (.NET 10)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS final
WORKDIR /app
EXPOSE 8080

# Configuração da pasta do SQLite
USER root
RUN mkdir -p /app/data && chown -R app:app /app/data # Muda o dono para o usuário 'app'

# Copia os arquivos compilados
COPY --from=build /app/publish .

# Define o usuário padrão (Agora o 'app' tem permissão na pasta 'data')
USER app

ENTRYPOINT ["dotnet", "Fortinite-Project.Web.dll"]