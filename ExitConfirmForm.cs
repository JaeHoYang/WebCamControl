namespace WebCamControl;

internal partial class ExitConfirmForm : Form
{
    internal ExitConfirmForm()
    {
        InitializeComponent();
    }

    private void BtnTray_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }

    private void BtnExit_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }
}
