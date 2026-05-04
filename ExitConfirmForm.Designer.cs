#nullable enable
namespace WebCamControl;

partial class ExitConfirmForm
{
    private System.ComponentModel.IContainer? components = null;
    private Label  lblMessage = null!;
    private Button btnTray    = null!;
    private Button btnExit    = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblMessage = new Label();
        btnTray    = new Button();
        btnExit    = new Button();
        SuspendLayout();

        // lblMessage
        lblMessage.Location  = new Point(16, 18);
        lblMessage.Size      = new Size(256, 32);
        lblMessage.Font      = new Font("맑은 고딕", 9.5F);
        lblMessage.Text      = "어떻게 하시겠습니까?";
        lblMessage.TextAlign = ContentAlignment.MiddleCenter;

        // btnTray
        btnTray.Location  = new Point(16, 62);
        btnTray.Size      = new Size(120, 34);
        btnTray.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnTray.Text      = "트레이로 최소화";
        btnTray.ForeColor = Color.DarkSlateBlue;
        btnTray.Click    += new EventHandler(BtnTray_Click);

        // btnExit
        btnExit.Location  = new Point(152, 62);
        btnExit.Size      = new Size(120, 34);
        btnExit.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnExit.Text      = "완전 종료";
        btnExit.ForeColor = Color.Red;
        btnExit.Click    += new EventHandler(BtnExit_Click);

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(288, 112);
        Controls.Add(lblMessage);
        Controls.Add(btnTray);
        Controls.Add(btnExit);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "종료";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
