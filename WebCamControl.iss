#define MyAppName    "WebCam Controller"
#define MyAppVersion "1.04"
#define MyAppPublisher "jaeho"
#define MyAppContact "jaeho9697@gmail.com"
#define MyAppExeName "WebCamControl.exe"
#define MyAppExeSrc  "bin\Release\net8.0-windows\win-x64\publish\WebCamControl.exe"

[Setup]
AppId={{6F3A2B1C-9D4E-4F7A-B832-1A2C3D4E5F60}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL=mailto:{#MyAppContact}
AppContact={#MyAppContact}
DefaultDirName={autopf}\WebCamControl
DefaultGroupName={#MyAppName}
PrivilegesRequired=admin
OutputDir=installer
OutputBaseFilename=WebCamControl_Setup_v{#MyAppVersion}
SetupIconFile=icon.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
Compression=lzma2/ultra64
SolidCompression=yes
WizardStyle=modern
MinVersion=10.0

[Languages]
Name: "korean"; MessagesFile: "compiler:Languages\Korean.isl"

[Tasks]
Name: "startup";     Description: "Windows 시작 시 자동으로 실행 (시작 프로그램 등록)"; GroupDescription: "추가 옵션:"
Name: "desktopicon"; Description: "바탕화면에 바로가기 만들기";                         GroupDescription: "추가 옵션:"

[Files]
Source: "{#MyAppExeSrc}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}";                        Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}";  Filename: "{uninstallexe}"
Name: "{commondesktop}\{#MyAppName}";                Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; 설치 후 실행 파일 해시 등록 (무결성 검증 기준값 설정)
Filename: "{app}\{#MyAppExeName}"; \
  Parameters: "--update-hash"; \
  Flags: runhidden waituntilterminated

; 시작 프로그램 등록
Filename: "{app}\{#MyAppExeName}"; \
  Parameters: "--register-startup"; \
  Flags: runhidden waituntilterminated; Tasks: startup

; 설치 완료 후 앱 바로 실행 여부 묻기
Filename: "{app}\{#MyAppExeName}"; \
  Description: "{cm:LaunchProgram,{#MyAppName}}"; \
  Flags: nowait postinstall skipifsilent

[UninstallRun]
; 제거 시 작업 스케줄러에서 삭제
Filename: "schtasks"; \
  Parameters: "/delete /tn ""WebCamControl"" /f"; \
  Flags: runhidden; RunOnceId: "RemoveStartupTask"
