using CoachDecentavos.Application.Common;
using CoachDecentavos.Application.Common.Interfaces;
using CoachDecentavos.Domain.Entities;
using CoachDecentavos.Domain.Enums;
using Microsoft.Extensions.Options;

namespace CoachDecentavos.Application.Ai;

public sealed record AskAssistantRequest(string Question, Guid? ProductId = null);

public sealed record AskAssistantResponse(string Answer);

public sealed record AcceptAiConsentRequest(string DisclaimerVersion);

public sealed class FinancialAssistantService
{
    public const string CurrentDisclaimerVersion = "2026-06-1";
    private const int MaxRequestsPerHour = 20;

    private readonly IAiRepository _ai;
    private readonly IGroqChatClient _groq;
    private readonly IRateLimitService _rateLimit;
    private readonly LlmOptions _llmOptions;

    public FinancialAssistantService(
        IAiRepository ai,
        IGroqChatClient groq,
        IRateLimitService rateLimit,
        IOptions<LlmOptions> llmOptions)
    {
        _ai = ai;
        _groq = groq;
        _rateLimit = rateLimit;
        _llmOptions = llmOptions.Value;
    }

    public async Task AcceptConsentAsync(Guid userId, string disclaimerVersion, CancellationToken cancellationToken = default)
    {
        if (await _ai.HasConsentAsync(userId, cancellationToken))
            return;

        await _ai.AddConsentAsync(UserAiConsent.Create(userId, disclaimerVersion), cancellationToken);
        await _ai.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> HasConsentAsync(Guid userId, CancellationToken cancellationToken = default)
        => await _ai.HasConsentAsync(userId, cancellationToken);

    public async Task<AskAssistantResponse> AskAsync(
        Guid userId,
        PreferredLocale locale,
        AskAssistantRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!await _ai.HasConsentAsync(userId, cancellationToken))
            throw new InvalidOperationException("AI consent is required.");

        if (!_rateLimit.TryAcquire($"ai:{userId}", MaxRequestsPerHour, TimeSpan.FromHours(1)))
            throw new InvalidOperationException("Rate limit exceeded. Try again later.");

        var language = locale == PreferredLocale.EnUs ? "English" : "Portuguese (Brazil)";
        var systemPrompt = $"""
            You are Carolyne Moraes, a warm and professional CFP financial coach.
            Respond in {language}.
            Provide educational guidance only. Never recommend specific stocks, funds, or crypto assets.
            Never tell the user to buy or sell anything specific.
            Keep answers concise (max 3 short paragraphs).
            If unsure, suggest speaking with Carolyne in a consulting session.

            Mandatory disclaimer to include at the end:
            "This content is educational and does not constitute individualized investment advice."
            """;

        var answer = string.IsNullOrWhiteSpace(_llmOptions.ApiKey) || !_llmOptions.Enabled
            ? BuildOfflineAnswer(locale, request.Question.Trim())
            : await _groq.AskAsync(systemPrompt, request.Question.Trim(), 500, cancellationToken);

        var session = await _ai.GetOrCreateSessionAsync(userId, request.ProductId, cancellationToken);
        await _ai.AddMessageAsync(ChatMessage.Create(session.Id, "user", request.Question.Trim()), cancellationToken);
        await _ai.AddMessageAsync(ChatMessage.Create(session.Id, "assistant", answer), cancellationToken);
        await _ai.SaveChangesAsync(cancellationToken);

        return new AskAssistantResponse(answer);
    }

    private static string BuildOfflineAnswer(PreferredLocale locale, string question)
    {
        var preview = question.Length > 120 ? question[..120] + "…" : question;

        if (locale == PreferredLocale.EnUs)
        {
            return $"""
                Thank you for your question about personal finance. The assistant is running in demo mode (no external AI key configured).

                Educational tip: start by mapping monthly income and fixed expenses, build an emergency fund, then consider investments aligned with your goals and timeline.

                Your question: "{preview}"

                For personalized guidance, book a consulting session with Carolyne.

                This content is educational and does not constitute individualized investment advice.
                """;
        }

        return $"""
            Obrigado pela sua pergunta sobre finanças pessoais. No momento estou em modo demonstração (sem chave de IA configurada).

            Sugestão educativa: comece mapeando receitas e despesas fixas do mês, defina uma reserva de emergência e só então pense em investimentos de acordo com seu perfil e prazo.

            Sua pergunta: "{preview}"

            Para orientação personalizada, agende uma consultoria com a Carolyne.

            Este conteúdo é educativo e não constitui recomendação de investimento individualizada.
            """;
    }
}
