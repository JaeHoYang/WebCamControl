#nullable enable
namespace WebCamControl;

partial class MonitorOptionsForm
{
    private System.ComponentModel.IContainer? components = null;

    // 탭
    private TabControl tabControl    = null!;
    private TabPage    tabGeneral    = null!;
    private TabPage    tabRecording  = null!;
    private TabPage    tabTranslation = null!;

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

    // 번역 탭
    private CheckBox  chkTransEnabled = null!;
    private Label     lblSecVosk      = null!;
    private ListView  lstVoskModels   = null!;
    private Button    btnAddModel     = null!;
    private Button    btnRemoveModel  = null!;
    private LinkLabel lnkVoskDl       = null!;
    private Label     lblSecDeepL     = null!;
    private CheckBox  chkDeepLOn      = null!;
    private TextBox   txtDeepLKey     = null!;
    private Button    btnShowKey      = null!;
    private LinkLabel lnkDeepLGet     = null!;
    private Label     lblSrcLang      = null!;
    private ComboBox  cmbSrcLang      = null!;
    private Label     lblTgtLang      = null!;
    private ComboBox  cmbTgtLang      = null!;
    private CheckBox  chkShowOrig     = null!;
    private Label     lblUsage        = null!;
    private Button    btnCheckUsage   = null!;
    private CheckBox  chkLogTrans          = null!;
    private Button    btnOpenSubtitleFolder = null!;

    // 일반 탭 — 저장 공간 경고
    private Label         lblSectionStorage = null!;
    private CheckBox      chkStorageWarning = null!;
    private Label         lblLogWarn        = null!;
    private NumericUpDown nudLogWarnMB      = null!;
    private Label         lblLogWarnUnit    = null!;
    private Label         lblRecWarn        = null!;
    private NumericUpDown nudRecWarnGB      = null!;
    private Label         lblRecWarnUnit    = null!;
    private Label         lblCurrentUsage   = null!;

    // 일반 탭 — 저장 폴더
    private Label   lblSectionDataRoot = null!;
    private TextBox txtDataRootParent  = null!;
    private Button  btnBrowseDataRoot  = null!;
    private Button  btnResetDataRoot   = null!;
    private Label   lblDataRootPreview = null!;

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
        tabTranslation      = new TabPage();
        chkTransEnabled     = new CheckBox();
        lblSecVosk          = new Label();
        lstVoskModels       = new ListView();
        btnAddModel         = new Button();
        btnRemoveModel      = new Button();
        lnkVoskDl           = new LinkLabel();
        lblSecDeepL         = new Label();
        chkDeepLOn          = new CheckBox();
        txtDeepLKey         = new TextBox();
        btnShowKey          = new Button();
        lnkDeepLGet         = new LinkLabel();
        lblSrcLang          = new Label();
        cmbSrcLang          = new ComboBox();
        lblTgtLang          = new Label();
        cmbTgtLang          = new ComboBox();
        chkShowOrig         = new CheckBox();
        lblUsage            = new Label();
        btnCheckUsage       = new Button();
        chkLogTrans              = new CheckBox();
        btnOpenSubtitleFolder    = new Button();
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
        lblSectionStorage   = new Label();
        chkStorageWarning   = new CheckBox();
        lblLogWarn          = new Label();
        nudLogWarnMB        = new NumericUpDown();
        lblLogWarnUnit      = new Label();
        lblRecWarn          = new Label();
        nudRecWarnGB        = new NumericUpDown();
        lblRecWarnUnit      = new Label();
        lblCurrentUsage     = new Label();
        lblSectionDataRoot  = new Label();
        txtDataRootParent   = new TextBox();
        btnBrowseDataRoot   = new Button();
        btnResetDataRoot    = new Button();
        lblDataRootPreview  = new Label();
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

        lblSectionStorage.Text      = "── 저장 공간 경고 ────────────────";
        lblSectionStorage.Location  = new Point(8, 248);
        lblSectionStorage.Size      = new Size(280, 16);
        lblSectionStorage.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionStorage.ForeColor = Color.DimGray;

        chkStorageWarning.Text     = "용량 초과 시 경고 알림";
        chkStorageWarning.Font     = new Font("맑은 고딕", 9.5F);
        chkStorageWarning.Location = new Point(8, 270);
        chkStorageWarning.Size     = new Size(280, 22);
        chkStorageWarning.CheckedChanged += new EventHandler(ChkStorageWarning_CheckedChanged);

        lblLogWarn.Text     = "로그 임계값:";
        lblLogWarn.Font     = new Font("맑은 고딕", 9F);
        lblLogWarn.Location = new Point(8, 298);
        lblLogWarn.AutoSize = true;

