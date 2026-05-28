namespace WebCamControl;

internal partial class TranslationOverlayForm : Form
{
    internal event Action? UserClosed;

    private bool _showOriginal;
    private const int MaxLines = 300;

    // partial 텍스트가 rtbLog 어느 위치부터 시작하는지 추적 (-1 = partial 없음)
    private int _partialStart = -1;

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

        if (_partialStart < 0)
            _partialStart = rtbLog.TextLength;

        // partial 범위를 새 텍스트로 덮어씀 (회색 = 미확정)
        rtbLog.Select(_partialStart, rtbLog.TextLength - _partialStart);
        rtbLog.SelectionColor = Color.FromArgb(130, 130, 130);
        rtbLog.SelectedText   = text;
        rtbLog.SelectionStart = rtbLog.TextLength;
        rtbLog.ScrollToCaret();
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

    // partial 범위를 rtbLog에서 제거하고 상태를 초기화
    private void ClearPartial()
    {
        if (_partialStart < 0) return;
        int len = rtbLog.TextLength - _partialStart;
        if (len > 0)
        {
            rtbLog.Select(_partialStart, len);
            rtbLog.SelectedText = string.Empty;
        }
        _partialStart   = -1;
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
        try { Invoke(action); }
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
        rtbLog.Select(0, removeUntil);
        rtbLog.SelectedText = string.Empty;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing)
            UserClosed?.Invoke();
        base.OnFormClosing(e);
    }
}
