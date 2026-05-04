namespace WebCamControl;

internal sealed class TelegramNotifier : IDisposable
{
    private readonly HttpClient _http = new();

    internal string BotToken { get; set; } = string.Empty;
    internal string ChatId   { get; set; } = string.Empty;

    internal bool IsConfigured =>
        !string.IsNullOrWhiteSpace(BotToken) && !string.IsNullOrWhiteSpace(ChatId);

    internal async Task SendAsync(string text)
    {
        if (!IsConfigured) return;

        var url     = $"https://api.telegram.org/bot{BotToken}/sendMessage";
        var content = new FormUrlEncodedContent([
            new("chat_id", ChatId),
            new("text",    text),
        ]);

        try   { await _http.PostAsync(url, content); }
        catch { /* best-effort */ }
    }

    public void Dispose() => _http.Dispose();
}
