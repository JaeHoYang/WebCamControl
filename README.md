# WebCam Controller

Windows PC의 웹캠과 마이크를 제어하고, 카메라 사용 여부를 실시간으로 감시하여 **텔레그램 · Discord 알림**을 보내는 보안 유틸리티입니다.  
추가로 시스템 오디오를 실시간으로 음성 인식하여 **자막 오버레이**로 표시하고 **DeepL로 번역**하는 기능도 제공합니다.

---

## 주요 기능

| 기능 | 설명 |
|------|------|
| 📷 카메라 제어 | 연결된 카메라 목록 표시, 클릭 한 번으로 활성화/비활성화 |
| 🎤 마이크 제어 | 연결된 마이크 목록 표시, 클릭 한 번으로 음소거/해제 |
| 👁️ 웹캠 감시 | 1~10초 간격으로 카메라 상태 자동 감시 (기본 3초) |
| 📱 텔레그램 알림 | 이상 감지 시 스마트폰으로 즉시 알림 (6가지 이벤트) |
| 🔔 Discord 알림 | 웹훅 URL 입력만으로 Discord 채널에 알림 전송 |
| 🎬 화면 녹화 | 웹캠 사용 감지 시 자동 녹화 또는 수동 녹화 시작 (MJPEG AVI) |
| 💬 실시간 자막 | 시스템 오디오를 Vosk로 음성 인식 → 화면 오버레이로 자막 표시 |
| 🌐 실시간 번역 | DeepL Free API로 인식된 음성을 자동 번역 (월 50만 자 무료) |
| 📋 이벤트 로그 | 날짜별 텍스트 파일에 이벤트 자동 기록, 앱 내 뷰어 제공 |
| 📝 자막 로그 | 음성 인식 원문·번역문을 날짜별 파일에 별도 기록 |
| 🖥️ 시스템 트레이 | 백그라운드 상주, 트레이 아이콘으로 조용히 동작 |
| 🔒 보안 저장 | 토큰 · URL · API 키를 Windows DPAPI로 암호화 저장 |
| 🛡️ 파일 변조 감지 | 실행 시마다 SHA-256 해시 검증 → 변조 시 즉시 알림 |
| ⚙️ 시작 프로그램 | Windows 로그인 시 관리자 권한으로 자동 실행 |

---

## 감시 알림 이벤트

| 이벤트 | 알림 메시지 |
|--------|------------|
| 감시 시작 | 🟢 웹캠 감시 시작 |
| 카메라 장치 활성화 | ⚠️ 웹캠 활성화 감지! |
| 카메라 장치 비활성화 | 🔴 웹캠 비활성화 감지! |
| 앱이 카메라 사용 시작 (Zoom, Teams 등) | 📷 웹캠 사용 감지! |
| 앱이 카메라 사용 종료 | ⏹️ 웹캠 사용 종료 |
| 감시 종료 / 프로그램 종료 | 🔴 WebCam Monitor 종료 |

---

## 요구사항

- Windows 10 / 11 (x64)
- 관리자 권한 (카메라 장치 제어에 필요)
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (미설치 시 설치 프로그램이 자동으로 다운로드)
- 인터넷 연결 (텔레그램 알림·DeepL 번역 사용 시)

---

## 설치

[Releases](../../releases) 페이지에서 `WebCamControl_Setup_v1.06.01.exe`를 다운로드하여 실행합니다.

설치 중 옵션:
- **Windows 시작 시 자동으로 실행** — 로그인 시 트레이에서 자동 감시 시작 (권장)
- **바탕화면에 바로가기 만들기**

> 설치 후 처음 실행 시 관리자 권한 요청 창이 표시됩니다. **예**를 눌러 허용해 주세요.

> **SmartScreen 경고가 표시되는 경우**  
> 설치 시 "Microsoft Defender SmartScreen에서 인식할 수 없는 앱" 경고가 뜰 수 있습니다.  
> 코드 서명 인증서가 없는 개인 개발 앱에서 발생하는 정상적인 경고입니다.  
> **"추가 정보" → "실행"** 을 클릭하면 설치가 진행됩니다.

---

## 실시간 자막 / 번역 설정

### Vosk 음성 인식 모델 설치

Vosk는 인터넷 없이 동작하는 오프라인 음성 인식 엔진입니다. 모델 파일을 직접 다운로드해야 합니다.

