#define MyAppName       "WebCam Controller"
#define MyAppVersion    "1.06.21"
#define MyAppPublisher  "jaeho"
#define MyAppContact    "jaeho9697@gmail.com"
#define MyAppExeName    "WebCamControl.exe"
#define MyAppExeSrc     "bin\Release\net8.0-windows\win-x64\publish\WebCamControl.exe"
#define DotNetVersion   "8.0"
#define DotNetRuntimeUrl "https://aka.ms/dotnet/8.0/windowsdesktop-runtime-win-x64.exe"

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

[Code]
var
  UseCustomDirPage: TInputOptionWizardPage;
  DataRootPage:     TInputDirWizardPage;

procedure InitializeWizard;
begin
  UseCustomDirPage := CreateInputOptionPage(
    wpReady,
    '데이터 저장 위치',
    '로그, 자막, 녹화 파일이 저장될 위치를 선택합니다.',
    '데이터 저장 위치를 어떻게 설정하시겠습니까?',
    True, False);
  UseCustomDirPage.Add('기본 위치 사용 (%AppData%\Local\WebCamControl)');
  UseCustomDirPage.Add('사용자 지정 위치 선택 (WebCamData 폴더 자동 생성)');
  UseCustomDirPage.SelectedValueIndex := 0;

  DataRootPage := CreateInputDirPage(
    UseCustomDirPage.ID,
    '사용자 지정 저장 위치',
    '선택한 폴더 아래에 "WebCamData" 폴더가 자동으로 생성됩니다.',
    '예: D:\ 선택 시 → D:\WebCamData\ 에 로그·영상이 저장됩니다.',
    False, '');
  DataRootPage.Add('저장 루트 폴더:');
  DataRootPage.Values[0] := 'C:\';
end;

function ShouldSkipPage(PageID: Integer): Boolean;
begin
  Result := False;
  if PageID = DataRootPage.ID then
    Result := (UseCustomDirPage.SelectedValueIndex = 0);
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  BootstrapDir, BootstrapFile, DataRootValue: String;
begin
  if CurStep = ssPostInstall then
  begin
    if UseCustomDirPage.SelectedValueIndex = 0 then Exit;
    DataRootValue := Trim(DataRootPage.Values[0]);
    if DataRootValue = '' then Exit;
    BootstrapDir := ExpandConstant('{localappdata}\WebCamControl');
    if not ForceDirectories(BootstrapDir) then Exit;
    BootstrapFile := BootstrapDir + '\dataroot_bootstrap.txt';
    SaveStringToFile(BootstrapFile, DataRootValue, False);
  end;
end;

// .NET 8 Windows Desktop Runtime 설치 여부 확인
function IsDotNet8Installed(): Boolean;
var
  FindRec: TFindRec;
begin
  Result := FindFirst(
    ExpandConstant('{commonpf64}\dotnet\shared\Microsoft.WindowsDesktop.App\8.*'),
    FindRec);
  if Result then FindClose(FindRec);
end;

// 설치 전 .NET 8 없으면 다운로드·설치
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  ResultCode: Integer;
begin
  Result := '';
  if IsDotNet8Installed() then Exit;

  if MsgBox(
    '.NET 8 Desktop Runtime이 설치되지 않았습니다.' + #13#10 +
    '지금 Microsoft에서 다운로드하여 설치합니다. (약 55 MB)' + #13#10#13#10 +
    '계속하시겠습니까?',
    mbConfirmation, MB_YESNO) = IDNO then
  begin
    Result := '.NET 8 Desktop Runtime 설치를 취소했습니다.';
    Exit;
  end;

  try
    DownloadTemporaryFile(
      '{#DotNetRuntimeUrl}',
      'dotnet8-runtime.exe', '', nil);
  except
    Result := '.NET 8 다운로드 실패: ' + GetExceptionMessage();
    Exit;
  end;

  if not Exec(ExpandConstant('{tmp}\dotnet8-runtime.exe'),
              '/install /quiet /norestart', '', SW_SHOW,
              ewWaitUntilTerminated, ResultCode) then
    Result := '.NET 8 설치 실패 (코드: ' + IntToStr(ResultCode) + ')';
end;
