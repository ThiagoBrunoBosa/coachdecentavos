namespace CoachDecentavos.Application.Common;

public sealed class LlmOptions
{
    public const string SectionName = "Llm";

    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.groq.com/openai/v1";
    public string Model { get; set; } = "llama-3.1-8b-instant";
    public bool Enabled { get; set; } = true;
}
