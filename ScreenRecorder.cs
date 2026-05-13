using System.Drawing.Imaging;
using System.Timers;
using SharpAvi;
using SharpAvi.Output;

namespace WebCamControl;

internal sealed class ScreenRecorder : IDisposable
{
    private AviWriter?           _writer;
    private IAviVideoStream?     _stream;
    private System.Timers.Timer? _timer;
    private Screen               _captureScreen = Screen.PrimaryScreen!;
    private readonly object      _writeLock     = new();

    internal bool    IsRecording   { get; private set; }
    internal int     MonitorIndex  { get; set; } = 0;
    internal int     Quality       { get; set; } = 1;  // 0=낮음 1=보통 2=높음
    internal string? CurrentFile   { get; private set; }

    internal static string SaveDirectory => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "WebCamControl", "recordings");

    private int Fps         => Quality switch { 0 => 5, 2 => 15, _ => 10 };
    private int JpegQuality => Quality switch { 0 => 30, 2 => 90, _ => 60 };

    private static readonly ImageCodecInfo JpegCodec =
        ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

    internal void Start(string tag = "screen")
    {
        if (IsRecording) return;

        _captureScreen = Screen.AllScreens.ElementAtOrDefault(MonitorIndex) ?? Screen.PrimaryScreen!;
        var b = _captureScreen.Bounds;

        Directory.CreateDirectory(SaveDirectory);
        CurrentFile = Path.Combine(SaveDirectory, $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{tag}_screen.avi");

        _writer = new AviWriter(CurrentFile) { FramesPerSecond = Fps, EmitIndex1 = true };
        _stream = _writer.AddVideoStream(b.Width, b.Height, BitsPerPixel.Bpp24);
        _stream.Codec = KnownFourCCs.Codecs.MotionJpeg;
        _stream.Name  = "Screen";

        _timer           = new System.Timers.Timer(1000.0 / Fps) { AutoReset = true };
        _timer.Elapsed  += CaptureFrame;
        _timer.Start();

        IsRecording = true;
    }

    internal void Stop()
    {
        if (!IsRecording) return;
        IsRecording = false;

        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;

        lock (_writeLock)
        {
            _writer?.Close();
            _writer = null;
            _stream = null;
        }
    }

    private void CaptureFrame(object? sender, ElapsedEventArgs e)
    {
        if (!IsRecording) return;

        var b = _captureScreen.Bounds;
        using var bmp = new Bitmap(b.Width, b.Height, PixelFormat.Format24bppRgb);
        using (var g = Graphics.FromImage(bmp))
            g.CopyFromScreen(b.Location, Point.Empty, b.Size);

        using var ms  = new MemoryStream();
        var ep        = new EncoderParameters(1);
        ep.Param[0]   = new EncoderParameter(Encoder.Quality, (long)JpegQuality);
        bmp.Save(ms, JpegCodec, ep);
        var jpg = ms.ToArray();

        lock (_writeLock)
        {
            if (IsRecording)
                _stream?.WriteFrame(true, jpg, 0, jpg.Length);
        }
    }

    public void Dispose() => Stop();
}
