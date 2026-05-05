using System.Net.Http.Headers;
using System.Text.Json;

namespace WebCamControl;

internal sealed class KakaoNotifier : IDisposable
{
    private readonly HttpClient _http = new();

    internal string RestApiKey   { get; set; } = string.Empty;
    internal string AccessToken  { get; set; } = string.Empty;
    internal string RefreshToken { get; set; } = string.Empty;

    internal bool IsConfigured =>
        !string.IsNullOrWhiteSpace(AccessToken) || !string.IsNullOrWhiteSpace(RefreshToken);

    internal static string BuildAuthUrl(string restApiKey) =>
        "https://kauth.kakao.com/oauth/authorize" +
        $"?client_id={Uri.EscapeDataString(restApiKey)}" +
        $"&redirect_uri={Uri.EscapeDataString(KakaoAuthServer.RedirectUri)}" +
        "&response_type=code" +
        "&scope=talk_message";

    internal async Task<bool> ExchangeCodeAsync(string code)
    {
        var content = new FormUrlEncodedContent([
            new("grant_type",   "authorization_code"),
            new("client_id",    RestApiKey),
            new("redirect_uri", KakaoAuthServer.RedirectUri),
            new("code",         code),
        ]);

        try
        {
            var response = await _http.PostAsync("https://kauth.kakao.com/oauth/token", content);
            return ParseTokenResponse(await response.Content.ReadAsStringAsync());
        }
        catch { return false; }
    }

    internal async Task SendAsync(string text)
    {
        if (!IsConfigured) return;

        if (string.IsNullOrWhiteSpace(AccessToken) && !string.IsNullOrWhiteSpace(RefreshToken))
            await RefreshAsync();

        if (!await TrySendAsync(text) && !string.IsNullOrWhiteSpace(RefreshToken))
        {
            await RefreshAsync();
            await TrySendAsync(text);
        }
    }

    internal async Task RefreshAsync()
    {
        if (string.IsNullOrWhiteSpace(RefreshToken) || string.IsNullOrWhiteSpace(RestApiKey)) return;

        var content = new FormUrlEncodedContent([
            new("grant_type",    "refresh_token"),
            new("client_id",     RestApiKey),
            new("refresh_token", RefreshToken),
        ]);

        try
        {
            var response = await _http.PostAsync("https://kauth.kakao.com/oauth/token", content);
            ParseTokenResponse(await response.Content.ReadAsStringAsync());
        }
        catch { }
    }

    private async Task<bool> TrySendAsync(string text)
    {
        if (string.IsNullOrWhiteSpace(AccessToken)) return false;

        var templateObject = JsonSerializer.Serialize(new
        {
            object_type = "text",
            text        = text,
            link        = new { web_url = "https://developers.kakao.com" }
        });

        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://kapi.kakao.com/v2/api/talk/memo/default/send");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        request.Content = new FormUrlEncodedContent([
            new("template_object", templateObject)
        ]);

        try
        {
            var response = await _http.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                AccessToken = string.Empty;
                return false;
            }
            return response.IsSuccessStatusCode;
        }
        catch { return false; }
    }

    private bool ParseTokenResponse(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("access_token", out var at))
                AccessToken = at.GetString() ?? string.Empty;
            if (doc.RootElement.TryGetProperty("refresh_token", out var rt))
                RefreshToken = rt.GetString() ?? string.Empty;
            return !string.IsNullOrEmpty(AccessToken);
        }
        catch { return false; }
    }

    public void Dispose() => _http.Dispose();
}
