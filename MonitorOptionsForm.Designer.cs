#nullable enable
namespace WebCamControl;

partial class MonitorOptionsForm
{
    private System.ComponentModel.IContainer? components = null;

    // 탭
    private TabControl tabControl    = null!;
    private TabPage    tabGeneral    = null!;
    private TabPage    tabRecording  = null!;

    // 일반 탭 — 감시
    private Label         lblSectionMonitor = null!;
    private Label         lblInterval       = null!;
    private NumericUpDown nudInterval       = null!;
    private Label         lblIntervalUnit   = null!;

    // 일반 탭 — 알림
    private Label    lblSectionNotify = null!;
    private CheckBox chkBalloonTips   = null!;
    private CheckBox chkStartStop     = null!;

    // 일반 탭 — 로그
    private Label    lblSectionLog    = null!;
    private CheckBox chkLogEnabled    = null!;
    private Label    lblLogPath       = null!;
    private Button   btnOpenFolder    = null!;
    private Button   btnViewTodayLog  = null!;

    // 화면 녹화 탭 — 화면
    private Label       lblSectionScreen   = null!;
    private CheckBox    chkRecordScreen    = null!;
    private Label       lblRecordMonitor   = null!;
    private ComboBox    cmbRecordMonitor   = null!;
    private Label       lblScreenQ         = null!;
    private RadioButton rdoScreenLow       = null!;
    private RadioButton rdoScreenMid       = null!;
    private RadioButton rdoScreenHigh      = null!;

    // 화면 녹화 탭 — 저장
    private Label  lblSectionSave       = null!;
    private Label  lblRecordPath        = null!;
    private Button btnOpenRecordFolder  = null!;

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
        tabControl          = new TabControl();
        tabGeneral          = new TabPage();
        tabRecording        = new TabPage();
        lblSectionMonitor   = new Label();
        lblInterval         = new Label();
        nudInterval         = new NumericUpDown();
        lblIntervalUnit     = new Label();
        lblSectionNotify    = new Label();
        chkBalloonTips      = new CheckBox();
        chkStartStop        = new CheckBox();
        lblSectionLog       = new Label();
        chkLogEnabled       = new CheckBox();
        lblLogPath          = new Label();
        btnOpenFolder       = new Button();
        btnViewTodayLog     = new Button();
        lblSectionScreen    = new Label();
        chkRecordScreen     = new CheckBox();
        lblRecordMonitor    = new Label();
        cmbRecordMonitor    = new ComboBox();
        lblScreenQ          = new Label();
        rdoScreenLow        = new RadioButton();
        rdoScreenMid        = new RadioButton();
        rdoScreenHigh       = new RadioButton();
        lblSectionSave      = new Label();
        lblRecordPath       = new Label();
        btnOpenRecordFolder = new Button();
        btnOk               = new Button();
        btnCancel           = new Button();
        SuspendLayout();

        // ── 일반 탭 ─────────────────────────────────────────────────────

        lblSectionMonitor.Text      = "── 감시 ──────────────────────────";
        lblSectionMonitor.Location  = new Point(8, 8);
        lblSectionMonitor.Size      = new Size(280, 16);
        lblSectionMonitor.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionMonitor.ForeColor = Color.DimGray;

        lblInterval.Text     = "감시 주기:";
        lblInterval.Font     = new Font("맑은 고딕", 9.5F);
        lblInterval.Location = new Point(8, 30);
        lblInterval.AutoSize = true;

        nudInterval.Location = new Point(92, 28);
        nudInterval.Size     = new Size(56, 25);
        nudInterval.Font     = new Font("맑은 고딕", 9.5F);
        nudInterval.Minimum  = 1;
        nudInterval.Maximum  = 10;
        nudInterval.Value    = 3;

        lblIntervalUnit.Text      = "초  (1 ~ 10)";
        lblIntervalUnit.Font      = new Font("맑은 고딕", 9F);
        lblIntervalUnit.Location  = new Point(154, 30);
        lblIntervalUnit.AutoSize  = true;
        lblIntervalUnit.ForeColor = Color.DimGray;

