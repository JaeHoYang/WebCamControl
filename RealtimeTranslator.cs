using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Text.Json;
using Vosk;

namespace WebCamControl;

internal sealed class RealtimeTranslator : IDisposable
{
    internal event Action<string, string>? TextReceived;  // (original, translated)
    internal event Action<string>?         PartialReceived;
    internal event Action?                 QuotaExceeded;

    private readonly DeepLClient _deepl = new();
    private WasapiLoopbackCapture? _capture;
    private VoskRecognizer? _recognizer;
    private Model? _model;
    private bool _running;
    private bool _quotaExceeded;

    // partial 단어 수가 이 임계값을 넘으면 무음을 기다리지 않고 강제 커밋
    private const int AutoCommitWordThreshold = 15;

    internal bool   IsRunning          => _running;
    internal string VoskModelPath      { get; set; } = string.Empty;
    internal string DeepLApiKey        { get; set; } = string.Empty;
    internal string SourceLang         { get; set; } = "auto";
    internal string TargetLang         { get; set; } = "KO";
    internal bool   TranslationEnabled { get; set; } = true;

    internal async Task StartAsync()
    {
        if (_running) return;

        if (!Directory.Exists(VoskModelPath))
            throw new InvalidOperationException($"Vosk 모델 경로를 찾을 수 없습니다:\n{VoskModelPath}");

        _deepl.ApiKey  = DeepLApiKey.Trim();
        _quotaExceeded = false;

        if (TranslationEnabled && !string.IsNullOrEmpty(_deepl.ApiKey))
        {
            string masked = _deepl.ApiKey.Length > 8
                ? _deepl.ApiKey[..4] + "****" + _deepl.ApiKey[^4..]
                : "****";
            EventLogger.WriteSubtitle($"DeepL 초기화 — key: {masked} (엔드포인트 자동 감지)");
        }

        _model      = await Task.Run(() => new Model(VoskModelPath));
        _recognizer = new VoskRecognizer(_model, 16000.0f);
        _recognizer.SetMaxAlternatives(0);
        _recognizer.SetWords(false);

        _capture              = new WasapiLoopbackCapture();
        _capture.DataAvailable += OnDataAvailable;
        _running = true;
        _capture.StartRecording();
    }

    internal void Stop()
    {
        if (!_running) return;
        _running = false;

        _capture?.StopRecording();
        _capture?.Dispose();
        _capture = null;

        if (_recognizer != null)
        {
            var finalText = ParseText(_recognizer.FinalResult());
            if (!string.IsNullOrWhiteSpace(finalText))
                ProcessText(finalText);
            _recognizer.Dispose();
            _recognizer = null;
        }

        _model?.Dispose();
        _model = null;
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (!_running || _recognizer == null || _capture == null) return;

        byte[] pcm = ConvertToPcm16k(e.Buffer, e.BytesRecorded, _capture.WaveFormat);
        if (pcm.Length == 0) return;

        if (_recognizer.AcceptWaveform(pcm, pcm.Length))
        {
            var text = ParseText(_recognizer.Result());
            if (!string.IsNullOrWhiteSpace(text))
                ProcessText(text);
        }
        else
        {
            var partial = ParsePartialText(_recognizer.PartialResult());
            if (!string.IsNullOrWhiteSpace(partial))
            {
                int wordCount = partial.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
                if (wordCount >= AutoCommitWordThreshold)
                {
                    // 무음 없이 말이 길어지면 현재 partial을 확정하고 인식기 리셋
                    _recognizer.Reset();
                    ProcessText(partial);
                }
                else
                {
                    PartialReceived?.Invoke(partial);
                }
            }
        }
    }

    private void ProcessText(string original)
    {
        if (_quotaExceeded) return;

        // 번역 OFF → 즉시 발화 (네트워크 대기 없음)
        if (!TranslationEnabled || string.IsNullOrEmpty(DeepLApiKey))
        {
            TextReceived?.Invoke(original, original);
            return;
        }

        _ = Task.Run(async () =>
        {
            try
            {
                string translated = await _deepl.TranslateAsync(original, SourceLang, TargetLang);
                TextReceived?.Invoke(original, translated);
            }
            catch (DeepLQuotaExceededException)
            {
                _quotaExceeded = true;
                QuotaExceeded?.Invoke();
                TextReceived?.Invoke(original, original);
            }
            catch (Exception ex)
            {
                EventLogger.WriteSubtitle($"번역 오류: {ex.Message}");
                TextReceived?.Invoke(original, original);
            }
        });
    }

    private static byte[] ConvertToPcm16k(byte[] input, int count, WaveFormat inputFormat)
    {
        try
        {
            using var ms  = new MemoryStream(input, 0, count);
            using var raw = new RawSourceWaveStream(ms, inputFormat);

            ISampleProvider samples = raw.ToSampleProvider();
            if (inputFormat.Channels > 1)
                samples = new StereoToMonoSampleProvider(samples);

            var resampled = new WdlResamplingSampleProvider(samples, 16000);
            var pcm16     = new SampleToWaveProvider16(resampled);

            using var output = new MemoryStream();
            byte[] buf = new byte[4096];
            int read;
            while ((read = pcm16.Read(buf, 0, buf.Length)) > 0)
                output.Write(buf, 0, read);
            return output.ToArray();
        }
        catch { return Array.Empty<byte>(); }
    }

    private static string ParseText(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("text").GetString() ?? string.Empty;
        }
        catch { return string.Empty; }
    }

    private static string ParsePartialText(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("partial").GetString() ?? string.Empty;
        }
        catch { return string.Empty; }
    }

    public void Dispose()
    {
        Stop();
        _deepl.Dispose();
    }
}
