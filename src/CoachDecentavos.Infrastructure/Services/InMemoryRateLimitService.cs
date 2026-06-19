using System.Collections.Concurrent;

namespace CoachDecentavos.Infrastructure.Services;

public sealed class InMemoryRateLimitService : Application.Common.Interfaces.IRateLimitService
{
    private readonly ConcurrentDictionary<string, Queue<DateTime>> _buckets = new();

    public bool TryAcquire(string key, int maxRequests, TimeSpan window)
    {
        var now = DateTime.UtcNow;
        var queue = _buckets.GetOrAdd(key, _ => new Queue<DateTime>());

        lock (queue)
        {
            while (queue.Count > 0 && now - queue.Peek() > window)
                queue.Dequeue();

            if (queue.Count >= maxRequests)
                return false;

            queue.Enqueue(now);
            return true;
        }
    }
}