1. [https://alphacephei.com/vosk/models](https://alphacephei.com/vosk/models) 에서 모델 다운로드
2. 압축을 원하는 폴더에 해제 (예: `C:\Vosk\vosk-model-small-ko-0.22`)
3. 앱에서 **설정 → 번역 탭 → 모델 추가** 버튼으로 경로 등록

**권장 모델**

| 언어 | 모델명 | 크기 | 특징 |
|------|--------|------|------|
| 한국어 | `vosk-model-small-ko-0.22` | 약 82 MB | 소형, 빠른 응답, 일상 대화에 적합 |
| 영어 (소형) | `vosk-model-small-en-us-0.15` | 약 40 MB | 매우 가볍고 빠름, 명확한 발음 권장 |
| 영어 (고품질) | `vosk-model-en-us-0.22` | 약 1.8 GB | 높은 인식률, 다양한 억양·어휘 지원 |

여러 모델을 등록해 두고 목록에서 클릭 한 번으로 활성 모델을 전환할 수 있습니다.

### DeepL API 키 발급

DeepL Free API는 **월 500,000자까지 무료**로 번역을 제공합니다. 신용카드 없이 이메일만으로 가입할 수 있습니다.

1. [https://www.deepl.com/pro-api](https://www.deepl.com/pro-api) 접속 → **Free 플랜** 가입
2. 이메일 인증 완료 후 [https://www.deepl.com/account/summary](https://www.deepl.com/account/summary) 접속
3. **Authentication Key for DeepL API** 항목에서 키 복사
   - Free 키는 끝에 `:fx`가 붙습니다 (예: `xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx:fx`)
4. 앱에서 **설정 → 번역 탭 → DeepL 번역 사용 체크 → API Key 입력**

### 자막 오버레이 사용

1. 메인 화면 **자막 시작** 버튼 클릭 → 오버레이 창이 나타납니다
2. 오버레이는 마우스로 이동·크기 조절이 가능합니다
3. 하단(회색): 현재 인식 중인 텍스트 / 상단(누적): 확정된 자막
4. 번역 ON 시 원문(밝은 회색) + 번역(노란색)으로 표시
5. 번역 OFF 시 원문만 흰색으로 거의 실시간 표시

---

## 사용 방법

1. 텔레그램/Discord 설정 완료 후 **감시 시작** 버튼 클릭
2. 버튼이 빨간색 **"감시 중지"** 로 바뀌면 감시 중
3. 창을 닫으면 트레이로 숨어서 계속 동작
4. 트레이 아이콘 더블클릭 → 창 복원 / 우클릭 → 메뉴

---

## 보안 기능

### DPAPI 암호화
Bot Token · Chat ID · Discord Webhook URL · DeepL API 키는 Windows DPAPI(현재 사용자 범위)로 암호화하여 저장합니다.
동일 PC · 동일 계정 외에는 복호화가 불가능합니다.

### 파일 무결성 검증
실행할 때마다 exe 파일의 SHA-256 해시를 HKLM 레지스트리 저장값과 비교합니다.
불일치(변조) 감지 시 텔레그램 알림 전송 후 프로그램을 종료합니다.

---

## 빌드

```powershell
# 개발 빌드
dotnet build

# 릴리즈 빌드 (프레임워크 의존, 단일 파일 — .NET 8 설치 필요)
dotnet publish -c Release -r win-x64 --self-contained false `
  -p:PublishSingleFile=true

# 인스톨러 (Inno Setup 7 필요)
& "C:\Program Files\Inno Setup 7\ISCC.exe" WebCamControl.iss
```

---

## 기술 스택

| 항목 | 내용 |
|------|------|
| 언어 | C# (.NET 8, WinForms) |
| 카메라 제어 | Windows SetupAPI (P/Invoke) |
| 마이크 제어 | Windows CoreAudio API (P/Invoke) |
| 카메라 사용 감지 | Windows Registry (CapabilityAccessManager) |
| 화면 녹화 | GDI CopyFromScreen + SharpAvi (MJPEG AVI) |
| 음성 인식 | Vosk 0.3.38 (오프라인 STT) |
| 번역 | DeepL Free API (HttpClient) |
| 오디오 캡처 | NAudio 2.2.1 (WasapiLoopbackCapture) |
| 텔레그램 알림 | Telegram Bot API (HttpClient) |
| Discord 알림 | Discord Webhook (HttpClient) |
| 이벤트 로그 | 날짜별 텍스트 파일 (AppData\Local\WebCamControl\logs) |
| 자막 로그 | 날짜별 텍스트 파일 (AppData\Local\WebCamControl\subtitle_logs) |
| 설정 암호화 | Windows DPAPI (ProtectedData) |
| 파일 무결성 검증 | SHA-256 + DPAPI (HKLM 레지스트리) |
| 시작 프로그램 등록 | Windows Task Scheduler (schtasks) |
| 인스톨러 | Inno Setup 7 |

---

## 라이선스

개인 사용 목적으로 제작되었습니다.

---

제작자: jaeho · jaeho9697@gmail.com
