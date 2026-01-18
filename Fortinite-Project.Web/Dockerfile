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

# Configuração da pasta do SQLite como Root
USER root
RUN mkdir -p /app/data && chmod 777 /app/data

# Copia os arquivos compilados do estágio de build
COPY --from=build /app/publish .

# Define o usuário padrão do .NET (usuário não-root por segurança)
USER app

# O nome da DLL deve ser idêntico ao do seu .csproj
ENTRYPOINT ["dotnet", "Fortinite-Project.Web.dll"]