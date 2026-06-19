namespace CoachDecentavos.Application.Common;

public sealed record ApiErrorResponse(string Message, string? TraceId = null);

public sealed record HealthResponse(string Status, DateTime TimestampUtc);