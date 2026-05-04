#nullable enable
namespace WebCamControl;

partial class TelegramSettingsForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label   lblToken      = null!;
    private TextBox txtToken      = null!;
    private Label   lblChatId     = null!;
    private TextBox txtChatId     = null!;
    private Button  btnToggleChatId = null!;
    private Button  btnTest       = null!;
    private Button  btnOk         = null!;
    private Button  btnCancel     = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblToken        = new Label();
        txtToken        = new TextBox();
        lblChatId       = new Label();
        txtChatId       = new TextBox();
        btnToggleChatId = new Button();
        btnTest         = new Button();
        btnOk           = new Button();
        btnCancel       = new Button();
        SuspendLayout();

        // lblToken
        lblToken.Location = new Point(12, 16);
        lblToken.Size     = new Size(276, 18);
        lblToken.Font     = new Font("맑은 고딕", 9F);
        lblToken.Text     = "Bot Token (@BotFather에서 발급):";

        // txtToken
        txtToken.Location = new Point(12, 38);
        txtToken.Size     = new Size(276, 23);
        txtToken.Font     = new Font("맑은 고딕", 9F);

        // lblChatId
        lblChatId.Location = new Point(12, 76);
        lblChatId.Size     = new Size(276, 18);
        lblChatId.Font     = new Font("맑은 고딕", 9F);
        lblChatId.Text     = "Chat ID (@userinfobot 에 /start 로 확인):";

        // txtChatId — 기본값 * 마스킹
        txtChatId.Location     = new Point(12, 98);
        txtChatId.Size         = new Size(246, 23);
        txtChatId.Font         = new Font("맑은 고딕", 9F);
        txtChatId.PasswordChar = '*';

        // btnToggleChatId — 눈 모양 토글
        btnToggleChatId.Location  = new Point(262, 96);
        btnToggleChatId.Size      = new Size(26, 26);
        btnToggleChatId.Font      = new Font("Segoe UI Emoji", 11F);
        btnToggleChatId.Text      = "👁";
        btnToggleChatId.FlatStyle = FlatStyle.Flat;
        btnToggleChatId.FlatAppearance.BorderSize = 0;
        btnToggleChatId.Cursor    = Cursors.Hand;
        btnToggleChatId.Click    += new EventHandler(BtnToggleChatId_Click);

        // btnTest
        btnTest.Location  = new Point(12, 138);
        btnTest.Size      = new Size(276, 30);
        btnTest.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnTest.Text      = "테스트 전송";
        btnTest.ForeColor = Color.DarkSlateBlue;
        btnTest.Click    += new EventHandler(BtnTest_Click);

        // btnOk
        btnOk.Location  = new Point(12, 180);
        btnOk.Size      = new Size(132, 30);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        // btnCancel
        btnCancel.Location  = new Point(156, 180);
        btnCancel.Size      = new Size(132, 30);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(300, 226);
        Controls.Add(lblToken);
        Controls.Add(txtToken);
        Controls.Add(lblChatId);
        Controls.Add(txtChatId);
        Controls.Add(btnToggleChatId);
        Controls.Add(btnTest);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "텔레그램 설정";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
