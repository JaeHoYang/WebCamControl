using System.Runtime.InteropServices;

namespace WebCamControl;

internal partial class TranslationOverlayForm : Form
{
    internal event Action? UserClosed;

    private bool _showOriginal;
    private const int MaxLines = 300;

    [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr h, int msg, IntPtr w, IntPtr l);
    private const int WM_SETREDRAW = 0x000B;

    internal bool ShowOriginal
    {
        get => _showOriginal;
        set => _showOriginal = value;
    }

    internal TranslationOverlayForm(bool showOriginal, Rectangle bounds,
        float fontSize = 11F, double opacity = 0.85, int bgColorArgb = -15461356)
    {
        InitializeComponent();
        _showOriginal = showOriginal;

        rtbLog.Font = new Font(rtbLog.Font.FontFamily, fontSize, FontStyle.Bold);
        Opacity      = opacity;
        var bg = Color.FromArgb(bgColorArgb);
        rtbLog.BackColor   = bg;
        lblPartial.BackColor = bg;
        BackColor          = bg;

        if (bounds != Rectangle.Empty)
        {
            var screen = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1920, 1080);
            int w = Math.Max(200, bounds.Width);
            int h = Math.Max(80,  bounds.Height);
            int x = Math.Max(screen.Left, Math.Min(bounds.X, screen.Right  - w));
            int y = Math.Max(screen.Top,  Math.Min(bounds.Y, screen.Bottom - h));
            SetBounds(x, y, w, h);
        }
    }

    internal void ShowPartial(string text)
    {
        if (!SafeInvoke(() => ShowPartial(text))) return;
        lblPartial.Text = text;
    }

    internal void ShowText(string original, string translated)
    {
        if (!SafeInvoke(() => ShowText(original, translated))) return;
        ClearPartial();
        if (_showOriginal)
            AppendLine($"원문: {original}", Color.FromArgb(180, 180, 180));
        AppendLine($"번역: {translated}", Color.Yellow);
        TrimOldLines();
    }

    internal void ShowSubtitle(string text)
    {
        if (!SafeInvoke(() => ShowSubtitle(text))) return;
        ClearPartial();
        AppendLine(text, Color.White);
        TrimOldLines();
    }

    internal void ShowWarning(string message)
    {
        if (!SafeInvoke(() => ShowWarning(message))) return;
        ClearPartial();
        AppendLine(message, Color.Orange);
    }

    private void ClearPartial()
    {
        lblPartial.Text = string.Empty;
    }

    // 크로스-스레드 호출을 안전하게 처리.
    // 이미 UI 스레드면 false 반환(호출자가 본문 실행).
    // 다른 스레드면 Invoke 후 true 반환(호출자는 본문 건너뜀).
    // 폼이 닫히는 중이면 Invoke 없이 true 반환(조용히 무시).
    private bool SafeInvoke(Action action)
    {
        if (!InvokeRequired) return false;
        if (IsDisposed || !IsHandleCreated) return true;
        try { BeginInvoke(action); } // 비동기 — 오디오 스레드가 UI 스레드를 블로킹하지 않음
        catch (ObjectDisposedException) { }
        catch (InvalidOperationException) { }
        return true;
    }

    private void AppendLine(string text, Color color)
    {
        rtbLog.SelectionStart  = rtbLog.TextLength;
        rtbLog.SelectionLength = 0;
        rtbLog.SelectionColor  = color;
        rtbLog.SelectedText    = text + "\n";
        rtbLog.SelectionStart  = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
    }

    private void TrimOldLines()
    {
        if (rtbLog.Lines.Length <= MaxLines) return;
        int removeUntil = rtbLog.GetFirstCharIndexFromLine(rtbLog.Lines.Length - MaxLines);
        SendMessage(rtbLog.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        rtbLog.Select(0, removeUntil);
        rtbLog.SelectedText = string.Empty;
        rtbLog.SelectionStart = rtbLog.TextLength;
        SendMessage(rtbLog.Handle, WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
        rtbLog.Invalidate();
        rtbLog.ScrollToCaret();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
            UserClosed?.Invoke();
        base.OnFormClosing(e);
    }
}
