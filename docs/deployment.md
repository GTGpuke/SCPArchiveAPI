# 🚀 Guide de Déploiement détaillé

## 🧰 Prérequis
- Docker & Docker Compose
- MongoDB 6.0+ (inclus dans le docker-compose)
- .NET 8 SDK (pour développement local)
- 2GB RAM minimum, 10GB espace disque

---

## ⚙️ Configuration des variables d'environnement
- `.env` : centralise les variables sensibles (URI MongoDB, API Key, etc.)
- `appsettings.json` : configuration par défaut (scraping, logs, etc.)

| Variable              | Description                        | Exemple                        |
|-----------------------|------------------------------------|--------------------------------|
| `MONGO_URI`           | URI de connexion MongoDB           | `mongodb://mongodb:27017`      |
| `MONGO_DB`            | Nom de la base MongoDB             | `scparchive`                   |
| `ASPNETCORE_ENVIRONMENT` | Environnement .NET               | `Production`                   |
| `API_KEY`             | Clé API admin                      | `your-secret-key`              |
| `RATE_LIMIT`          | Limite de requêtes/minute          | `100`                          |
| `PROMETHEUS_PORT`     | Port Prometheus                    | `9090`                         |
| `GRAFANA_PORT`        | Port Grafana                       | `3000`                         |

---

## 🐳 Déploiement Docker Compose
- `docker-compose.yml` orchestre :
  - API SCPArchive
  - MongoDB (persistance volume)
  - Prometheus (monitoring)
  - Grafana (dashboard)

### Commandes principales
```bash
# Build et lancement
docker-compose up -d --build
# Logs API
docker-compose logs -f api
# Arrêt
docker-compose down
```

---

## 📈 Scaling & haute disponibilité
- **API** : scalable horizontalement (`docker-compose up -d --scale api=3`)
- **MongoDB** : support du replica set (voir documentation MongoDB)
- **Load balancer** : possible via Nginx ou Traefik

---

## 💾 Sauvegarde & restauration
- **MongoDB** :
```bash
# Sauvegarde
docker-compose exec mongodb mongodump --out /backup
# Restauration
docker-compose exec mongodb mongorestore /backup
```
- **Volumes Docker** : à sauvegarder régulièrement

---

## 🔒 Sécurité
- API Key obligatoire pour les endpoints sensibles
- Limitation du nombre de requêtes (rate limiting)
- Mise à jour régulière des images Docker
- Scan de vulnérabilités (Trivy, Snyk...)
- Configuration HTTPS recommandée (reverse proxy)

---

## 🕵️‍♂️ Supervision & logs
- **Prometheus** : http://localhost:9090
- **Grafana** : http://localhost:3000 (admin/admin)
- **API Metrics** : http://localhost:5000/metrics
- **Logs** : accessibles via `docker-compose logs`

---

## 🔄 CI/CD
- **GitHub Actions** : build, test, lint, coverage, push image Docker
- **Déploiement automatique** : possible sur Azure, AWS, GCP, OVH, etc.
- **Secrets** : stockés dans GitHub Secrets ou Azure Key Vault

---

## 🏅 Bonnes pratiques
- Utiliser des tags d'image Docker versionnés
- Séparer les environnements (dev, staging, prod)
- Monitorer l'espace disque MongoDB
- Mettre à jour régulièrement les dépendances
- Documenter les procédures de rollback

---

## 📚 Pour aller plus loin
- [Architecture](architecture.md)
- [Monitoring & métriques](prometheus.md)
- [Stratégie de scraping](scraping.md)
