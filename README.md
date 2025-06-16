# 🧪 SCPArchiveAPI – Scraper & RESTful Service.
Projet d’API haute performance en C# (.NET 8) pour collecter, stocker, exposer et monitorer l’ensemble des documents de la [Fondation SCP](http://scp-wiki.wikidot.com).

## 📌 Objectifs.
- Scraper automatiquement les articles SCP, à intervalle régulier.

- Stocker efficacement les documents dans MongoDB.

- Exposer une API REST sécurisée, filtrable et documentée via Swagger.

- Fournir des métriques et visualisations sur le scraping (progression, erreurs, performance).

- Fournir une base scalable, maintenable, conteneurisée, prête pour la production.

## 🏗️ Architecture du projet.
```
SCPArchiveApi/
├── src/
│   └── SCPArchiveApi/
│       ├── Controllers/              # Endpoints REST (avec versioning)
│       ├── DTOs/                     # Objets de transfert (séparés des modèles DB)
│       ├── Middleware/               # Middleware (auth, logs, exceptions, rate limiting)
│       ├── Models/                   # Modèles de données (ScpEntry, Metadata)
│       ├── Repositories/             # Accès base de données (Mongo/PostgreSQL)
│       ├── Scraper/                  # Scraper SCP Wiki (html, jobs)
│       ├── Services/                 # Business logic (scraping, recherche…)
│       ├── Validators/               # FluentValidation / DataAnnotations personnalisés
│       ├── Program.cs                # Entry point .NET 8
│       ├── Startup.cs                # (Optionnel) Config avancée DI / middleware
│       ├── SCPArchiveApi.csproj
│       ├── appsettings.json          # Config générale (connexions, logs, etc.)
│       └── openapi.yaml              # Schéma OpenAPI (extrait automatiquement)
│
├── tests/
│   └── SCPArchiveApi.Tests/
│       ├── Unit/                     # Tests unitaires (services, logique, validateurs)
│       ├── Integration/              # Tests d’intégration (API, DB, scrapers)
│       ├── Performance/              # Tests de performance (API, DB, scrapers)
│       ├── E2E/                      # Tests de bout-en-bout (HttpClient, Postman/newman)
│       ├── SCPArchiveApi.Tests.csproj
│       └── testsettings.json         # Configuration de test
│
├── .github/
│   └── workflows/
│       ├── ci.yml                    # Build, test, lint, coverage
│       └── deploy.yml (optionnel)    # Déploiement auto (Docker Hub, Azure, etc.)
│
├── build/
│   ├── Makefile                      # Commandes raccourcies (build, test, lint…)
│   ├── build.ps1                     # Version PowerShell du Makefile
│   └── docker-entrypoint.sh          # Entrée personnalisée Docker
│
├── docs/                             # Documentation technique
│   ├── architecture.md               # Diagrammes / description du projet
│   ├── api.md                        # Explication des endpoints, pagination, filtres
│   ├── scraping.md                   # Stratégie de scraping & fréquence
│   ├── database-schema.md            # Modèle des collections / tables
│   ├── prometheus.md                 # Endpoints metrics, format, dashboard
│   └── deployment.md                 # Procédure d’hébergement (Docker, env)
│
├── .dockerignore
├── .env                              # Fichier de variable d’environnement (non commité)
├── .gitignore                        # Fichier détaillant les fichiers à ne pas commit.
├── CHANGELOG.md                      # Journal des changements (suivi sémantique)
├── Directory.Build.props             # Propriétés MSBuild partagées
├── docker-compose.yml                # API + Mongo + Prometheus + Grafana + DB tool
├── LICENSE                           # Licence libre (MIT, GPL…)
├── Dockerfile                        # Fichier de configuration de l'image docker
├── README.md                         # Présentation + installation + API rapide
```

## ⚙️ Stack technique.
| Composant            | Technologie choisie                  |
| -------------------- | ------------------------------------ |
| **Langage**          | C# (.NET 8)                          |
| **API REST**         | ASP.NET Core + Swashbuckle (Swagger) |
| **Scraping**         | HtmlAgilityPack + HttpClient         |
| **Scheduler**        | Quartz.NET                           |
| **Base de données**  | MongoDB (`MongoDB.Driver`)           |
| **Métriques**        | Prometheus-net (`/metrics`)          |
| **Logs**             | Serilog                              |
| **Visualisation**    | Grafana                              |
| **Conteneurisation** | Docker + Docker Compose              |
| **Tests**            | xUnit / NUnit                        |


## 🔍 Fonctionnalités clés.
- 📥 Scraper tous les articles SCP depuis le site officiel (contenu, ID, tags, metadata).

- 📆 Mise à jour périodique des contenus via scheduler (Quartz.NET).

- 🧾 Stockage structuré des données SCP dans MongoDB.

- 🌐 API RESTful exposant :

  - GET /scps — Liste paginée et filtrée des SCP.

  - GET /scps/{id} — Détail d’un SCP spécifique.

  - GET /search?q=xxx — Recherche plein texte.

- 🔐 Sécurité : Authentification JWT, Rate limiting, CORS, HTTPS.

- 📊 Métriques Prometheus : pages scrapées, durée, erreurs, etc.

- 📈 Dashboard Grafana : visualisation en temps réel de l’activité.

## 🚀 Démarrage rapide.
```bash
# 1. Cloner le repo
git clone https://github.com/ton-compte/scp-api.git
cd scp-api

# 2. Lancer le projet complet avec Docker
docker-compose up --build

# 3. Accès à l'API
http://localhost:5000/swagger

# 4. Accès à Grafana
http://localhost:3000 (admin / admin)
```

## 📄 Licence.
Projet open source sous licence MIT. Le contenu SCP est sous licence [CC BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/).
