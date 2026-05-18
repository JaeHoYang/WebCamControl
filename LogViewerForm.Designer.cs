#nullable enable
namespace WebCamControl;

partial class LogViewerForm
{
    private System.ComponentModel.IContainer? components = null;

    private Label          lblLogDate  = null!;
    private DateTimePicker dtpLogDate  = null!;
    private Label          lblLogType  = null!;
    private ComboBox       cmbLogType  = null!;
    private RichTextBox    rtbLog      = null!;
    private Button         btnRefresh  = null!;
    private Button         btnClose    = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblLogDate  = new Label();
        dtpLogDate  = new DateTimePicker();
        lblLogType  = new Label();
        cmbLogType  = new ComboBox();
        rtbLog      = new RichTextBox();
        btnRefresh  = new Button();
        btnClose    = new Button();
        SuspendLayout();

        lblLogDate.Text      = "날짜:";
        lblLogDate.Font      = new Font("맑은 고딕", 9F);
        lblLogDate.Location  = new Point(12, 14);
        lblLogDate.AutoSize  = true;

        dtpLogDate.Location  = new Point(52, 10);
        dtpLogDate.Size      = new Size(140, 25);
        dtpLogDate.Font      = new Font("맑은 고딕", 9F);
        dtpLogDate.Format    = DateTimePickerFormat.Short;
        dtpLogDate.ValueChanged += new EventHandler(DtpLogDate_ValueChanged);

        lblLogType.Text      = "로그 종류:";
        lblLogType.Font      = new Font("맑은 고딕", 9F);
        lblLogType.Location  = new Point(204, 14);
        lblLogType.AutoSize  = true;

        cmbLogType.Location      = new Point(282, 10);
        cmbLogType.Size          = new Size(120, 25);
        cmbLogType.Font          = new Font("맑은 고딕", 9F);
        cmbLogType.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbLogType.Items.Add("감시 로그");
        cmbLogType.Items.Add("자막 로그");
        cmbLogType.SelectedIndexChanged += new EventHandler(CmbLogType_SelectedIndexChanged);

        rtbLog.Location   = new Point(12, 44);
        rtbLog.Size       = new Size(560, 346);
        rtbLog.Font       = new Font("맑은 고딕", 9F);
        rtbLog.ReadOnly   = true;
        rtbLog.BackColor  = Color.White;
        rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbLog.WordWrap   = false;

        btnRefresh.Location  = new Point(12, 404);
        btnRefresh.Size      = new Size(100, 30);
        btnRefresh.Font      = new Font("맑은 고딕", 9.5F);
        btnRefresh.Text      = "새로고침";
        btnRefresh.Click    += new EventHandler(BtnRefresh_Click);

        btnClose.Location  = new Point(472, 404);
        btnClose.Size      = new Size(100, 30);
        btnClose.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnClose.Text      = "닫기";
        btnClose.Click    += new EventHandler(BtnClose_Click);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(584, 448);
        Controls.Add(lblLogDate);
        Controls.Add(dtpLogDate);
        Controls.Add(lblLogType);
        Controls.Add(cmbLogType);
        Controls.Add(rtbLog);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "로그";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
