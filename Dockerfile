# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers projet et restaurer les dépendances
COPY *.csproj ./
RUN dotnet restore

# Copier tout le reste du code et compiler
COPY . ./
RUN dotnet publish -c Release -o out

# Étape 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copier les fichiers publiés depuis l'étape précédente
COPY --from=build /app/out ./

# Port exposé (modifie si ton app utilise un autre port)
EXPOSE 80

# Démarrer l'application
ENTRYPOINT ["dotnet", "SCPArchiveApi.dll"]