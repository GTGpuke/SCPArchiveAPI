# Stratégie de Scraping

## Vue d'ensemble

Le scraping du Wiki SCP est effectué de manière respectueuse pour éviter toute surcharge du site source.

## Fréquence et planification

### Scraping initial
- Extraction complète initiale avec délais entre requêtes
- Sauvegarde des données dans MongoDB
- Génération des métadonnées et index

### Mises à jour régulières
- Vérification quotidienne des modifications
- Scraping différentiel basé sur les dates de modification
- Mise à jour nocturne (03:00 UTC)

## Implémentation technique

### Gestion des requêtes
```csharp
// Délai entre les requêtes
private const int REQUEST_DELAY_MS = 1000;

// User-Agent personnalisé
private const string USER_AGENT = "SCPArchive-Bot/1.0 (contact@example.com)";
```

### Extraction du contenu
- Utilisation de HtmlAgilityPack
- Parser personnalisé pour le format Wiki
- Gestion des cas particuliers (tableaux, images)

### Gestion des erreurs
- Retry pattern avec backoff exponentiel
- Logging détaillé
- Alertes en cas d'échecs répétés

## Bonnes pratiques

1. Respect des robots.txt
2. Cache des requêtes
3. Monitoring des performances
4. Sauvegarde des données extraites