        nudLogWarnMB.Location = new Point(86, 295);
        nudLogWarnMB.Size     = new Size(72, 25);
        nudLogWarnMB.Font     = new Font("맑은 고딕", 9F);
        nudLogWarnMB.Minimum  = 10;
        nudLogWarnMB.Maximum  = 10000;
        nudLogWarnMB.Value    = 100;

        lblLogWarnUnit.Text      = "MB";
        lblLogWarnUnit.Font      = new Font("맑은 고딕", 9F);
        lblLogWarnUnit.Location  = new Point(162, 298);
        lblLogWarnUnit.AutoSize  = true;
        lblLogWarnUnit.ForeColor = Color.DimGray;

        lblRecWarn.Text     = "영상 임계값:";
        lblRecWarn.Font     = new Font("맑은 고딕", 9F);
        lblRecWarn.Location = new Point(8, 326);
        lblRecWarn.AutoSize = true;

        nudRecWarnGB.Location = new Point(86, 323);
        nudRecWarnGB.Size     = new Size(72, 25);
        nudRecWarnGB.Font     = new Font("맑은 고딕", 9F);
        nudRecWarnGB.Minimum  = 1;
        nudRecWarnGB.Maximum  = 1000;
        nudRecWarnGB.Value    = 5;

        lblRecWarnUnit.Text      = "GB";
        lblRecWarnUnit.Font      = new Font("맑은 고딕", 9F);
        lblRecWarnUnit.Location  = new Point(162, 326);
        lblRecWarnUnit.AutoSize  = true;
        lblRecWarnUnit.ForeColor = Color.DimGray;

        lblCurrentUsage.Location  = new Point(8, 354);
        lblCurrentUsage.Size      = new Size(280, 16);
        lblCurrentUsage.Font      = new Font("맑은 고딕", 8F);
        lblCurrentUsage.ForeColor = Color.Gray;
        lblCurrentUsage.Text      = "현재: 계산 중...";

        lblSectionDataRoot.Text      = "── 저장 폴더 ─────────────────────";
        lblSectionDataRoot.Location  = new Point(8, 382);
        lblSectionDataRoot.Size      = new Size(280, 16);
        lblSectionDataRoot.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSectionDataRoot.ForeColor = Color.DimGray;

        txtDataRootParent.Location        = new Point(8, 404);
        txtDataRootParent.Size            = new Size(220, 25);
        txtDataRootParent.Font            = new Font("맑은 고딕", 9F);
        txtDataRootParent.ReadOnly        = true;
        txtDataRootParent.BackColor       = SystemColors.Window;

        btnBrowseDataRoot.Location  = new Point(232, 403);
        btnBrowseDataRoot.Size      = new Size(28, 26);
        btnBrowseDataRoot.Font      = new Font("맑은 고딕", 9F);
        btnBrowseDataRoot.Text      = "...";
        btnBrowseDataRoot.Click    += new EventHandler(BtnBrowseDataRoot_Click);

        btnResetDataRoot.Location  = new Point(264, 403);
        btnResetDataRoot.Size      = new Size(26, 26);
        btnResetDataRoot.Font      = new Font("맑은 고딕", 9F);
        btnResetDataRoot.Text      = "↺";
        btnResetDataRoot.ForeColor = Color.DimGray;
        btnResetDataRoot.Click    += new EventHandler(BtnResetDataRoot_Click);

        lblDataRootPreview.Location  = new Point(8, 434);
        lblDataRootPreview.Size      = new Size(280, 32);
        lblDataRootPreview.Font      = new Font("맑은 고딕", 7.5F);
        lblDataRootPreview.ForeColor = Color.SteelBlue;

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
        tabGeneral.Controls.Add(lblSectionStorage);
        tabGeneral.Controls.Add(chkStorageWarning);
        tabGeneral.Controls.Add(lblLogWarn);
        tabGeneral.Controls.Add(nudLogWarnMB);
        tabGeneral.Controls.Add(lblLogWarnUnit);
        tabGeneral.Controls.Add(lblRecWarn);
        tabGeneral.Controls.Add(nudRecWarnGB);
        tabGeneral.Controls.Add(lblRecWarnUnit);
        tabGeneral.Controls.Add(lblCurrentUsage);
        tabGeneral.Controls.Add(lblSectionDataRoot);
        tabGeneral.Controls.Add(txtDataRootParent);
        tabGeneral.Controls.Add(btnBrowseDataRoot);
        tabGeneral.Controls.Add(btnResetDataRoot);
        tabGeneral.Controls.Add(lblDataRootPreview);

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

