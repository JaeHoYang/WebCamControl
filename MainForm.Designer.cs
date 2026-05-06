#nullable enable
namespace WebCamControl;

partial class MainForm
{
    private System.ComponentModel.IContainer? components = null;

    // 기존 컨트롤
    private Label    lblCamera = null!;
    private ComboBox cmbCamera = null!;
    private Button   btnVideo  = null!;
    private Label    lblMic    = null!;
    private ComboBox cmbMic    = null!;
    private Button   btnMic    = null!;
    private Button   btnExit   = null!;
    private Label     lblAuthor  = null!;
    private Label     lblVersion = null!;
    private LinkLabel lnkHelp    = null!;

    // 감시 컨트롤
    private Label  lblMonitorSection        = null!;
    private Button btnMonitorToggle         = null!;
    private Button btnMonitorOptions        = null!;
    private Button btnNotificationSettings  = null!;
    private Label  lblMonitorStatus         = null!;

    // 시스템 트레이
    private NotifyIcon        notifyIcon      = null!;
    private ContextMenuStrip  trayContextMenu = null!;
    private ToolStripMenuItem trayMenuOpen    = null!;
    private ToolStripMenuItem trayMenuExit    = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblCamera                  = new Label();
        cmbCamera                  = new ComboBox();
        btnVideo                   = new Button();
        lblMic                     = new Label();
        cmbMic                     = new ComboBox();
        btnMic                     = new Button();
        btnExit                    = new Button();
        lblAuthor                  = new Label();
        lblVersion                 = new Label();
        lnkHelp                    = new LinkLabel();
        lblMonitorSection          = new Label();
        btnMonitorToggle           = new Button();
        btnMonitorOptions          = new Button();
        btnNotificationSettings    = new Button();
        lblMonitorStatus           = new Label();
        trayMenuOpen               = new ToolStripMenuItem();
        trayMenuExit               = new ToolStripMenuItem();
        trayContextMenu            = new ContextMenuStrip();
        notifyIcon                 = new NotifyIcon();
        SuspendLayout();

        // lblCamera
        lblCamera.AutoSize = true;
        lblCamera.Location = new Point(20, 20);
        lblCamera.Font     = new Font("맑은 고딕", 9F);
        lblCamera.Text     = "카메라:";

        // cmbCamera
        cmbCamera.Location      = new Point(78, 17);
        cmbCamera.Size          = new Size(202, 25);
        cmbCamera.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbCamera.Font          = new Font("맑은 고딕", 9F);
        cmbCamera.SelectedIndexChanged += new EventHandler(CmbCamera_SelectedIndexChanged);

        // btnVideo
        btnVideo.Location = new Point(20, 52);
        btnVideo.Size     = new Size(260, 36);
        btnVideo.TabIndex = 0;
        btnVideo.Font     = new Font("맑은 고딕", 10.5F, FontStyle.Bold);
        btnVideo.Click   += new EventHandler(BtnVideo_Click);

        // lblMic
        lblMic.AutoSize = true;
        lblMic.Location = new Point(20, 108);
        lblMic.Font     = new Font("맑은 고딕", 9F);
        lblMic.Text     = "마이크:";

        // cmbMic
        cmbMic.Location      = new Point(78, 105);
        cmbMic.Size          = new Size(202, 25);
        cmbMic.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMic.Font          = new Font("맑은 고딕", 9F);
        cmbMic.SelectedIndexChanged += new EventHandler(CmbMic_SelectedIndexChanged);

        // btnMic
        btnMic.Location = new Point(20, 140);
        btnMic.Size     = new Size(260, 36);
        btnMic.TabIndex = 1;
        btnMic.Font     = new Font("맑은 고딕", 10.5F, FontStyle.Bold);
        btnMic.Click   += new EventHandler(BtnMic_Click);

        // btnExit
        btnExit.Location  = new Point(20, 192);
        btnExit.Size      = new Size(260, 36);
        btnExit.TabIndex  = 2;
        btnExit.Font      = new Font("맑은 고딕", 10.5F, FontStyle.Bold);
        btnExit.Text      = "종료";
        btnExit.ForeColor = Color.Black;
        btnExit.Click    += new EventHandler(BtnExit_Click);

        // lblMonitorSection (구분선 역할)
        lblMonitorSection.Location  = new Point(20, 244);
        lblMonitorSection.Size      = new Size(260, 18);
        lblMonitorSection.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblMonitorSection.Text      = "─── 웹캠 감시 ───";
        lblMonitorSection.TextAlign = ContentAlignment.MiddleCenter;
        lblMonitorSection.ForeColor = Color.DimGray;

