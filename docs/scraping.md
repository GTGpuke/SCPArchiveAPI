# 🕷️ Stratégie de Scraping détaillée

## 🎯 Objectif
Synchroniser la base locale MongoDB avec le contenu du wiki SCP officiel, tout en respectant les ressources du site source et en assurant la qualité des données.

---

## 🔄 Cycle de vie du scraping
1. **Scraping initial**
   - Extraction de tous les articles SCP (par lot, avec délai entre requêtes)
   - Indexation des articles, métadonnées, tags
   - Stockage dans MongoDB (upsert)
2. **Mises à jour régulières**
   - Scraping différentiel basé sur la date de dernière modification
   - Vérification quotidienne (job Quartz à 03:00 UTC)
   - Mise à jour des articles modifiés, ajout des nouveaux
3. **Scraping manuel**
   - Déclenché via l'API (endpoint POST)
   - Utilisé pour forcer la mise à jour d'un article spécifique

---

## ⚙️ Configuration

| Clé                  | Description                                 | Exemple                                  |
|----------------------|---------------------------------------------|------------------------------------------|
| `BaseUrl`            | URL du wiki SCP                             | `http://www.scp-wiki.net`                |
| `DelayBetweenRequests`| Délai (ms) entre chaque requête             | `1000`                                   |
| `UserAgent`          | User-Agent personnalisé                     | `SCPArchive-Bot/1.0 (contact@...)`       |
| `Timeout`            | Timeout HTTP (ms)                           | `10000`                                  |

**Exemple (appsettings.json) :**
```json
"Scraping": {
  "BaseUrl": "http://www.scp-wiki.net",
  "DelayBetweenRequests": 1000,
  "UserAgent": "SCPArchive-Bot/1.0 (contact@example.com)"
}
```

---

## 🧩 Extraction des données
- **HtmlAgilityPack** : Parsing du HTML, extraction des sections (titre, classe, description, addenda, tags)
- **Pattern d'extraction** :
  - XPath/CSS selectors pour cibler les blocs de contenu
  - Regex pour extraire la classe d'objet, la note, etc.
- **Gestion des addenda** : Extraction des blocs additionnels (Addendum, Notes, etc.)

---

## 🚨 Gestion des erreurs
- Retry avec backoff exponentiel en cas d'échec
- Logging détaillé (succès, erreurs, temps de scraping)
- Alertes si trop d'échecs consécutifs

---

## ⏰ Planification & orchestration
- **Quartz.NET** : Planification des jobs de scraping (full, delta, manuel)
- **Docker Compose** : Orchestration multi-service (API, MongoDB, Prometheus)
- **Logs** : Historique des jobs, durée, nombre d'articles scrapés

---

## 🤝 Respect du site source
- Respect du `robots.txt`
- Délai configurable entre les requêtes
- User-Agent explicite
- Pas de scraping massif en journée (UTC)

---

## ✅ Bonnes pratiques
- Sauvegarde régulière de la base MongoDB
- Monitoring du taux de succès/échec
- Limitation du nombre de requêtes simultanées
- Possibilité de désactiver le scraping via variable d'environnement

---

## 📝 Exemple de log de scraping
```
[2025-06-16 03:00:00] Démarrage du scraping différentiel
[2025-06-16 03:00:01] Article SCP-173 mis à jour
[2025-06-16 03:00:02] Article SCP-682 inchangé
[2025-06-16 03:00:10] Scraping terminé : 1 article mis à jour, 0 erreur
```

---

## 📚 Pour aller plus loin
- [Schéma de la base](database-schema.md)
- [Architecture](architecture.md)
- [Monitoring & métriques](prometheus.md)
- [Déploiement](deployment.md)
