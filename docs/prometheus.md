# 📊 Monitoring & Métriques Prometheus

## 🌐 Endpoints exposés
- **/metrics** : endpoint Prometheus standard (format texte)
- **/health** : endpoint santé (JSON)

---

## 📈 Métriques API

| Nom                                 | Labels                        | Description                                 |
|-------------------------------------|-------------------------------|---------------------------------------------|
| `scp_http_requests_total`           | endpoint, method, status      | Nombre total de requêtes HTTP               |
| `scp_request_duration_seconds`      | endpoint, method              | Latence des requêtes (histogramme)          |
| `scp_http_errors_total`             | endpoint, status              | Nombre d'erreurs HTTP                       |
| `scp_rate_limit_exceeded_total`     | ip                            | Nombre de refus pour dépassement de quota   |

## 🕷️ Métriques Scraping

| Nom                                 | Labels                        | Description                                 |
|-------------------------------------|-------------------------------|---------------------------------------------|
| `scp_scraping_duration_seconds`     | type                          | Durée des jobs de scraping                  |
| `scp_scraped_articles_total`        | -                             | Nombre total d'articles scrapés             |
| `scp_scraping_errors_total`         | reason                        | Nombre d'échecs de scraping par raison      |

## 🗄️ Métriques MongoDB

| Nom                                 | Labels                        | Description                                 |
|-------------------------------------|-------------------------------|---------------------------------------------|
| `scp_mongo_operation_duration_seconds`| operation                    | Latence des opérations MongoDB              |
| `scp_database_size_bytes`           | -                             | Taille totale de la base                    |
| `scp_documents_total`               | collection                    | Nombre de documents par collection          |

---

## 🏷️ Labels Prometheus utilisés
- `endpoint` : chemin de l'API (ex: /api/v1/scps)
- `method` : méthode HTTP (GET, POST...)
- `status` : code HTTP (200, 404, 500...)
- `type` : type de scraping (full, delta, manuel)
- `operation` : type d'opération MongoDB
- `reason` : cause d'une erreur scraping
- `ip` : adresse IP du client (pour le rate limiting)

---

## 📊 Panels Grafana recommandés
1. **Taux de requêtes par endpoint**
2. **Latence moyenne (p95, p99)**
3. **Taux d'erreurs par endpoint**
4. **Performances du scraping (durée, succès/échecs)**
5. **Santé de la base MongoDB (taille, latence, documents)**

---

## 🚨 Alertes recommandées
- Latence > 500ms sur un endpoint
- Taux d'erreurs > 1% sur un endpoint
- Plus de 5 échecs de scraping consécutifs
- Espace disque MongoDB < 20%
- Plus de 10 refus de rate limiting/minute

---

## ⚙️ Exemple de configuration Prometheus
```yaml
scrape_configs:
  - job_name: 'scp-api'
    static_configs:
      - targets: ['api:80']
```

---

## 📝 Exemple de métrique exposée
```
scp_http_requests_total{endpoint="/api/v1/scps",method="GET",status="200"} 1234
scp_request_duration_seconds_bucket{endpoint="/api/v1/scps",le="0.1"} 1000
scp_scraping_errors_total{reason="timeout"} 2
```

---

## 📚 Pour aller plus loin
- [Architecture](architecture.md)
- [Stratégie de scraping](scraping.md)
- [Déploiement](deployment.md)
