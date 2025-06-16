# 🗄️ Schéma de base de données – SCPArchiveAPI

Ce document décrit :
- La structure de la base MongoDB utilisée par SCPArchiveAPI
- Les bonnes pratiques de modélisation
- Les stratégies automatisées de sécurité, maintenance, et conformité (RGPD)

---

## 📚 Collections principales

### 1. `scps`

Contient les articles SCP extraits du site officiel.

| Champ           | Type        | Description                                |
|----------------|-------------|--------------------------------------------|
| `scpId`        | `string`    | Identifiant SCP (ex : "SCP-173")           |
| `title`        | `string`    | Titre de l’article                         |
| `content`      | `string`    | Contenu HTML complet                       |
| `tags`         | `string[]`  | Tags liés (ex : euclid, biological)        |
| `classification`| `string`   | Classe d’objet (Keter, Euclid…)            |
| `author`       | `string`    | Nom d’auteur (si détectable)               |
| `url`          | `string`    | URL source                                 |
| `dateScraped`  | `datetime`  | Date du dernier scraping                   |
| `metadata`     | `object`    | Infos supplémentaires (langue, etc.)       |
| `isDeleted`    | `bool`      | Suppression logique (soft delete)          |
| `deletedAt`    | `datetime`  | Date de suppression (si applicable)        |

**Index recommandés :**
- `{ scpId: 1 }` – unique
- `{ tags: 1 }`
- `{ classification: 1 }`
- `{ dateScraped: -1 }`

---

### 2. `scraping_jobs`

Suivi des tâches de scraping.

| Champ          | Type        | Description                                |
|----------------|-------------|--------------------------------------------|
| `jobId`        | `string`    | Identifiant unique                         |
| `startTime`    | `datetime`  | Début                                      |
| `endTime`      | `datetime`  | Fin                                        |
| `status`       | `string`    | Succès / Échec / Partiel                   |
| `pagesScraped` | `int`       | Nombre de pages traitées                   |
| `errors`       | `int`       | Nombre d’erreurs                           |
| `log`          | `string`    | Message ou trace résumé                    |

---

### 3. `audit_logs`

Historique des modifications sensibles.

| Champ         | Type        | Description                                |
|---------------|-------------|--------------------------------------------|
| `entity`      | `string`    | Nom de la collection ciblée                |
| `entityId`    | `string`    | ID du document concerné                    |
| `action`      | `string`    | `create` / `update` / `delete`             |
| `timestamp`   | `datetime`  | Date et heure                              |
| `userId`      | `string`    | ID de l'utilisateur                        |
| `diff`        | `object`    | Détail des champs modifiés                 |

---

## ✅ Bonnes pratiques de modélisation

- Utilisation de **FluentValidation** pour vérifier les modèles dès leur création.
- **Suppression logique (soft delete)** systématique pour éviter les pertes.
- Séparation claire entre données métier (`scps`) et techniques (`scraping_jobs`, `audit_logs`).
- Indexation optimisée selon les usages (filtrage, tri, recherche plein texte).

---

## 🔁 Automatisation

- **Nettoyage périodique** des `isDeleted = true` après 30 jours.
- **Archivage** des `scps` inactifs (> 1 an) dans une collection `scps_archive`.
- **Vérifications automatisées** de données corrompues ou incomplètes.
- **Tâches planifiées** (via Quartz.NET ou cron Docker) pour maintenance régulière.

---

## 🛡️ Sécurité des données

- **Connexion chiffrée (TLS)** à la base MongoDB.
- **Accès restreint** aux seules IPs nécessaires (VPC ou firewall cloud).
- **Comptes MongoDB avec rôles limités** : lecture, écriture, admin.
- **Chiffrement des données au repos** (via volumes chiffrés ou Atlas natif).
- **Logs d’accès sensibles** enregistrés dans `audit_logs`.

---

## 📜 RGPD & conformité

- **Suppression complète sur demande utilisateur** : tous les documents liés doivent être effacés.
- **Expiration configurable** : les logs et données techniques ont une durée de vie par défaut.
- **Anonymisation** : test et analyse réalisés sur des jeux sans identifiants personnels.
- **Journal de consentement** (si utilisateur intégré) : date et choix enregistré.
- **Documentation interne** : traitements documentés, finalités, conservation, contact DPO.

---

## ⚙️ Maintenance à long terme

- **Sauvegardes automatiques** : quotidiennes, testées régulièrement.
- **Surveillance de la base** : croissance, anomalies, erreurs via Prometheus + Grafana.
- **Migration versionnée** : historique des changements de schéma.
- **Scripts d’administration** : purge, export CSV, anonymisation, reset.

---

## 📋 Checklist (à maintenir)

| Élément                                   | Recommandé | Fréquence     |
|-------------------------------------------|------------|---------------|
| Sauvegardes automatisées                  | ✅          | Quotidienne    |
| Purge des `isDeleted = true`              | ✅          | Hebdomadaire   |
| Surveillance croissance base              | ✅          | Continue       |
| Audit des accès `audit_logs`              | ✅          | Permanente     |
| Documentation RGPD                        | ✅          | Semestrielle   |
| Tests de restauration de backup           | ✅          | Trimestrielle  |

---

## 🔗 Ressources utiles

- [MongoDB Security Checklist](https://www.mongodb.com/security)
- [CNIL – Guide RGPD développeurs](https://www.cnil.fr/fr/developpeurs)
- [MongoDB Data Lifecycle Management](https://www.mongodb.com/use-cases/data-archiving)