# 🧪 SCPArchiveAPI – Scraper & RESTful Service.
Projet d’API haute performance en C# (.NET 8) pour collecter, stocker, exposer et monitorer l’ensemble des documents de la [Fondation SCP](http://scp-wiki.wikidot.com).

## 📌 Objectifs.
- Scraper automatiquement les articles SCP, à intervalle régulier

- Stocker efficacement les documents dans MongoDB ou PostgreSQL

- Exposer une API REST sécurisée, filtrable et documentée via Swagger

- Fournir des métriques et visualisations sur le scraping (progression, erreurs, performance)

- Fournir une base scalable, maintenable, conteneurisée, prête pour la production

## 🏗️ Architecture du projet.
```
SCPApi/
├── SCPApi.WebApi/          # ASP.NET Core REST API (Swagger, Auth, Metrics)
├── SCPApi.Scheduler/       # Jobs Quartz.NET pour le scraping récurrent
├── SCPApi.Data/            # Abstraction DB MongoDB / PostgreSQL
├── SCPApi.Scraper/         # Scraper HtmlAgilityPack + HttpClient
├── SCPApi.Tests/           # Tests unitaires (xUnit / NUnit)
├── docker-compose.yml      # Stack complète (API + DB + Prometheus + Grafana)
├── Dockerfile              # Build de l’API
└── README.md
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