        // ── 번역 탭 ──────────────────────────────────────────────────

        chkTransEnabled.Text     = "자막/번역 기능 활성화";
        chkTransEnabled.Font     = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        chkTransEnabled.Location = new Point(8, 8);
        chkTransEnabled.Size     = new Size(280, 22);
        chkTransEnabled.CheckedChanged += new EventHandler(ChkTransEnabled_CheckedChanged);

        lblSecVosk.Text      = "── 음성 인식 (Vosk) ─────────────";
        lblSecVosk.Location  = new Point(8, 36);
        lblSecVosk.Size      = new Size(280, 16);
        lblSecVosk.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSecVosk.ForeColor = Color.DimGray;

        lstVoskModels.Location      = new Point(8, 56);
        lstVoskModels.Size          = new Size(280, 70);
        lstVoskModels.Font          = new Font("맑은 고딕", 9F);
        lstVoskModels.View          = View.Details;
        lstVoskModels.FullRowSelect = true;
        lstVoskModels.MultiSelect   = false;
        lstVoskModels.HeaderStyle   = ColumnHeaderStyle.Nonclickable;
        lstVoskModels.Columns.Add("모델명", 84);
        lstVoskModels.Columns.Add("경로",  192);
        lstVoskModels.Click        += new EventHandler(LstVoskModels_Click);

        btnAddModel.Location  = new Point(8, 130);
        btnAddModel.Size      = new Size(70, 24);
        btnAddModel.Font      = new Font("맑은 고딕", 9F);
        btnAddModel.Text      = "추가";
        btnAddModel.Click    += new EventHandler(BtnAddModel_Click);

        btnRemoveModel.Location  = new Point(82, 130);
        btnRemoveModel.Size      = new Size(70, 24);
        btnRemoveModel.Font      = new Font("맑은 고딕", 9F);
        btnRemoveModel.Text      = "삭제";
        btnRemoveModel.Click    += new EventHandler(BtnRemoveModel_Click);

        lnkVoskDl.Text      = "↗ Vosk 모델 다운로드 (alphacephei.com/vosk/models)";
        lnkVoskDl.Location  = new Point(8, 159);
        lnkVoskDl.Size      = new Size(280, 18);
        lnkVoskDl.Font      = new Font("맑은 고딕", 8.5F);
        lnkVoskDl.LinkClicked += new LinkLabelLinkClickedEventHandler(LnkVoskDl_LinkClicked);

        lblSecDeepL.Text      = "── 번역 (DeepL Free API) ─────────";
        lblSecDeepL.Location  = new Point(8, 183);
        lblSecDeepL.Size      = new Size(280, 16);
        lblSecDeepL.Font      = new Font("맑은 고딕", 8.5F, FontStyle.Bold);
        lblSecDeepL.ForeColor = Color.DimGray;

        chkDeepLOn.Text     = "번역 활성화 (해제 시 자막만 표시)";
        chkDeepLOn.Font     = new Font("맑은 고딕", 9.5F);
        chkDeepLOn.Location = new Point(8, 203);
        chkDeepLOn.Size     = new Size(280, 22);
        chkDeepLOn.CheckedChanged += new EventHandler(ChkDeepLOn_CheckedChanged);

        txtDeepLKey.Location        = new Point(8, 229);
        txtDeepLKey.Size            = new Size(252, 25);
        txtDeepLKey.Font            = new Font("맑은 고딕", 9F);
        txtDeepLKey.PasswordChar    = '*';
        txtDeepLKey.PlaceholderText = "DeepL API Key";

        btnShowKey.Location  = new Point(263, 228);
        btnShowKey.Size      = new Size(25, 26);
        btnShowKey.Font      = new Font("맑은 고딕", 9F);
        btnShowKey.Text      = "👁";
        btnShowKey.Click    += new EventHandler(BtnShowKey_Click);

        lnkDeepLGet.Text      = "↗ DeepL Free API 발급 안내 (500,000자/월 무료)";
        lnkDeepLGet.Location  = new Point(8, 260);
        lnkDeepLGet.Size      = new Size(280, 18);
        lnkDeepLGet.Font      = new Font("맑은 고딕", 8.5F);
        lnkDeepLGet.LinkClicked += new LinkLabelLinkClickedEventHandler(LnkDeepLGet_LinkClicked);

        lblSrcLang.Text     = "원본:";
        lblSrcLang.Font     = new Font("맑은 고딕", 9F);
        lblSrcLang.Location = new Point(8, 285);
        lblSrcLang.AutoSize = true;

