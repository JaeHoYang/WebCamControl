# WebCam Controller

Windows PC의 웹캠과 마이크를 제어하고, 카메라 사용 여부를 실시간으로 감시하여 **텔레그램 알림**을 보내는 보안 유틸리티입니다.

## 기능

### 카메라 / 마이크 제어
- 연결된 카메라·마이크 장치 목록 표시 및 선택
- 원클릭으로 카메라 활성화 / 비활성화
- 원클릭으로 마이크 음소거 / 해제

### 웹캠 감시 + 텔레그램 알림
3초 간격으로 다음 4가지 상황을 감지하고 텔레그램으로 즉시 알림을 보냅니다:

| 이벤트 | 알림 메시지 |
|--------|------------|
| 카메라 장치 활성화 | ⚠️ 웹캠 활성화 감지! |
| 카메라 장치 비활성화 | 🔴 웹캠 비활성화 감지! |
| 앱이 카메라 사용 시작 | 📷 웹캠 사용 감지! |
| 앱이 카메라 사용 종료 | ⏹️ 웹캠 사용 종료 |

### 기타
- **시스템 트레이**: X 버튼 / 최소화 → 트레이로 숨김, 더블클릭으로 복원
- **시작 프로그램**: 설치 시 또는 최초 실행 시 Windows 시작 프로그램 등록 선택 가능
- **보안 저장**: Bot Token · Chat ID를 Windows DPAPI로 암호화하여 저장

## 요구사항

- Windows 10 / 11 (x64)
- 관리자 권한 (카메라 장치 제어에 필요)

## 설치

[Releases](../../releases) 페이지에서 `WebCamControl_Setup_v1.02.exe`를 다운로드하여 실행합니다.

설치 중 **"Windows 시작 시 자동으로 실행"** 옵션을 선택하면 로그인 시 자동으로 트레이에서 실행됩니다.

## 텔레그램 봇 준비

1. **Bot Token 발급**
   - 텔레그램에서 `@BotFather` 검색 → `/newbot` 입력
   - 안내에 따라 봇 이름 설정 후 **Bot Token** 발급

2. **Chat ID 확인**
   - 텔레그램에서 `@userinfobot` 검색 → `/start` 입력
   - 표시된 **Chat ID** 복사

3. **앱에서 설정**
   - `텔레그램 설정` 버튼 → Token · Chat ID 입력 → `테스트 전송` 으로 확인

## 빌드

```bash
# 개발 빌드
dotnet build

# 릴리즈 단일 파일 빌드
dotnet publish -c Release -r win-x64 --self-contained true ^
  -p:PublishSingleFile=true ^
  -p:IncludeNativeLibrariesForSelfExtract=true

# 인스톨러 (Inno Setup 6/7 필요)
ISCC.exe WebCamControl.iss
```

## 기술 스택

| 항목 | 내용 |
|------|------|
| 언어 | C# (.NET 8, WinForms) |
| 카메라 제어 | Windows SetupAPI (P/Invoke) |
| 마이크 제어 | Windows CoreAudio API (P/Invoke) |
| 카메라 사용 감지 | Windows Registry (CapabilityAccessManager) |
| 텔레그램 알림 | Telegram Bot API (HttpClient) |
| 설정 암호화 | Windows DPAPI (ProtectedData) |
| 시작 프로그램 등록 | Windows Task Scheduler (schtasks) |
| 인스톨러 | Inno Setup |

## 라이선스

개인 사용 목적으로 제작되었습니다.

---

제작자: jaeho · jaeho9697@gmail.com
