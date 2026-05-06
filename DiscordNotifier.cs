using System.Text;
using System.Text.Json;

namespace WebCamControl;

internal sealed class DiscordNotifier : IDisposable
{
    private readonly HttpClient _http = new();

    internal string WebhookUrl { get; set; } = string.Empty;

    internal bool IsConfigured => !string.IsNullOrWhiteSpace(WebhookUrl);

    internal async Task SendAsync(string text)
    {
        if (!IsConfigured) return;

        var payload = JsonSerializer.Serialize(new { content = text });
        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        try   { await _http.PostAsync(WebhookUrl, content); }
        catch { /* best-effort */ }
    }

    public void Dispose() => _http.Dispose();
}
