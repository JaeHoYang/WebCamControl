#nullable enable
namespace WebCamControl;

partial class NotificationSettingsForm
{
    private System.ComponentModel.IContainer? components = null;

    // Tab
    private TabControl tabControl       = null!;
    private TabPage    tabTelegram      = null!;
    private TabPage    tabDiscord       = null!;
    private TabPage    tabKakao         = null!;

    // 텔레그램 탭
    private CheckBox chkTelegramEnabled = null!;
    private Label    lblToken           = null!;
    private TextBox  txtToken           = null!;
    private Label    lblChatId          = null!;
    private TextBox  txtChatId          = null!;
    private Button   btnToggleChatId    = null!;
    private Button   btnTelegramTest    = null!;

    // Discord 탭
    private CheckBox chkDiscordEnabled = null!;
    private Label    lblWebhookUrl     = null!;
    private TextBox  txtWebhookUrl     = null!;
    private Button   btnDiscordTest    = null!;

    // 카카오톡 탭
    private CheckBox chkKakaoEnabled  = null!;
    private Label    lblKakaoKey      = null!;
    private TextBox  txtKakaoKey      = null!;
    private Button   btnKakaoLogin    = null!;
    private Label    lblKakaoStatus   = null!;
    private Button   btnKakaoTest     = null!;

    // 하단 버튼
    private Button btnOk     = null!;
    private Button btnCancel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tabControl         = new TabControl();
        tabTelegram        = new TabPage();
        tabDiscord         = new TabPage();
        tabKakao           = new TabPage();
        chkTelegramEnabled = new CheckBox();
        lblToken           = new Label();
        txtToken           = new TextBox();
        lblChatId          = new Label();
        txtChatId          = new TextBox();
        btnToggleChatId    = new Button();
        btnTelegramTest    = new Button();
        chkDiscordEnabled  = new CheckBox();
        lblWebhookUrl      = new Label();
        txtWebhookUrl      = new TextBox();
        btnDiscordTest     = new Button();
        chkKakaoEnabled    = new CheckBox();
        lblKakaoKey        = new Label();
        txtKakaoKey        = new TextBox();
        btnKakaoLogin      = new Button();
        lblKakaoStatus     = new Label();
        btnKakaoTest       = new Button();
        btnOk              = new Button();
        btnCancel          = new Button();
        SuspendLayout();

        // ── 텔레그램 탭 컨트롤 ────────────────────────────────────────

        chkTelegramEnabled.Text      = "텔레그램 알림 활성화";
        chkTelegramEnabled.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        chkTelegramEnabled.Location  = new Point(8, 10);
        chkTelegramEnabled.Size      = new Size(270, 22);

        lblToken.Text     = "Bot Token (@BotFather에서 발급):";
        lblToken.Font     = new Font("맑은 고딕", 9F);
        lblToken.Location = new Point(8, 40);
        lblToken.AutoSize = true;

        txtToken.Location = new Point(8, 60);
        txtToken.Size     = new Size(278, 23);
        txtToken.Font     = new Font("맑은 고딕", 9F);

        lblChatId.Text     = "Chat ID (@userinfobot 에 /start 로 확인):";
        lblChatId.Font     = new Font("맑은 고딕", 9F);
        lblChatId.Location = new Point(8, 94);
        lblChatId.AutoSize = true;

        txtChatId.Location     = new Point(8, 114);
        txtChatId.Size         = new Size(248, 23);
        txtChatId.Font         = new Font("맑은 고딕", 9F);
        txtChatId.PasswordChar = '*';

        btnToggleChatId.Location  = new Point(260, 112);
        btnToggleChatId.Size      = new Size(26, 26);
        btnToggleChatId.Font      = new Font("Segoe UI Emoji", 11F);
        btnToggleChatId.Text      = "👁";
        btnToggleChatId.FlatStyle = FlatStyle.Flat;
        btnToggleChatId.FlatAppearance.BorderSize = 0;
        btnToggleChatId.Cursor    = Cursors.Hand;
        btnToggleChatId.Click    += new EventHandler(BtnToggleChatId_Click);

        btnTelegramTest.Location  = new Point(8, 152);
        btnTelegramTest.Size      = new Size(278, 30);
        btnTelegramTest.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnTelegramTest.Text      = "테스트 전송";
        btnTelegramTest.ForeColor = Color.DarkSlateBlue;
        btnTelegramTest.Click    += new EventHandler(BtnTelegramTest_Click);

        tabTelegram.Text = "텔레그램";
        tabTelegram.Font = new Font("맑은 고딕", 9F);
        tabTelegram.Controls.Add(chkTelegramEnabled);
        tabTelegram.Controls.Add(lblToken);
        tabTelegram.Controls.Add(txtToken);
        tabTelegram.Controls.Add(lblChatId);
        tabTelegram.Controls.Add(txtChatId);
        tabTelegram.Controls.Add(btnToggleChatId);
        tabTelegram.Controls.Add(btnTelegramTest);