        lblSectionNotify.Text      = "── 알림 ──────────────────────────";
        lblSectionNotify.Location  = new Point(8, 62);
        lblSectionNotify.Size      = new Size(280, 16);
        lblSectionNotify.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionNotify.ForeColor = Color.DimGray;

        chkBalloonTips.Text     = "트레이 풍선 알림 표시";
        chkBalloonTips.Font     = new Font("맑은 고딕", 9.5F);
        chkBalloonTips.Location = new Point(8, 84);
        chkBalloonTips.Size     = new Size(280, 22);

        chkStartStop.Text     = "감시 시작·종료 알림 전송";
        chkStartStop.Font     = new Font("맑은 고딕", 9.5F);
        chkStartStop.Location = new Point(8, 108);
        chkStartStop.Size     = new Size(280, 22);

        lblSectionLog.Text      = "── 로그 ──────────────────────────";
        lblSectionLog.Location  = new Point(8, 140);
        lblSectionLog.Size      = new Size(280, 16);
        lblSectionLog.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionLog.ForeColor = Color.DimGray;

        chkLogEnabled.Text     = "날짜별 로그 저장";
        chkLogEnabled.Font     = new Font("맑은 고딕", 9.5F);
        chkLogEnabled.Location = new Point(8, 162);
        chkLogEnabled.Size     = new Size(280, 22);
        chkLogEnabled.CheckedChanged += new EventHandler(ChkLogEnabled_CheckedChanged);

        lblLogPath.Location  = new Point(8, 188);
        lblLogPath.Size      = new Size(280, 16);
        lblLogPath.Font      = new Font("맑은 고딕", 7.5F);
        lblLogPath.ForeColor = Color.Gray;

        btnOpenFolder.Location  = new Point(8, 208);
        btnOpenFolder.Size      = new Size(134, 28);
        btnOpenFolder.Font      = new Font("맑은 고딕", 9F);
        btnOpenFolder.Text      = "폴더 열기";
        btnOpenFolder.Click    += new EventHandler(BtnOpenFolder_Click);

        btnViewTodayLog.Location  = new Point(148, 208);
        btnViewTodayLog.Size      = new Size(140, 28);
        btnViewTodayLog.Font      = new Font("맑은 고딕", 9F);
        btnViewTodayLog.Text      = "오늘 로그 보기";
        btnViewTodayLog.ForeColor = Color.DarkSlateBlue;
        btnViewTodayLog.Click    += new EventHandler(BtnViewTodayLog_Click);

        tabGeneral.Text = "일반";
        tabGeneral.Font = new Font("맑은 고딕", 9F);
        tabGeneral.Controls.Add(lblSectionMonitor);
        tabGeneral.Controls.Add(lblInterval);
        tabGeneral.Controls.Add(nudInterval);
        tabGeneral.Controls.Add(lblIntervalUnit);
        tabGeneral.Controls.Add(lblSectionNotify);
        tabGeneral.Controls.Add(chkBalloonTips);
        tabGeneral.Controls.Add(chkStartStop);
        tabGeneral.Controls.Add(lblSectionLog);
        tabGeneral.Controls.Add(chkLogEnabled);
        tabGeneral.Controls.Add(lblLogPath);
        tabGeneral.Controls.Add(btnOpenFolder);
        tabGeneral.Controls.Add(btnViewTodayLog);

        // ── 화면 녹화 탭 ────────────────────────────────────────────────

        lblSectionScreen.Text      = "── 화면 녹화 ─────────────────────";
        lblSectionScreen.Location  = new Point(8, 8);
        lblSectionScreen.Size      = new Size(280, 16);
        lblSectionScreen.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionScreen.ForeColor = Color.DimGray;

        chkRecordScreen.Text     = "웹캠 감지 시 화면 자동 녹화";
        chkRecordScreen.Font     = new Font("맑은 고딕", 9.5F);
        chkRecordScreen.Location = new Point(8, 28);
        chkRecordScreen.Size     = new Size(280, 22);
        chkRecordScreen.CheckedChanged += new EventHandler(ChkRecordScreen_CheckedChanged);

