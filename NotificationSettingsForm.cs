namespace WebCamControl;

internal partial class NotificationSettingsForm : Form
{
    private readonly MonitorSettings  _settings;
    private readonly DiscordNotifier  _discord;

    internal NotificationSettingsForm(MonitorSettings settings, DiscordNotifier discord)
    {
        InitializeComponent();
        _settings = settings;
        _discord  = discord;

        chkTelegramEnabled.Checked = settings.TelegramEnabled;
        txtToken.Text              = settings.BotToken;
        txtChatId.Text             = settings.ChatId;

        chkDiscordEnabled.Checked = settings.DiscordEnabled;
        txtWebhookUrl.Text        = settings.DiscordWebhookUrl;
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

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        _settings.TelegramEnabled   = chkTelegramEnabled.Checked;
        _settings.BotToken          = txtToken.Text.Trim();
        _settings.ChatId            = txtChatId.Text.Trim();
        _settings.DiscordEnabled    = chkDiscordEnabled.Checked;
        _settings.DiscordWebhookUrl = txtWebhookUrl.Text.Trim();
        _discord.WebhookUrl         = _settings.DiscordWebhookUrl;
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
