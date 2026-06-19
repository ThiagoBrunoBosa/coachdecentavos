using System.Net.Http.Json;
using System.Text.Json;
using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Infrastructure.Ai;

public sealed class GroqChatClient : IGroqChatClient
{
    private readonly HttpClient _httpClient;
    private readonly LlmOptions _options;

    public GroqChatClient(HttpClient httpClient, IOptions<LlmOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> AskAsync(
        string systemPrompt,
        string userMessage,
        int maxTokens,
        CancellationToken cancellationToken = default)
    {
        if (!_options.Enabled || string.IsNullOrWhiteSpace(_options.ApiKey))
            throw new InvalidOperationException("AI assistant is disabled.");

        var request = new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = "system", content = systemPrompt },
                new { role = "user", content = userMessage },
            },
            max_tokens = maxTokens,
            temperature = 0.4,
        };

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl.TrimEnd('/')}/chat/completions");
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ApiKey);
        httpRequest.Content = JsonContent.Create(request);

        using var response = await _httpClient.SendAsync(httpRequest, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return content?.Trim() ?? string.Empty;
    }
}