        lblRecordMonitor.Text     = "녹화 대상:";
        lblRecordMonitor.Font     = new Font("맑은 고딕", 9F);
        lblRecordMonitor.Location = new Point(8, 56);
        lblRecordMonitor.AutoSize = true;

        cmbRecordMonitor.Location      = new Point(76, 53);
        cmbRecordMonitor.Size          = new Size(212, 25);
        cmbRecordMonitor.Font          = new Font("맑은 고딕", 9F);
        cmbRecordMonitor.DropDownStyle = ComboBoxStyle.DropDownList;

        lblScreenQ.Text     = "화질:";
        lblScreenQ.Font     = new Font("맑은 고딕", 9F);
        lblScreenQ.Location = new Point(8, 84);
        lblScreenQ.AutoSize = true;

        rdoScreenLow.Text     = "낮음 (5fps)";
        rdoScreenLow.Font     = new Font("맑은 고딕", 9F);
        rdoScreenLow.Location = new Point(52, 82);
        rdoScreenLow.Size     = new Size(82, 22);

        rdoScreenMid.Text     = "보통 (10fps)";
        rdoScreenMid.Font     = new Font("맑은 고딕", 9F);
        rdoScreenMid.Location = new Point(138, 82);
        rdoScreenMid.Size     = new Size(88, 22);
        rdoScreenMid.Checked  = true;

        rdoScreenHigh.Text     = "높음 (15fps)";
        rdoScreenHigh.Font     = new Font("맑은 고딕", 9F);
        rdoScreenHigh.Location = new Point(230, 82);
        rdoScreenHigh.Size     = new Size(58, 22);

        lblSectionSave.Text      = "── 저장 위치 ─────────────────────";
        lblSectionSave.Location  = new Point(8, 116);
        lblSectionSave.Size      = new Size(280, 16);
        lblSectionSave.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionSave.ForeColor = Color.DimGray;

        lblRecordPath.Location  = new Point(8, 136);
        lblRecordPath.Size      = new Size(280, 16);
        lblRecordPath.Font      = new Font("맑은 고딕", 7.5F);
        lblRecordPath.ForeColor = Color.Gray;
        lblRecordPath.Text      = ScreenRecorder.SaveDirectory;

        btnOpenRecordFolder.Location  = new Point(8, 156);
        btnOpenRecordFolder.Size      = new Size(134, 28);
        btnOpenRecordFolder.Font      = new Font("맑은 고딕", 9F);
        btnOpenRecordFolder.Text      = "폴더 열기";
        btnOpenRecordFolder.Click    += new EventHandler(BtnOpenRecordFolder_Click);

        tabRecording.Text = "화면 녹화";
        tabRecording.Font = new Font("맑은 고딕", 9F);
        tabRecording.Controls.Add(lblSectionScreen);
        tabRecording.Controls.Add(chkRecordScreen);
        tabRecording.Controls.Add(lblRecordMonitor);
        tabRecording.Controls.Add(cmbRecordMonitor);
        tabRecording.Controls.Add(lblScreenQ);
        tabRecording.Controls.Add(rdoScreenLow);
        tabRecording.Controls.Add(rdoScreenMid);
        tabRecording.Controls.Add(rdoScreenHigh);
        tabRecording.Controls.Add(lblSectionSave);
        tabRecording.Controls.Add(lblRecordPath);
        tabRecording.Controls.Add(btnOpenRecordFolder);

        // ── TabControl ────────────────────────────────────────────────

        tabControl.Location = new Point(8, 8);
        tabControl.Size     = new Size(300, 298);
        tabControl.Font     = new Font("맑은 고딕", 9F);
        tabControl.TabPages.Add(tabGeneral);
        tabControl.TabPages.Add(tabRecording);

        // ── 하단 버튼 ─────────────────────────────────────────────────

        btnOk.Location  = new Point(8, 316);
        btnOk.Size      = new Size(140, 32);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        btnCancel.Location  = new Point(160, 316);
        btnCancel.Size      = new Size(140, 32);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        // ── Form ──────────────────────────────────────────────────────

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(316, 360);
        Controls.Add(tabControl);
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
