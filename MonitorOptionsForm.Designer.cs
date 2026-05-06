#nullable enable
namespace WebCamControl;

partial class MonitorOptionsForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label         lblSectionMonitor = null!;
    private Label         lblInterval       = null!;
    private NumericUpDown nudInterval       = null!;
    private Label         lblIntervalUnit   = null!;

    private Label   lblSectionNotify = null!;
    private CheckBox chkBalloonTips  = null!;
    private CheckBox chkStartStop    = null!;

    private Label   lblSectionLog  = null!;
    private CheckBox chkLogEnabled = null!;
    private Label    lblLogPath    = null!;
    private Button   btnOpenFolder   = null!;
    private Button   btnViewTodayLog = null!;

    private Button btnOk     = null!;
    private Button btnCancel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblSectionMonitor = new Label();
        lblInterval       = new Label();
        nudInterval       = new NumericUpDown();
        lblIntervalUnit   = new Label();
        lblSectionNotify  = new Label();
        chkBalloonTips    = new CheckBox();
        chkStartStop      = new CheckBox();
        lblSectionLog     = new Label();
        chkLogEnabled     = new CheckBox();
        lblLogPath        = new Label();
        btnOpenFolder     = new Button();
        btnViewTodayLog   = new Button();
        btnOk             = new Button();
        btnCancel         = new Button();
        SuspendLayout();

        // ── 감시 섹션 ─────────────────────────────────────────────────

        lblSectionMonitor.Text      = "── 감시 ──────────────────────────";
        lblSectionMonitor.Location  = new Point(12, 12);
        lblSectionMonitor.Size      = new Size(290, 16);
        lblSectionMonitor.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionMonitor.ForeColor = Color.DimGray;

        lblInterval.Text     = "감시 주기:";
        lblInterval.Font     = new Font("맑은 고딕", 9.5F);
        lblInterval.Location = new Point(12, 36);
        lblInterval.AutoSize = true;

        nudInterval.Location  = new Point(96, 33);
        nudInterval.Size      = new Size(56, 25);
        nudInterval.Font      = new Font("맑은 고딕", 9.5F);
        nudInterval.Minimum   = 1;
        nudInterval.Maximum   = 10;
        nudInterval.Value     = 3;

        lblIntervalUnit.Text     = "초  (1 ~ 10)";
        lblIntervalUnit.Font     = new Font("맑은 고딕", 9F);
        lblIntervalUnit.Location = new Point(158, 36);
        lblIntervalUnit.AutoSize = true;
        lblIntervalUnit.ForeColor = Color.DimGray;

        // ── 알림 섹션 ─────────────────────────────────────────────────

        lblSectionNotify.Text      = "── 알림 ──────────────────────────";
        lblSectionNotify.Location  = new Point(12, 72);
        lblSectionNotify.Size      = new Size(290, 16);
        lblSectionNotify.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionNotify.ForeColor = Color.DimGray;

        chkBalloonTips.Text     = "트레이 풍선 알림 표시";
        chkBalloonTips.Font     = new Font("맑은 고딕", 9.5F);
        chkBalloonTips.Location = new Point(12, 96);
        chkBalloonTips.Size     = new Size(290, 22);

        chkStartStop.Text     = "감시 시작·종료 알림 전송";
        chkStartStop.Font     = new Font("맑은 고딕", 9.5F);
        chkStartStop.Location = new Point(12, 122);
        chkStartStop.Size     = new Size(290, 22);

        // ── 로그 섹션 ─────────────────────────────────────────────────

        lblSectionLog.Text      = "── 로그 ──────────────────────────";
        lblSectionLog.Location  = new Point(12, 158);
        lblSectionLog.Size      = new Size(290, 16);
        lblSectionLog.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionLog.ForeColor = Color.DimGray;

        chkLogEnabled.Text     = "날짜별 로그 저장";
        chkLogEnabled.Font     = new Font("맑은 고딕", 9.5F);
        chkLogEnabled.Location = new Point(12, 182);
        chkLogEnabled.Size     = new Size(290, 22);
        chkLogEnabled.CheckedChanged += new EventHandler(ChkLogEnabled_CheckedChanged);

        lblLogPath.Location  = new Point(12, 210);
        lblLogPath.Size      = new Size(290, 16);
        lblLogPath.Font      = new Font("맑은 고딕", 7.5F);
        lblLogPath.ForeColor = Color.Gray;
        lblLogPath.Text      = "";

        btnOpenFolder.Location  = new Point(12, 232);
        btnOpenFolder.Size      = new Size(138, 28);
        btnOpenFolder.Font      = new Font("맑은 고딕", 9F);
        btnOpenFolder.Text      = "폴더 열기";
        btnOpenFolder.Click    += new EventHandler(BtnOpenFolder_Click);

        btnViewTodayLog.Location  = new Point(156, 232);
        btnViewTodayLog.Size      = new Size(146, 28);
        btnViewTodayLog.Font      = new Font("맑은 고딕", 9F);
        btnViewTodayLog.Text      = "오늘 로그 보기";
        btnViewTodayLog.ForeColor = Color.DarkSlateBlue;
        btnViewTodayLog.Click    += new EventHandler(BtnViewTodayLog_Click);

        // ── 하단 버튼 ─────────────────────────────────────────────────

        btnOk.Location  = new Point(12, 278);
        btnOk.Size      = new Size(140, 32);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        btnCancel.Location  = new Point(162, 278);
        btnCancel.Size      = new Size(140, 32);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        // ── Form ──────────────────────────────────────────────────────

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(316, 322);
        Controls.Add(lblSectionMonitor);
        Controls.Add(lblInterval);
        Controls.Add(nudInterval);
        Controls.Add(lblIntervalUnit);
        Controls.Add(lblSectionNotify);
        Controls.Add(chkBalloonTips);
        Controls.Add(chkStartStop);
        Controls.Add(lblSectionLog);
        Controls.Add(chkLogEnabled);
        Controls.Add(lblLogPath);
        Controls.Add(btnOpenFolder);
        Controls.Add(btnViewTodayLog);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "설정";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
