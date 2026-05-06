#nullable enable
namespace WebCamControl;

partial class LogViewerForm
{
    private System.ComponentModel.IContainer? components = null;

    private RichTextBox rtbLog     = null!;
    private Button      btnRefresh = null!;
    private Button      btnClose   = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        rtbLog     = new RichTextBox();
        btnRefresh = new Button();
        btnClose   = new Button();
        SuspendLayout();

        rtbLog.Location   = new Point(12, 12);
        rtbLog.Size       = new Size(560, 380);
        rtbLog.Font       = new Font("맑은 고딕", 9F);
        rtbLog.ReadOnly   = true;
        rtbLog.BackColor  = Color.White;
        rtbLog.ScrollBars = RichTextBoxScrollBars.Vertical;
        rtbLog.WordWrap   = false;

        btnRefresh.Location  = new Point(12, 406);
        btnRefresh.Size      = new Size(100, 30);
        btnRefresh.Font      = new Font("맑은 고딕", 9.5F);
        btnRefresh.Text      = "새로고침";
        btnRefresh.Click    += new EventHandler(BtnRefresh_Click);

        btnClose.Location  = new Point(472, 406);
        btnClose.Size      = new Size(100, 30);
        btnClose.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnClose.Text      = "닫기";
        btnClose.Click    += new EventHandler(BtnClose_Click);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(584, 448);
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
