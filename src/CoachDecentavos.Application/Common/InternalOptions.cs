namespace CoachDecentavos.Application.Common;

public sealed class InternalOptions
{
    public const string SectionName = "Internal";

    public string CronSecret { get; set; } = string.Empty;
}
