#nullable enable
namespace WebCamControl;

partial class VoskModelEntryForm
{
    private Label   lblName   = null!;
    private TextBox txtName   = null!;
    private Label   lblPath   = null!;
    private TextBox txtPath   = null!;
    private Button  btnBrowse = null!;
    private Button  btnOk     = null!;
    private Button  btnCancel = null!;

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void InitializeComponent()
    {
        lblName   = new Label();
        txtName   = new TextBox();
        lblPath   = new Label();
        txtPath   = new TextBox();
        btnBrowse = new Button();
        btnOk     = new Button();
        btnCancel = new Button();
        SuspendLayout();

        lblName.Text     = "모델 이름:";
        lblName.Font     = new Font("맑은 고딕", 9F);
        lblName.Location = new Point(12, 14);
        lblName.AutoSize = true;

        txtName.Location        = new Point(84, 11);
        txtName.Size            = new Size(196, 25);
        txtName.Font            = new Font("맑은 고딕", 9F);
        txtName.PlaceholderText = "예: 한국어, 영어";

        lblPath.Text     = "폴더 경로:";
        lblPath.Font     = new Font("맑은 고딕", 9F);
        lblPath.Location = new Point(12, 46);
        lblPath.AutoSize = true;

        txtPath.Location        = new Point(84, 43);
        txtPath.Size            = new Size(152, 25);
        txtPath.Font            = new Font("맑은 고딕", 8.5F);
        txtPath.PlaceholderText = "Vosk 모델 폴더 경로";

        btnBrowse.Location = new Point(240, 42);
        btnBrowse.Size     = new Size(40, 26);
        btnBrowse.Font     = new Font("맑은 고딕", 8.5F);
        btnBrowse.Text     = "찾기";
        btnBrowse.Click   += new EventHandler(BtnBrowse_Click);

        btnOk.Location  = new Point(12, 82);
        btnOk.Size      = new Size(128, 30);
        btnOk.Font      = new Font("맑은 고딕", 9.5F, FontStyle.Bold);
        btnOk.Text      = "확인";
        btnOk.Click    += new EventHandler(BtnOk_Click);

        btnCancel.Location  = new Point(152, 82);
        btnCancel.Size      = new Size(128, 30);
        btnCancel.Font      = new Font("맑은 고딕", 9.5F);
        btnCancel.Text      = "취소";
        btnCancel.Click    += new EventHandler(BtnCancel_Click);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode       = AutoScaleMode.Font;
        ClientSize          = new Size(292, 124);
        Controls.Add(lblName);
        Controls.Add(txtName);
        Controls.Add(lblPath);
        Controls.Add(txtPath);
        Controls.Add(btnBrowse);
        Controls.Add(btnOk);
        Controls.Add(btnCancel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox     = false;
        MinimizeBox     = false;
        Text            = "Vosk 모델 추가";
        StartPosition   = FormStartPosition.CenterParent;
        ResumeLayout(false);
    }
}
