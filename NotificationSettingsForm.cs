using System.Diagnostics;

namespace WebCamControl;

internal partial class NotificationSettingsForm : Form
{
    private readonly MonitorSettings _settings;
    private readonly KakaoNotifier   _kakao;

    internal NotificationSettingsForm(MonitorSettings settings, KakaoNotifier kakao)
    {
        InitializeComponent();
        _settings = settings;
        _kakao    = kakao;

        chkTelegramEnabled.Checked = settings.TelegramEnabled;
        txtToken.Text              = settings.BotToken;
        txtChatId.Text             = settings.ChatId;

        chkKakaoEnabled.Checked = settings.KakaoEnabled;
        txtRestApiKey.Text       = settings.KakaoRestApiKey;
        UpdateKakaoStatus();
    }

    private void UpdateKakaoStatus()
    {
        bool authed = !string.IsNullOrEmpty(_settings.KakaoAccessToken) ||
                      !string.IsNullOrEmpty(_settings.KakaoRefreshToken);
        lblKakaoStatus.Text      = authed ? "인증 상태: ✅ 인증 완료" : "인증 상태: ❌ 미인증";
        lblKakaoStatus.ForeColor = authed ? Color.DarkGreen : Color.Red;
        btnKakaoTest.Enabled     = authed;
    }

    private void BtnToggleChatId_Click(object? sender, EventArgs e)
    {
        bool masked = txtChatId.PasswordChar == '*';
        txtChatId.PasswordChar = masked ? '\0' : '*';
        btnToggleChatId.Text   = masked ? "🙈" : "👁";
    }

    private void BtnTelegramTest_Click(object? sender, EventArgs e)
    {
        var token  = txtToken.Text.Trim();
        var chatId = txtChatId.Text.Trim();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(chatId))
        {
            MessageBox.Show("Bot Token과 Chat ID를 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTelegramTest.Enabled = false;
        btnTelegramTest.Text    = "전송 중...";

        var notifier = new TelegramNotifier { BotToken = token, ChatId = chatId };
        notifier.SendAsync("✅ WebCam Monitor 테스트 메시지").ContinueWith(_ =>
        {
            Invoke(() =>
            {
                notifier.Dispose();
                btnTelegramTest.Enabled = true;
                btnTelegramTest.Text    = "테스트 전송";
                MessageBox.Show("전송했습니다. 텔레그램을 확인해주세요.", "테스트",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        });
    }

    private void BtnKakaoLogin_Click(object? sender, EventArgs e)
    {
        var apiKey = txtRestApiKey.Text.Trim();
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            MessageBox.Show("REST API 키를 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _kakao.RestApiKey     = apiKey;
        btnKakaoLogin.Enabled = false;
        btnKakaoLogin.Text    = "로그인 중...";

        Task.Run(async () =>
        {
            var authUrl = KakaoNotifier.BuildAuthUrl(apiKey);
            Process.Start(new ProcessStartInfo { FileName = authUrl, UseShellExecute = true });

            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            var code = await KakaoAuthServer.WaitForCodeAsync(cts.Token);

            if (string.IsNullOrEmpty(code))
            {
                Invoke(() =>
                {
                    btnKakaoLogin.Enabled = true;
                    btnKakaoLogin.Text    = "카카오 로그인";
                    MessageBox.Show("인증에 실패했습니다. 다시 시도해주세요.", "오류",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
                return;
            }

            bool ok = await _kakao.ExchangeCodeAsync(code);

            Invoke(() =>
            {
                btnKakaoLogin.Enabled = true;
                btnKakaoLogin.Text    = "카카오 로그인";

                if (ok)
                {
                    _settings.KakaoRestApiKey   = apiKey;
                    _settings.KakaoAccessToken  = _kakao.AccessToken;
                    _settings.KakaoRefreshToken = _kakao.RefreshToken;
                    _settings.Save();
                    UpdateKakaoStatus();
                    MessageBox.Show("카카오 인증이 완료되었습니다!", "인증 완료",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        "토큰 교환에 실패했습니다.\n" +
                        "REST API 키와 Redirect URI 등록(http://localhost:9697/)을 확인해주세요.",
                        "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        });
    }

    private void BtnKakaoTest_Click(object? sender, EventArgs e)
    {
        btnKakaoTest.Enabled = false;
        btnKakaoTest.Text    = "전송 중...";

        _kakao.SendAsync("✅ WebCam Monitor 카카오톡 테스트 메시지").ContinueWith(_ =>
        {
            Invoke(() =>
            {
                _settings.KakaoAccessToken  = _kakao.AccessToken;
                _settings.KakaoRefreshToken = _kakao.RefreshToken;
                _settings.Save();

                btnKakaoTest.Enabled = true;
                btnKakaoTest.Text    = "테스트 전송";
                MessageBox.Show("전송했습니다. 카카오톡을 확인해주세요.", "테스트",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        });
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        _settings.TelegramEnabled = chkTelegramEnabled.Checked;
        _settings.BotToken        = txtToken.Text.Trim();
        _settings.ChatId          = txtChatId.Text.Trim();
        _settings.KakaoEnabled    = chkKakaoEnabled.Checked;
        _settings.KakaoRestApiKey = txtRestApiKey.Text.Trim();
        _settings.Save();

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