        // ── Discord 탭 컨트롤 ────────────────────────────────────────

        chkDiscordEnabled.Text      = "Discord 알림 활성화";
        chkDiscordEnabled.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        chkDiscordEnabled.Location  = new Point(8, 10);
        chkDiscordEnabled.Size      = new Size(270, 22);

        lblWebhookUrl.Text     = "Webhook URL (Discord 채널 설정에서 발급):";
        lblWebhookUrl.Font     = new Font("맑은 고딕", 9F);
        lblWebhookUrl.Location = new Point(8, 40);
        lblWebhookUrl.AutoSize = true;

        txtWebhookUrl.Location  = new Point(8, 60);
        txtWebhookUrl.Size      = new Size(278, 23);
        txtWebhookUrl.Font      = new Font("맑은 고딕", 9F);

        btnDiscordTest.Location  = new Point(8, 100);
        btnDiscordTest.Size      = new Size(278, 30);
        btnDiscordTest.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnDiscordTest.Text      = "테스트 전송";
        btnDiscordTest.ForeColor = Color.DarkSlateBlue;
        btnDiscordTest.Click    += new EventHandler(BtnDiscordTest_Click);

        tabDiscord.Text = "디스코드";
        tabDiscord.Font = new Font("맑은 고딕", 9F);
        tabDiscord.Controls.Add(chkDiscordEnabled);
        tabDiscord.Controls.Add(lblWebhookUrl);
        tabDiscord.Controls.Add(txtWebhookUrl);
        tabDiscord.Controls.Add(btnDiscordTest);

        // ── 카카오톡 탭 컨트롤 ───────────────────────────────────────

        chkKakaoEnabled.Text     = "카카오톡 알림 활성화";
        chkKakaoEnabled.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        chkKakaoEnabled.Location = new Point(8, 10);
        chkKakaoEnabled.Size     = new Size(270, 22);

        lblKakaoKey.Text     = "REST API 키 (developers.kakao.com):";
        lblKakaoKey.Font     = new Font("맑은 고딕", 9F);
        lblKakaoKey.Location = new Point(8, 40);
        lblKakaoKey.AutoSize = true;

        txtKakaoKey.Location = new Point(8, 60);
        txtKakaoKey.Size     = new Size(278, 23);
        txtKakaoKey.Font     = new Font("맑은 고딕", 9F);

        btnKakaoLogin.Location  = new Point(8, 94);
        btnKakaoLogin.Size      = new Size(278, 30);
        btnKakaoLogin.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnKakaoLogin.Text      = "카카오 계정으로 로그인";
        btnKakaoLogin.ForeColor = Color.DarkGoldenrod;
        btnKakaoLogin.Click    += new EventHandler(BtnKakaoLogin_Click);

        lblKakaoStatus.Location  = new Point(8, 132);
        lblKakaoStatus.Size      = new Size(278, 18);
        lblKakaoStatus.Font      = new Font("맑은 고딕", 8.5F);
        lblKakaoStatus.ForeColor = Color.Gray;
        lblKakaoStatus.Text      = "상태: 로그인 필요";

        btnKakaoTest.Location  = new Point(8, 152);
        btnKakaoTest.Size      = new Size(278, 30);
        btnKakaoTest.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnKakaoTest.Text      = "테스트 전송";
        btnKakaoTest.ForeColor = Color.DarkSlateBlue;
        btnKakaoTest.Click    += new EventHandler(BtnKakaoTest_Click);

        tabKakao.Text = "카카오톡";
        tabKakao.Font = new Font("맑은 고딕", 9F);
        tabKakao.Controls.Add(chkKakaoEnabled);
        tabKakao.Controls.Add(lblKakaoKey);
        tabKakao.Controls.Add(txtKakaoKey);
        tabKakao.Controls.Add(btnKakaoLogin);
        tabKakao.Controls.Add(lblKakaoStatus);
        tabKakao.Controls.Add(btnKakaoTest);

        // ── TabControl ────────────────────────────────────────────────

        tabControl.Location = new Point(12, 12);
        tabControl.Size     = new Size(296, 228);
        tabControl.Font     = new Font("맑은 고딕", 9F);
        tabControl.TabPages.Add(tabTelegram);
        tabControl.TabPages.Add(tabDiscord);
        // tabKakao: 준비 중 — Kakao REST API 설정 복잡도로 비활성화

        // ── 하단 버튼 ────────────────────────────────────────────────

        btnOk.Location  = new Point(12, 252);
        btnOk.Size      = new Size(136, 32);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        btnCancel.Location  = new Point(160, 252);
        btnCancel.Size      = new Size(136, 32);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        // ── Form ─────────────────────────────────────────────────────

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(320, 296);
        Controls.Add(tabControl);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "알림 설정";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
