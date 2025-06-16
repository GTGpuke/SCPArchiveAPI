# Documentation de l'API

## Points d'accès REST

### Objets SCP

#### GET /api/v1/scps
Récupère une liste paginée d'objets SCP.

**Paramètres de requête**
- `page` (défaut: 1) - Numéro de page
- `limit` (défaut: 20, max: 100) - Nombre d'éléments par page
- `objectClass` - Filtre par classe (Safe, Euclid, Keter)
- `search` - Recherche textuelle
- `sort` - Tri (rating, date, views)

#### GET /api/v1/scps/{id}
Récupère un objet SCP spécifique par son ID.

#### POST /api/v1/scps/scrape
Déclenche un scraping manuel (authentification admin requise).

### Métriques et Santé

#### GET /health
Vérifie l'état de l'API et ses dépendances.

#### GET /metrics
Endpoints Prometheus pour le monitoring.

## Pagination

La pagination utilise les headers HTTP standards:
```
Link: <https://api.../scps?page=2>; rel="next",
      <https://api.../scps?page=10>; rel="last"
X-Total-Count: 100
```

## Rate Limiting

- 100 requêtes/minute pour les clients non authentifiés
- 1000 requêtes/minute pour les clients authentifiés
- Headers de réponse:
  ```
  X-RateLimit-Limit: 100
  X-RateLimit-Remaining: 95
  X-RateLimit-Reset: 1623456789
  ```

## Gestion des erreurs

Format JSON standard pour les erreurs :
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Le champ 'title' est requis",
    "details": { ... }
  }
}
