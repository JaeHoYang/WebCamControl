#nullable enable
namespace WebCamControl;

partial class TranslationOverlayForm
{
    private RichTextBox rtbLog     = null!;
    private Panel       pnlSep     = null!;
    private Label       lblPartial = null!;

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
        rtbLog     = new RichTextBox();
        pnlSep     = new Panel();
        lblPartial = new Label();
        SuspendLayout();

        lblPartial.Dock      = DockStyle.Bottom;
        lblPartial.Height    = 24;
        lblPartial.Font      = new Font("맑은 고딕", 10F);
        lblPartial.ForeColor = Color.Silver;
        lblPartial.BackColor = Color.FromArgb(20, 20, 20);
        lblPartial.Padding   = new Padding(6, 2, 6, 2);
        lblPartial.AutoSize  = false;

        pnlSep.Dock      = DockStyle.Bottom;
        pnlSep.Height    = 1;
        pnlSep.BackColor = Color.FromArgb(80, 80, 80);

        rtbLog.Dock        = DockStyle.Fill;
        rtbLog.BackColor   = Color.FromArgb(20, 20, 20);
        rtbLog.ForeColor   = Color.White;
        rtbLog.Font        = new Font("맑은 고딕", 11F, FontStyle.Bold);
        rtbLog.BorderStyle = BorderStyle.None;
        rtbLog.ReadOnly    = true;
        rtbLog.ScrollBars  = RichTextBoxScrollBars.Vertical;
        rtbLog.WordWrap    = true;
        rtbLog.Cursor      = Cursors.Default;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        BackColor           = Color.FromArgb(20, 20, 20);
        ClientSize          = new Size(600, 200);
        Controls.Add(rtbLog);
        Controls.Add(pnlSep);
        Controls.Add(lblPartial);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Opacity         = 0.85;
        ShowInTaskbar   = false;
        Text            = "자막";
        TopMost         = true;
        ResumeLayout(false);
    }
}