        // btnMonitorToggle
        btnMonitorToggle.Location  = new Point(20, 268);
        btnMonitorToggle.Size      = new Size(84, 32);
        btnMonitorToggle.TabIndex  = 3;
        btnMonitorToggle.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnMonitorToggle.Text      = "감시 시작";
        btnMonitorToggle.ForeColor = Color.Green;
        btnMonitorToggle.Click    += new EventHandler(BtnMonitorToggle_Click);

        // btnMonitorOptions
        btnMonitorOptions.Location  = new Point(108, 268);
        btnMonitorOptions.Size      = new Size(84, 32);
        btnMonitorOptions.TabIndex  = 4;
        btnMonitorOptions.Font      = new Font("맑은 고딕", 10.5F, FontStyle.Bold);
        btnMonitorOptions.Text      = "설정";
        btnMonitorOptions.ForeColor = Color.Black;
        btnMonitorOptions.Click    += new EventHandler(BtnMonitorOptions_Click);

        // btnNotificationSettings
        btnNotificationSettings.Location  = new Point(196, 268);
        btnNotificationSettings.Size      = new Size(84, 32);
        btnNotificationSettings.TabIndex  = 5;
        btnNotificationSettings.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnNotificationSettings.Text      = "알림 설정";
        btnNotificationSettings.ForeColor = Color.DarkSlateBlue;
        btnNotificationSettings.Click    += new EventHandler(BtnNotificationSettings_Click);

        // lblMonitorStatus
        lblMonitorStatus.Location  = new Point(20, 310);
        lblMonitorStatus.Size      = new Size(260, 18);
        lblMonitorStatus.Font      = new Font("맑은 고딕", 8.5F);
        lblMonitorStatus.Text      = "상태: 꺼짐";
        lblMonitorStatus.ForeColor = Color.Gray;

        // lblAuthor
        lblAuthor.Text      = "제작자: jaeho";
        lblAuthor.Location  = new Point(20, 342);
        lblAuthor.Size      = new Size(160, 18);
        lblAuthor.Font      = new Font("맑은 고딕", 8F, FontStyle.Italic);
        lblAuthor.ForeColor = Color.Gray;
        lblAuthor.TextAlign = ContentAlignment.MiddleLeft;

        // lblVersion
        lblVersion.Text      = "v1.04";
        lblVersion.Location  = new Point(20, 362);
        lblVersion.Size      = new Size(260, 16);
        lblVersion.Font      = new Font("맑은 고딕", 8F, FontStyle.Italic);
        lblVersion.ForeColor = Color.Gray;
        lblVersion.TextAlign = ContentAlignment.MiddleCenter;

        // lnkHelp
        lnkHelp.Text      = "도움말";
        lnkHelp.Location  = new Point(202, 342);
        lnkHelp.Size      = new Size(78, 18);
        lnkHelp.Font      = new Font("맑은 고딕", 8F);
        lnkHelp.TextAlign = ContentAlignment.MiddleRight;
        lnkHelp.LinkClicked += new LinkLabelLinkClickedEventHandler(LnkHelp_LinkClicked);

        // tray context menu
        trayMenuOpen.Text  = "활성화";
        trayMenuOpen.Font  = new Font("맑은 고딕", 9F, FontStyle.Bold);
        trayMenuOpen.Click += new EventHandler(TrayMenuOpen_Click);

        trayMenuExit.Text  = "종료";
        trayMenuExit.Font  = new Font("맑은 고딕", 9F);
        trayMenuExit.Click += new EventHandler(TrayMenuExit_Click);

        trayContextMenu.Items.Add(trayMenuOpen);
        trayContextMenu.Items.Add(new ToolStripSeparator());
        trayContextMenu.Items.Add(trayMenuExit);

        // notifyIcon
        notifyIcon.Text             = "WebCam Monitor";
        notifyIcon.Visible          = false;
        notifyIcon.ContextMenuStrip = trayContextMenu;
        notifyIcon.Icon             = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                                      ?? SystemIcons.Application;
        notifyIcon.DoubleClick += new EventHandler(NotifyIcon_DoubleClick);

        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(300, 388);
        Controls.Add(lblCamera);
        Controls.Add(cmbCamera);
        Controls.Add(btnVideo);
        Controls.Add(lblMic);
        Controls.Add(cmbMic);
        Controls.Add(btnMic);
        Controls.Add(btnExit);
        Controls.Add(lblMonitorSection);
        Controls.Add(btnMonitorToggle);
        Controls.Add(btnMonitorOptions);
        Controls.Add(btnNotificationSettings);
        Controls.Add(lblMonitorStatus);
        Controls.Add(lblAuthor);
        Controls.Add(lblVersion);
        Controls.Add(lnkHelp);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;
        MinimizeBox     = true;
        Name            = "MainForm";
        Text            = "WebCam Controller";
        StartPosition   = FormStartPosition.CenterScreen;
        Icon            = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                          ?? SystemIcons.Application;
        ResumeLayout(false);
    }
}
