using CoachDecentavos.Domain.Enums;

namespace CoachDecentavos.Domain.Entities;

public class ContentTranslation
{
    public Guid Id { get; private set; }
    public ContentEntityType EntityType { get; private set; }
    public Guid EntityId { get; private set; }
    public PreferredLocale Locale { get; private set; }
    public string FieldKey { get; private set; } = string.Empty;
    public string Value { get; private set; } = string.Empty;
    public DateTime UpdatedAtUtc { get; private set; }

    private ContentTranslation() { }

    public static ContentTranslation Create(ContentEntityType entityType, Guid entityId, PreferredLocale locale, string fieldKey, string value)
    {
        if (entityId == Guid.Empty)
            throw new ArgumentException("Entity id is required.", nameof(entityId));
        if (string.IsNullOrWhiteSpace(fieldKey))
            throw new ArgumentException("Field key is required.", nameof(fieldKey));

        return new ContentTranslation
        {
            Id = Guid.NewGuid(),
            EntityType = entityType,
            EntityId = entityId,
            Locale = locale,
            FieldKey = fieldKey.Trim(),
            Value = value ?? string.Empty,
            UpdatedAtUtc = DateTime.UtcNow
        };
    }
}