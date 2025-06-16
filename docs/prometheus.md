# Métriques Prometheus

## Endpoints exposés

### /metrics
Point d'accès principal pour les métriques Prometheus.

## Métriques personnalisées

### Performances API
```
# Latence des requêtes
scp_request_duration_seconds{endpoint="/api/v1/scps", method="GET"} 0.123

# Nombre de requêtes
scp_http_requests_total{endpoint="/api/v1/scps", method="GET", status="200"} 1234

# Taux d'erreurs
scp_http_errors_total{endpoint="/api/v1/scps", status="500"} 12
```

### Scraping
```
# Durée des opérations de scraping
scp_scraping_duration_seconds{type="full"} 3600

# Nombre d'articles scrapés
scp_scraped_articles_total 5000

# Erreurs de scraping
scp_scraping_errors_total{reason="timeout"} 5
```

### Base de données
```
# Latence des requêtes MongoDB
scp_mongo_operation_duration_seconds{operation="find"} 0.05

# Taille de la base
scp_database_size_bytes 1234567

# Nombre de documents
scp_documents_total{collection="scps"} 5000
```

## Dashboard Grafana

### Panels recommandés

1. Taux de requêtes par endpoint
2. Latence moyenne (p95, p99)
3. Taux d'erreurs
4. Performances du scraping
5. Santé de la base de données

### Alertes

- Latence > 500ms
- Taux d'erreurs > 1%
- Échecs de scraping consécutifs
- Espace disque MongoDB < 20%
