# Guide de Déploiement

## Prérequis

- Docker & Docker Compose
- MongoDB 6.0+
- .NET 8 SDK (développement)
- 2GB RAM minimum
- 10GB espace disque

## Configuration

### Variables d'environnement
```bash
# MongoDB
MONGO_URI=mongodb://mongodb:27017
MONGO_DB=scparchive

# API
ASPNETCORE_ENVIRONMENT=Production
API_KEY=your-secret-key
RATE_LIMIT=100

# Monitoring
PROMETHEUS_PORT=9090
GRAFANA_PORT=3000
```

## Déploiement Docker

### Production
```bash
# Build et démarrage
docker-compose -f docker-compose.prod.yml up -d

# Vérification des logs
docker-compose logs -f api

# Arrêt
docker-compose down
```

### Scaling
```bash
# Scaling horizontal
docker-compose up -d --scale api=3

# Avec un load balancer (nginx)
docker-compose -f docker-compose.prod.yml -f docker-compose.lb.yml up -d
```

## Sauvegarde

### MongoDB
```bash
# Backup
docker-compose exec mongodb mongodump --out /backup

# Restore
docker-compose exec mongodb mongorestore /backup
```

## Monitoring

- Prometheus: http://localhost:9090
- Grafana: http://localhost:3000
- API Metrics: http://localhost:5000/metrics

## Sécurité

1. Mise à jour régulière des conteneurs
2. Scan de vulnérabilités
3. Configuration HTTPS
4. Rate limiting
5. Authentification API
