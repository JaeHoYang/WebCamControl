namespace WebCamControl;

internal partial class TelegramSettingsForm : Form
{
    internal string BotToken => txtToken.Text.Trim();
    internal string ChatId   => txtChatId.Text.Trim();

    internal TelegramSettingsForm(string currentToken, string currentChatId)
    {
        InitializeComponent();
        txtToken.Text  = currentToken;
        txtChatId.Text = currentChatId;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtToken.Text) || string.IsNullOrWhiteSpace(txtChatId.Text))
        {
            MessageBox.Show("Bot Token과 Chat ID를 모두 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnToggleChatId_Click(object? sender, EventArgs e)
    {
        bool masked = txtChatId.PasswordChar == '*';
        txtChatId.PasswordChar  = masked ? '\0' : '*';
        btnToggleChatId.Text    = masked ? "🙈" : "👁";
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void BtnTest_Click(object? sender, EventArgs e)
    {
        var token  = txtToken.Text.Trim();
        var chatId = txtChatId.Text.Trim();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(chatId))
        {
            MessageBox.Show("Bot Token과 Chat ID를 입력해주세요.", "입력 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTest.Enabled = false;
        btnTest.Text    = "전송 중...";

        var notifier = new TelegramNotifier { BotToken = token, ChatId = chatId };
        notifier.SendAsync("✅ WebCam Monitor 테스트 메시지").ContinueWith(_ =>
        {
            Invoke(() =>
            {
                notifier.Dispose();
                btnTest.Enabled = true;
                btnTest.Text    = "테스트 전송";
                MessageBox.Show("전송했습니다. 텔레그램을 확인해주세요.", "테스트",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            });
        });
    }
}
