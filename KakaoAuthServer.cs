using System.Net;
using System.Text;

namespace WebCamControl;

internal static class KakaoAuthServer
{
    internal const string RedirectUri = "http://localhost:9697/";

    /// <summary>
    /// 로컬 HTTP 서버를 시작하고 카카오 OAuth 콜백에서 code를 수신합니다.
    /// 브라우저 열기는 호출자가 담당합니다.
    /// </summary>
    internal static async Task<string?> WaitForCodeAsync(CancellationToken ct)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add(RedirectUri);

        try
        {
            listener.Start();
        }
        catch
        {
            return null; // 포트 충돌 등
        }

        try
        {
            var context = await listener.GetContextAsync().WaitAsync(ct);
            var code    = context.Request.QueryString["code"];

            byte[] body = Encoding.UTF8.GetBytes(
                "<html><body style='font-family:sans-serif;text-align:center;padding-top:60px'>" +
                "<h2>✅ 인증 완료</h2><p>이 창을 닫고 앱으로 돌아가세요.</p></body></html>");
            context.Response.ContentType     = "text/html; charset=utf-8";
            context.Response.ContentLength64 = body.Length;
            await context.Response.OutputStream.WriteAsync(body, ct);
            context.Response.Close();

            return code;
        }
        catch
        {
            return null;
        }
        finally
        {
            listener.Stop();
        }
    }
}
