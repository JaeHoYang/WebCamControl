#nullable enable
namespace WebCamControl;

partial class RecordingListForm
{
    private System.ComponentModel.IContainer? components = null;

    private ListView lvRecordings    = null!;
    private Label    lblCount        = null!;
    private Button   btnOpenFolder   = null!;
    private Button   btnRefresh      = null!;
    private Button   btnClose        = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lvRecordings  = new ListView();
        lblCount      = new Label();
        btnOpenFolder = new Button();
        btnRefresh    = new Button();
        btnClose      = new Button();
        SuspendLayout();

        lvRecordings.Location      = new Point(12, 12);
        lvRecordings.Size          = new Size(560, 340);
        lvRecordings.Font          = new Font("맑은 고딕", 9F);
        lvRecordings.View          = View.Details;
        lvRecordings.FullRowSelect = true;
        lvRecordings.MultiSelect   = false;
        lvRecordings.HeaderStyle   = ColumnHeaderStyle.Nonclickable;
        lvRecordings.Columns.Add("파일명",  240);
        lvRecordings.Columns.Add("날짜",    150);
        lvRecordings.Columns.Add("크기",     90);
        lvRecordings.DoubleClick  += new EventHandler(LvRecordings_DoubleClick);

        lblCount.Location  = new Point(12, 360);
        lblCount.Size      = new Size(200, 20);
        lblCount.Font      = new Font("맑은 고딕", 8.5F);
        lblCount.ForeColor = Color.DimGray;
        lblCount.Text      = "총 0개";

        btnOpenFolder.Location  = new Point(12, 386);
        btnOpenFolder.Size      = new Size(120, 30);
        btnOpenFolder.Font      = new Font("맑은 고딕", 9F);
        btnOpenFolder.Text      = "폴더 열기";
        btnOpenFolder.Click    += new EventHandler(BtnOpenFolder_Click);

        btnRefresh.Location  = new Point(340, 386);
        btnRefresh.Size      = new Size(100, 30);
        btnRefresh.Font      = new Font("맑은 고딕", 9F);
        btnRefresh.Text      = "새로고침";
        btnRefresh.Click    += new EventHandler(BtnRefresh_Click);

        btnClose.Location  = new Point(472, 386);
        btnClose.Size      = new Size(100, 30);
        btnClose.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnClose.Text      = "닫기";
        btnClose.Click    += new EventHandler(BtnClose_Click);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(584, 430);
        Controls.Add(lvRecordings);
        Controls.Add(lblCount);
        Controls.Add(btnOpenFolder);
        Controls.Add(btnRefresh);
        Controls.Add(btnClose);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "녹화 파일 목록";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
