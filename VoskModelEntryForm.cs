namespace WebCamControl;

internal partial class VoskModelEntryForm : Form
{
    internal string ModelName => txtName.Text.Trim();
    internal string ModelPath => txtPath.Text.Trim();

    internal VoskModelEntryForm()
    {
        InitializeComponent();
    }

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dlg = new FolderBrowserDialog
        {
            Description            = "Vosk 모델 폴더 선택",
            UseDescriptionForTitle = true,
            SelectedPath           = txtPath.Text,
        };
        if (dlg.ShowDialog(this) == DialogResult.OK)
            txtPath.Text = dlg.SelectedPath;
    }

    private void BtnOk_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(ModelName))
        {
            MessageBox.Show("모델 이름을 입력해주세요.", "입력 필요",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtName.Focus();
            return;
        }
        if (!Directory.Exists(ModelPath))
        {
            MessageBox.Show("존재하는 폴더 경로를 입력해주세요.", "경로 오류",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            txtPath.Focus();
            return;
        }
        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
