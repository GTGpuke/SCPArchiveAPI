# 🏛️ Architecture détaillée de SCPArchiveApi

## ⚡️ Vue d'ensemble

L'API SCPArchive est conçue selon une architecture en couches, inspirée des principes **Clean Architecture** et **DDD** (Domain-Driven Design). Chaque dossier du projet a une responsabilité claire, ce qui facilite la maintenance, les tests et l'évolutivité.

---

## 🛠️ Stack technique

| Technologie         | Rôle principal                |
|--------------------|------------------------------|
| .NET 8             | WebAPI, logique métier        |
| MongoDB            | Stockage NoSQL principal      |
| Docker             | Containerisation              |
| Prometheus/Grafana | Monitoring & dashboards       |
| Quartz.NET         | Jobs planifiés (scraping)     |
| FluentValidation   | Validation des entrées        |
| Swagger/OpenAPI    | Documentation interactive     |
| AutoMapper         | Mapping DTOs <-> modèles      |

---

## 📁 Structure des dossiers

| Dossier         | Rôle / Contenu principal                                                                 |
|-----------------|----------------------------------------------------------------------------------------|
| `Controllers/`  | Endpoints REST, versioning, validation, documentation OpenAPI                          |
| `DTOs/`         | Objets de transfert API (entrée/sortie), découplés des modèles DB                      |
| `Middleware/`   | Auth, logs, gestion globale des exceptions, rate limiting                              |
| `Models/`       | Modèles de données métier (ScpEntry, Metadata, etc.)                                   |
| `Repositories/` | Accès à la base MongoDB, requêtes, index, abstraction du stockage                      |
| `Scraper/`      | Extraction HTML, parsing, jobs Quartz pour la synchronisation avec le wiki SCP         |
| `Services/`     | Logique métier, orchestration, recherche, mapping, gestion du scraping                 |
| `Validators/`   | Règles de validation personnalisées (FluentValidation, DataAnnotations)                |

---

## 🔄 Cycle de vie d'une requête

```mermaid
graph TD
    Client[Client HTTP]
    MW[Middleware (auth, logs, erreurs, rate limit)]
    Controller[Controller API]
    Service[Service métier]
    Repo[Repository MongoDB]
    DB[(MongoDB)]
    
    Client --> MW --> Controller --> Service --> Repo --> DB
    Service -->|Mapping| Controller
    Controller -->|DTO| Client
```

1. **Entrée** : Un client envoie une requête HTTP (ex: `GET /api/v1/scps/SCP-173`).
2. **Middleware** : Auth, logs, exceptions, rate limiting sont appliqués.
3. **Controller** : Reçoit la requête, valide les paramètres, appelle le service métier.
4. **Service** : Orchestration de la logique métier, transformation des données, appels aux repositories.
5. **Repository** : Accès à MongoDB, exécution des requêtes, mapping des résultats.
6. **DTO** : Les données sont mappées vers un DTO pour la réponse.
7. **Sortie** : Le contrôleur retourne la réponse HTTP (JSON) au client.

---

## 🔒 Sécurité

- **Authentification** par API Key (optionnelle, extensible JWT/OAuth2)
- **Rate limiting** par IP et par clé API
- **Validation stricte** des entrées (FluentValidation)
- **Logs** d'accès et d'erreurs centralisés
- **Headers de sécurité** HTTP (CORS, HSTS, etc.)

---

## ☁️ Scalabilité & résilience

- **Stateless** : chaque instance API est indépendante (scalable horizontalement)
- **MongoDB répliqué** possible (haute disponibilité)
- **Jobs Quartz** pour le scraping asynchrone
- **Docker Compose** pour l'orchestration multi-service

---

## 👀 Observabilité

- **Logs structurés** (niveau, trace, contexte)
- **Métriques Prometheus** (latence, erreurs, scraping, DB)
- **Dashboards Grafana** prêts à l'emploi
- **Healthchecks** (`/health`, `/metrics`)

---

## 🚀 Intégration continue & déploiement

- **GitHub Actions** : build, test, lint, coverage, déploiement Docker
- **Dockerfile multi-stage** pour des images légères et sécurisées
- **Environnements** de staging/production via variables d'environnement

---

## 🧩 Patterns utilisés

> **Repository Pattern** : abstraction de la base de données  
> **Service Layer** : logique métier centralisée  
> **DTO Pattern** : découplage API / modèle DB  
> **Middleware Pipeline** : gestion transversale (auth, logs, erreurs)  
> **Job Scheduling** : scraping asynchrone  
> **Validation Layer** : validation centralisée des entrées

---

## 🔗 Points d'extension

- Ajout d'une base SQL (PostgreSQL) via un nouveau repository
- Authentification avancée (JWT, OAuth2)
- Webhooks pour notifications externes
- Support multi-langue (i18n)
- Caching Redis pour accélérer les recherches

---

## 📚 Pour aller plus loin
- [Schéma de la base](database-schema.md)
- [Documentation API](api.md)
- [Stratégie de scraping](scraping.md)
- [Monitoring & métriques](prometheus.md)
- [Déploiement](deployment.md)
