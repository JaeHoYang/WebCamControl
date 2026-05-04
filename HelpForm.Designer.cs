#nullable enable
namespace WebCamControl;

partial class HelpForm
{
    private System.ComponentModel.IContainer? components = null;
    private RichTextBox rtb      = null!;
    private Button      btnClose = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        rtb      = new RichTextBox();
        btnClose = new Button();
        SuspendLayout();

        // rtb
        rtb.Location  = new Point(12, 12);
        rtb.Size      = new Size(376, 380);
        rtb.Font      = new Font("맑은 고딕", 9F);
        rtb.ReadOnly  = true;
        rtb.BackColor = SystemColors.Window;
        rtb.BorderStyle = BorderStyle.None;
        rtb.ScrollBars  = RichTextBoxScrollBars.Vertical;

        // btnClose
        btnClose.Location  = new Point(148, 404);
        btnClose.Size      = new Size(104, 30);
        btnClose.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnClose.Text      = "닫기";
        btnClose.Click    += new EventHandler(BtnClose_Click);

        // Form
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(400, 448);
        Controls.Add(rtb);
        Controls.Add(btnClose);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "도움말";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
