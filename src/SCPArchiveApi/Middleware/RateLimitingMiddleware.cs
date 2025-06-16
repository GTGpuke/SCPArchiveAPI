// Middleware pour la limitation du nombre de requêtes (rate limiting)
// Permet de protéger l'API contre les abus en limitant le nombre de requêtes par client/IP

namespace SCPArchiveApi.Middleware;

/// <summary>
/// Middleware pour le rate limiting (limitation du nombre de requêtes)
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    // Dictionnaire pour stocker les "buckets" de chaque client (IP)
    private readonly Dictionary<string, TokenBucket> _buckets = new();
    private readonly object _lock = new();

    public RateLimitingMiddleware(RequestDelegate next, ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Intercepte chaque requête et vérifie si le client a dépassé sa limite
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        
        if (!IsRateLimited(clientIp))
        {
            await _next(context); // Passe au middleware suivant
            return;
        }

        // Si la limite est dépassée, retourne une erreur 429
        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.Response.WriteAsync("Trop de requêtes. Veuillez réessayer plus tard.");
    }

    /// <summary>
    /// Vérifie et met à jour le "bucket" de l'IP (algorithme du token bucket)
    /// </summary>
    private bool IsRateLimited(string clientIp)
    {
        lock (_lock)
        {
            if (!_buckets.TryGetValue(clientIp, out var bucket))
            {
                // 100 requêtes par minute par défaut
                bucket = new TokenBucket(100, 100, TimeSpan.FromMinutes(1));
                _buckets[clientIp] = bucket;
            }

            return !bucket.TryTake();
        }
    }

    /// <summary>
    /// Implémentation simple du token bucket pour le rate limiting
    /// </summary>
    private class TokenBucket
    {
        private readonly int _capacity;
        private readonly int _refillAmount;
        private readonly TimeSpan _refillTime;
        private int _tokens;
        private DateTime _lastRefill;

        public TokenBucket(int capacity, int refillAmount, TimeSpan refillTime)
        {
            _capacity = capacity;
            _refillAmount = refillAmount;
            _refillTime = refillTime;
            _tokens = capacity;
            _lastRefill = DateTime.UtcNow;
        }

        /// <summary>
        /// Tente de consommer un jeton, sinon refuse la requête
        /// </summary>
        public bool TryTake()
        {
            RefillTokens();

            if (_tokens > 0)
            {
                _tokens--;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Recharge les jetons si le temps de recharge est écoulé
        /// </summary>
        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var timePassed = now - _lastRefill;

            if (timePassed >= _refillTime)
            {
                _tokens = Math.Min(_capacity, _tokens + _refillAmount);
                _lastRefill = now;
            }
        }
    }
}
