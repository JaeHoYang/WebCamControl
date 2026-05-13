namespace WebCamControl;

internal partial class NotificationSettingsForm : Form
{
    private readonly MonitorSettings  _settings;
    private readonly DiscordNotifier  _discord;
    private readonly KakaoNotifier    _kakao;

    internal NotificationSettingsForm(MonitorSettings settings, DiscordNotifier discord, KakaoNotifier kakao)
    {
        InitializeComponent();
        _settings = settings;
        _discord  = discord;
        _kakao    = kakao;

        chkTelegramEnabled.Checked = settings.TelegramEnabled;
        txtToken.Text              = settings.BotToken;
        txtChatId.Text             = settings.ChatId;

        chkDiscordEnabled.Checked = settings.DiscordEnabled;
        txtWebhookUrl.Text        = settings.DiscordWebhookUrl;

        chkKakaoEnabled.Checked = settings.KakaoEnabled;
        txtKakaoKey.Text        = settings.KakaoAppKey;
        UpdateKakaoStatus();
    }

    private void UpdateKakaoStatus()
    {
        bool loggedIn = !string.IsNullOrWhiteSpace(_kakao.AccessToken) ||
                        !string.IsNullOrWhiteSpace(_kakao.RefreshToken);
        lblKakaoStatus.Text      = loggedIn ? "상태: 로그인됨 ✅" : "상태: 로그인 필요";
        lblKakaoStatus.ForeColor = loggedIn ? Color.DarkGreen : Color.Gray;
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

    private void BtnDiscordTest_Click(object? sender, EventArgs e)
    {
        var url = txtWebhookUrl.Text.Trim();

        if (string.IsNullOrWhiteSpace(url))
        {
            MessageBox.Show("Webhook URL을 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnDiscordTest.Enabled = false;
        btnDiscordTest.Text    = "전송 중...";

        var notifier = new DiscordNotifier { WebhookUrl = url };
        notifier.SendAsync("✅ WebCam Monitor Discord 테스트 메시지").ContinueWith(_ =>
        {
            Invoke(() =>
            {
                notifier.Dispose();
                btnDiscordTest.Enabled = true;
                btnDiscordTest.Text    = "테스트 전송";
                MessageBox.Show("전송했습니다. Discord 채널을 확인해주세요.", "테스트",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        });
    }

    private async void BtnKakaoLogin_Click(object? sender, EventArgs e)
    {
        var appKey = txtKakaoKey.Text.Trim();
        if (string.IsNullOrWhiteSpace(appKey))
        {
            MessageBox.Show("REST API 키를 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnKakaoLogin.Enabled = false;
        btnKakaoLogin.Text    = "로그인 중...";
        _kakao.RestApiKey     = appKey;

        try
        {
            // 1. 로컬 서버 대기 시작 (비동기 — 아직 await 하지 않음)
            var waitCodeTask = KakaoAuthServer.WaitForCodeAsync(CancellationToken.None);

            // 2. 브라우저 열기
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
                KakaoNotifier.BuildAuthUrl(appKey)) { UseShellExecute = true });

            // 3. OAuth 콜백 코드 수신 대기
            string? code = await waitCodeTask;
            if (string.IsNullOrEmpty(code))
                throw new InvalidOperationException("인증 코드를 받지 못했습니다.");

            // 4. 코드 → 토큰 교환
            bool ok = await _kakao.ExchangeCodeAsync(code);
            if (!ok)
                throw new InvalidOperationException("토큰 교환에 실패했습니다. API 키와 redirect URI를 확인해주세요.");

            UpdateKakaoStatus();
            MessageBox.Show("로그인 성공! 카카오톡 알림을 사용할 수 있습니다.", "로그인 완료",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"로그인 실패: {ex.Message}", "오류",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnKakaoLogin.Enabled = true;
            btnKakaoLogin.Text    = "카카오 계정으로 로그인";
        }
    }

    private void BtnKakaoTest_Click(object? sender, EventArgs e)
    {
        if (!_kakao.IsConfigured && string.IsNullOrWhiteSpace(_kakao.AccessToken))
        {
            MessageBox.Show("먼저 카카오 계정으로 로그인해주세요.", "로그인 필요",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnKakaoTest.Enabled = false;
        btnKakaoTest.Text    = "전송 중...";

        _kakao.SendAsync("✅ WebCam Monitor 카카오톡 테스트 메시지").ContinueWith(_ =>
        {
            Invoke(() =>
            {
                btnKakaoTest.Enabled = true;
                btnKakaoTest.Text    = "테스트 전송";
                MessageBox.Show("전송했습니다. 카카오톡 나챗방을 확인해주세요.", "테스트",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        });
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        _settings.TelegramEnabled   = chkTelegramEnabled.Checked;
        _settings.BotToken          = txtToken.Text.Trim();
        _settings.ChatId            = txtChatId.Text.Trim();
        _settings.DiscordEnabled    = chkDiscordEnabled.Checked;
        _settings.DiscordWebhookUrl = txtWebhookUrl.Text.Trim();
        _discord.WebhookUrl         = _settings.DiscordWebhookUrl;
        _settings.KakaoEnabled      = chkKakaoEnabled.Checked;
        _settings.KakaoAppKey       = txtKakaoKey.Text.Trim();
        _settings.KakaoAccessToken  = _kakao.AccessToken;
        _settings.KakaoRefreshToken = _kakao.RefreshToken;
        _kakao.IsEnabled            = _settings.KakaoEnabled;
        _kakao.RestApiKey           = _settings.KakaoAppKey;
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
