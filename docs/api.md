# Documentation de l'API

## Versioning
- Toutes les routes sont préfixées par `/api/v{version}/` (ex: `/api/v1/scps`)
- Version par défaut : **v1**

## Authentification
- Par défaut, l'API est publique en lecture.
- Les endpoints sensibles (scraping, admin) nécessitent une API Key (header `X-API-KEY`).

---

## 🚦 Endpoints principaux

### 1. Récupération d'un article SCP
`GET /api/v1/scps/{itemNumber}`

| Paramètre    | Type   | Description                  |
|--------------|--------|------------------------------|
| itemNumber   | string | Numéro SCP (ex: SCP-173)     |

**Réponse :**
```json
{
  "itemNumber": "SCP-173",
  "title": "La Statue",
  "objectClass": "Euclid",
  "content": {
    "description": "...",
    "containment": "...",
    "addenda": [ { "title": "Addendum 1", "content": "..." } ]
  },
  "tags": ["statue", "dangerous"],
  "metadata": {
    "author": "Dr. Gears",
    "creationDate": "2010-01-01T00:00:00Z",
    "lastModified": "2024-06-01T12:00:00Z",
    "rating": 1234
  },
  "scraping": {
    "lastScraped": "2025-06-16T10:00:00Z",
    "sourceUrl": "http://www.scp-wiki.net/scp-173"
  }
}
```

| Code | Signification |
|------|--------------|
| 200  | OK           |
| 404  | Non trouvé   |

---

### 2. Recherche d'articles SCP
`GET /api/v1/scps`

| Paramètre    | Type   | Description                                 |
|--------------|--------|---------------------------------------------|
| query        | string | Texte à rechercher (full-text)              |
| objectClass  | string | Filtre (Safe, Euclid, Keter...)             |
| tags         | string | Filtre par tags (séparés par virgule)       |
| sort         | string | Champ de tri (`rating`, `date`, `views`)    |
| page         | int    | Numéro de page (défaut 1)                   |
| pageSize     | int    | Taille de page (défaut 20, max 100)         |

**Réponse :**
```json
{
  "items": [ ... ],
  "total": 1234,
  "query": { ... }
}
```

| Header             | Description                        |
|--------------------|------------------------------------|
| X-Total-Count      | Nombre total de résultats           |
| Link               | Pagination (RFC 5988)              |

---

### 3. Déclencher le scraping manuel
`POST /api/v1/scps/{itemNumber}/scrape`

- Lance le scraping d'un article spécifique (admin)
- Header : `X-API-KEY: ...`
- Réponse : **202 Accepted**

---

### 4. Endpoint santé
`GET /health`

**Réponse :**
```json
{
  "status": "Healthy",
  "checks": {
    "mongodb": "Healthy",
    "scraper": "Healthy"
  }
}
```

---

### 5. Endpoint Prometheus
`GET /metrics`
- Expose les métriques Prometheus (latence, erreurs, scraping, DB)

---

## ⚠️ Gestion des erreurs

Toutes les erreurs sont retournées au format JSON :
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Le champ 'title' est requis",
    "details": { ... }
  }
}
```

| Code | Signification         |
|------|----------------------|
| 400  | Erreur de validation |
| 401  | Non authentifié      |
| 403  | Non autorisé         |
| 404  | Non trouvé           |
| 429  | Trop de requêtes     |
| 500  | Erreur serveur       |

---

## 📄 Pagination & tri
- Pagination via `page` et `pageSize`
- Tri via `sort` (ex: `rating`, `date`)
- Headers HTTP :
  - `X-Total-Count` : total des résultats
  - `Link` : liens de pagination (next, prev, last, first)

---

## 🚦 Rate limiting
- 100 requêtes/minute par IP (non authentifié)
- 1000 requêtes/minute par API Key
- Headers :
  - `X-RateLimit-Limit`
  - `X-RateLimit-Remaining`
  - `X-RateLimit-Reset`

---

## 🧭 Conventions REST
- Utilisation des verbes HTTP standards (GET, POST, PUT, DELETE)
- Statuts HTTP explicites
- Documentation Swagger disponible sur `/swagger`

---

## 💡 Exemples de requêtes

```http
GET /api/v1/scps?query=statue&objectClass=Euclid&page=2&pageSize=10
```

```http
POST /api/v1/scps/SCP-173/scrape
X-API-KEY: votrecletest
```

---

## 📚 Pour aller plus loin
- [Schéma de la base](database-schema.md)
- [Architecture](architecture.md)
- [Stratégie de scraping](scraping.md)
- [Monitoring & métriques](prometheus.md)
- [Déploiement](deployment.md)
