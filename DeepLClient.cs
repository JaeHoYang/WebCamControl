using System.Text.Json;

namespace WebCamControl;

internal sealed class DeepLQuotaExceededException : Exception
{
    internal DeepLQuotaExceededException() : base("DeepL 무료 한도 초과") { }
}

internal sealed class DeepLClient : IDisposable
{
    private static readonly HttpClient _http = new();

    internal string ApiKey { get; set; } = string.Empty;

    private static readonly string[] Endpoints =
    {
        "https://api-free.deepl.com/v2",
        "https://api.deepl.com/v2",
    };

    // 성공한 엔드포인트를 캐시
    private string? _resolvedUrl;

    private HttpRequestMessage BuildPost(string baseUrl, string path, FormUrlEncodedContent body)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}{path}") { Content = body };
        req.Headers.TryAddWithoutValidation("Authorization", $"DeepL-Auth-Key {ApiKey}");
        return req;
    }

    private HttpRequestMessage BuildGet(string baseUrl, string path)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}{path}");
        req.Headers.TryAddWithoutValidation("Authorization", $"DeepL-Auth-Key {ApiKey}");
        return req;
    }

    private FormUrlEncodedContent BuildTranslateForm(string text, string sourceLang, string targetLang)
    {
        var form = new List<KeyValuePair<string, string>>
        {
            new("auth_key",    ApiKey),   // 구형 인증 방식 병행 (일부 키에서 헤더 인증 실패 시 대비)
            new("text",        text),
            new("target_lang", targetLang),
        };
        if (sourceLang != "auto")
            form.Add(new("source_lang", sourceLang));
        return new FormUrlEncodedContent(form);
    }

    internal async Task<string> TranslateAsync(string text, string sourceLang, string targetLang)
    {
        // 이미 성공한 엔드포인트가 있으면 바로 사용
        if (_resolvedUrl != null)
        {
            var r = await _http.SendAsync(BuildPost(_resolvedUrl, "/translate",
                BuildTranslateForm(text, sourceLang, targetLang)));
            if ((int)r.StatusCode == 456) throw new DeepLQuotaExceededException();
            r.EnsureSuccessStatusCode();
            return ParseTranslation(await r.Content.ReadAsStringAsync());
        }

        // 두 엔드포인트 순서대로 시도
        string lastError = string.Empty;
        foreach (var url in Endpoints)
        {
            var resp = await _http.SendAsync(BuildPost(url, "/translate",
                BuildTranslateForm(text, sourceLang, targetLang)));

            if (resp.IsSuccessStatusCode)
            {
                _resolvedUrl = url;
                return ParseTranslation(await resp.Content.ReadAsStringAsync());
            }

            if ((int)resp.StatusCode == 456) throw new DeepLQuotaExceededException();

            string body = await resp.Content.ReadAsStringAsync();
            lastError = $"{(int)resp.StatusCode} ({resp.ReasonPhrase}) — {body}";

            if ((int)resp.StatusCode != 403) break; // 403 외 오류는 재시도 불필요
        }

        throw new HttpRequestException($"DeepL 번역 실패: {lastError}");
    }

    internal async Task<(long used, long limit)> GetUsageAsync()
    {
        foreach (var url in Endpoints)
        {
            var resp = await _http.SendAsync(BuildGet(url, "/usage"));
            if (!resp.IsSuccessStatusCode) continue;

            using var doc = JsonDocument.Parse(await resp.Content.ReadAsStringAsync());
            long used  = doc.RootElement.GetProperty("character_count").GetInt64();
            long limit = doc.RootElement.GetProperty("character_limit").GetInt64();
            _resolvedUrl = url;
            return (used, limit);
        }

        throw new HttpRequestException("DeepL 사용량 조회 실패");
    }

    private static string ParseTranslation(string json)
    {
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement
            .GetProperty("translations")[0]
            .GetProperty("text")
            .GetString() ?? string.Empty;
    }

    public void Dispose() { }
}
