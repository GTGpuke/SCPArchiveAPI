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
│   └── SCPArchiveApi/                 # Code principal de l’API
│       ├── Controllers/               # Endpoints REST
│       ├── Models/                    # Modèles de données (ScpEntry, Metadata)
│       ├── Services/                  # Scraping, business logic
│       ├── Repositories/             # Accès base de données (Mongo/PostgreSQL)
│       ├── Scraper/                  # Scraper SCP Wiki (html, jobs)
│       ├── Program.cs                # Entry point + configuration
│       ├── Startup.cs (optionnel)    # Configuration avancée DI / middleware
│       ├── appsettings.json          # Config générale (connexions, logs)
│       ├── Dockerfile                # Image Docker de l’API
│       └── SCPArchiveApi.csproj
│
├── tests/
│   └── SCPArchiveApi.Tests/          # Projet de tests (XUnit, NUnit…)
│       ├── Unit/                     # Tests unitaires (services, logique)
│       ├── Integration/              # Tests d’intégration (API, DB)
│       └── SCPArchiveApi.Tests.csproj
│
├── .github/                           # CI pour GitHub
│   └── workflows/
│       └── ci.yml                     # Pipeline CI/CD GitHub Actions
├── .gitignore                         # Fichiers à exclure du repo
├── .dockerignore                      # Fichiers à exclure des builds Docker
├── docker-compose.yml                 # Multi-services : API + MongoDB + Prometheus
├── LICENSE                            # (Optionnel) Licence libre
├── README.md                          # Documentation du projet
├── Directory.Build.props              # (Optionnel) Props globaux pour tout le projet
```

## ⚙️ Stack technique.
| Composant         | Tech choisie                 |
| ----------------- | ---------------------------- |
| **Langage**       | C# (.NET 8)                  |
| **API REST**      | ASP.NET Core + Swashbuckle   |
| **Scraping**      | HtmlAgilityPack + HttpClient |
| **Scheduler**     | Quartz.NET                   |
| **DB**            | MongoDB (`MongoDB.Driver`)   |
| **Metrics**       | Prometheus-net (`/metrics`)  |
| **Logs**          | Serilog                      |
| **Visualisation** | Grafana                      |
| **Conteneurs**    | Docker / Docker Compose      |
| **Tests**         | xUnit / NUnit                |

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
