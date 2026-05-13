using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using SharpAvi;
using SharpAvi.Output;

namespace WebCamControl;

internal sealed class WebcamRecorder : IDisposable
{
    private VideoCaptureDevice? _device;
    private AviWriter?          _writer;
    private IAviVideoStream?    _stream;
    private readonly object     _writeLock   = new();
    private bool                _streamReady = false;
    private int                 _frameCount  = 0;

    internal bool    IsRecording { get; private set; }
    internal int     Quality     { get; set; } = 1;  // 0=낮음 1=보통 2=높음
    internal string? CurrentFile { get; private set; }

    // 카메라는 보통 ~30fps; 품질에 맞게 프레임 스킵
    private int TargetFps   => Quality switch { 0 => 5, 2 => 15, _ => 10 };
    private int FrameSkip   => Math.Max(1, (int)Math.Round(30.0 / TargetFps));
    private int JpegQuality => Quality switch { 0 => 30, 2 => 90, _ => 60 };

    private static readonly ImageCodecInfo JpegCodec =
        ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

    internal bool Start(string deviceName, string tag = "webcam")
    {
        if (IsRecording) return false;

        var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        var info    = devices.Cast<FilterInfo>()
                             .FirstOrDefault(f => f.Name == deviceName);
        if (info == null) return false;

        Directory.CreateDirectory(ScreenRecorder.SaveDirectory);
        CurrentFile = Path.Combine(ScreenRecorder.SaveDirectory,
            $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{tag}_webcam.avi");

        _frameCount  = 0;
        _streamReady = false;
        _writer      = new AviWriter(CurrentFile) { FramesPerSecond = TargetFps, EmitIndex1 = true };

        _device           = new VideoCaptureDevice(info.MonikerString);
        _device.NewFrame += OnNewFrame;
        _device.Start();

        IsRecording = true;
        return true;
    }

    private void OnNewFrame(object sender, NewFrameEventArgs e)
    {
        if (!IsRecording) return;

        // 프레임 스킵으로 목표 FPS 제어
        if (++_frameCount % FrameSkip != 0) return;

        using var bmp = (Bitmap)e.Frame.Clone();

        // 첫 프레임에서 스트림 초기화 (해상도 확인 후)
        if (!_streamReady)
        {
            lock (_writeLock)
            {
                if (!_streamReady && _writer != null)
                {
                    _stream       = _writer.AddVideoStream(bmp.Width, bmp.Height, BitsPerPixel.Bpp24);
                    _stream.Codec = KnownFourCCs.Codecs.MotionJpeg;
                    _stream.Name  = "Webcam";
                    _streamReady  = true;
                }
            }
        }

        using var ms = new MemoryStream();
        var ep       = new EncoderParameters(1);
        ep.Param[0]  = new EncoderParameter(Encoder.Quality, (long)JpegQuality);
        bmp.Save(ms, JpegCodec, ep);
        var jpg = ms.ToArray();

        lock (_writeLock)
        {
            if (IsRecording && _streamReady)
                _stream?.WriteFrame(true, jpg, 0, jpg.Length);
        }
    }

    internal void Stop()
    {
        if (!IsRecording) return;
        IsRecording = false;

        if (_device != null)
        {
            _device.SignalToStop();
            _device.WaitForStop();
            _device.NewFrame -= OnNewFrame;
            _device            = null;
        }

        lock (_writeLock)
        {
            _writer?.Close();
            _writer      = null;
            _stream      = null;
            _streamReady = false;
        }
    }

    public void Dispose() => Stop();
}