        cmbSrcLang.Location      = new Point(42, 282);
        cmbSrcLang.Size          = new Size(110, 25);
        cmbSrcLang.Font          = new Font("맑은 고딕", 9F);
        cmbSrcLang.DropDownStyle = ComboBoxStyle.DropDownList;

        lblTgtLang.Text     = "번역:";
        lblTgtLang.Font     = new Font("맑은 고딕", 9F);
        lblTgtLang.Location = new Point(162, 285);
        lblTgtLang.AutoSize = true;

        cmbTgtLang.Location      = new Point(196, 282);
        cmbTgtLang.Size          = new Size(92, 25);
        cmbTgtLang.Font          = new Font("맑은 고딕", 9F);
        cmbTgtLang.DropDownStyle = ComboBoxStyle.DropDownList;

        chkShowOrig.Text     = "원문도 자막에 표시";
        chkShowOrig.Font     = new Font("맑은 고딕", 9.5F);
        chkShowOrig.Location = new Point(8, 313);
        chkShowOrig.Size     = new Size(280, 22);

        lblUsage.Location  = new Point(8, 340);
        lblUsage.Size      = new Size(184, 25);
        lblUsage.Font      = new Font("맑은 고딕", 9F);
        lblUsage.Text      = "이번 달: - / - 자";
        lblUsage.ForeColor = Color.DimGray;
        lblUsage.TextAlign = ContentAlignment.MiddleLeft;

        btnCheckUsage.Location  = new Point(196, 339);
        btnCheckUsage.Size      = new Size(92, 26);
        btnCheckUsage.Font      = new Font("맑은 고딕", 8.5F);
        btnCheckUsage.Text      = "사용량 조회";
        btnCheckUsage.Click    += new EventHandler(BtnCheckUsage_Click);

        chkLogTrans.Text     = "원문/번역문 로그 기록";
        chkLogTrans.Font     = new Font("맑은 고딕", 9.5F);
        chkLogTrans.Location = new Point(8, 371);
        chkLogTrans.Size     = new Size(180, 22);

        btnOpenSubtitleFolder.Text      = "폴더 열기";
        btnOpenSubtitleFolder.Font      = new Font("맑은 고딕", 8.5F);
        btnOpenSubtitleFolder.Location  = new Point(192, 370);
        btnOpenSubtitleFolder.Size      = new Size(96, 24);
        btnOpenSubtitleFolder.ForeColor = Color.DarkSlateBlue;
        btnOpenSubtitleFolder.Click    += new EventHandler(BtnOpenSubtitleFolder_Click);

        tabTranslation.Text = "번역";
        tabTranslation.Font = new Font("맑은 고딕", 9F);
        tabTranslation.Controls.Add(chkTransEnabled);
        tabTranslation.Controls.Add(lblSecVosk);
        tabTranslation.Controls.Add(lstVoskModels);
        tabTranslation.Controls.Add(btnAddModel);
        tabTranslation.Controls.Add(btnRemoveModel);
        tabTranslation.Controls.Add(lnkVoskDl);
        tabTranslation.Controls.Add(lblSecDeepL);
        tabTranslation.Controls.Add(chkDeepLOn);
        tabTranslation.Controls.Add(txtDeepLKey);
        tabTranslation.Controls.Add(btnShowKey);
        tabTranslation.Controls.Add(lnkDeepLGet);
        tabTranslation.Controls.Add(lblSrcLang);
        tabTranslation.Controls.Add(cmbSrcLang);
        tabTranslation.Controls.Add(lblTgtLang);
        tabTranslation.Controls.Add(cmbTgtLang);
        tabTranslation.Controls.Add(chkShowOrig);
        tabTranslation.Controls.Add(lblUsage);
        tabTranslation.Controls.Add(btnCheckUsage);
        tabTranslation.Controls.Add(chkLogTrans);
        tabTranslation.Controls.Add(btnOpenSubtitleFolder);

        // ── TabControl ────────────────────────────────────────────────

        tabControl.Location = new Point(8, 8);
        tabControl.Size     = new Size(300, 494);
        tabControl.Font     = new Font("맑은 고딕", 9F);
        tabControl.TabPages.Add(tabGeneral);
        tabControl.TabPages.Add(tabRecording);
        tabControl.TabPages.Add(tabTranslation);

        // ── 하단 버튼 ─────────────────────────────────────────────────

        btnOk.Location  = new Point(8, 510);
        btnOk.Size      = new Size(140, 32);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        btnCancel.Location  = new Point(160, 510);
        btnCancel.Size      = new Size(140, 32);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        // ── Form ──────────────────────────────────────────────────────

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(316, 554);
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
