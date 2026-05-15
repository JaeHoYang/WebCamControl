namespace WebCamControl;

internal partial class HelpForm : Form
{
    internal HelpForm()
    {
        InitializeComponent();
        PopulateContent();
    }

    private void PopulateContent()
    {
        AppendHeader("메인 화면 버튼 안내");
        AppendBody(
            "【 비디오 On / Off 】\n" +
            "   선택한 카메라 장치를 활성화하거나 비활성화합니다.\n" +
            "   파란색 = 켜짐 / 빨간색 = 꺼짐\n\n" +
            "【 마이크 On / Off 】\n" +
            "   선택한 마이크 장치의 음소거를 켜거나 끕니다.\n" +
            "   파란색 = 켜짐 / 빨간색 = 음소거\n\n" +
            "【 종료 】\n" +
            "   트레이로 숨기거나 프로그램을 완전 종료하는 선택창이 나타납니다.\n\n" +
            "【 감시 시작 / 감시 중지 】\n" +
            "   카메라 상태를 주기적으로 감시합니다.\n" +
            "   이상 감지 시 알림을 전송하고 자동 화면 녹화를 시작합니다.\n\n" +
            "【 설정 】\n" +
            "   감시 주기, 알림, 로그, 화면 녹화 옵션을 조정합니다.\n\n" +
            "【 알림 설정 】\n" +
            "   텔레그램 봇 토큰·Chat ID, Discord 웹훅 URL을 설정합니다.\n\n" +
            "【 녹화 시작 / 녹화 중지 】\n" +
            "   수동으로 화면 녹화를 시작하거나 중지합니다.\n" +
            "   설정에서 화면 녹화를 활성화해야 작동합니다.");

        AppendSeparator();

        AppendHeader("알림 설정 개요");
        AppendBody(
            "메인 화면 '알림 설정' 버튼을 클릭하면 알림 채널을 설정할 수 있습니다.\n\n" +
            "지원 채널\n" +
            "  • 텔레그램 — 봇을 통해 스마트폰으로 즉시 알림\n" +
            "  • Discord  — 웹훅 URL로 Discord 채널에 알림\n\n" +
            "알림이 전송되는 6가지 이벤트\n" +
            "  ① 🟢 감시 시작\n" +
            "  ② ⚠️  웹캠 장치 활성화 감지\n" +
            "  ③ 🔴 웹캠 장치 비활성화 감지\n" +
            "  ④ 📷 앱이 웹캠 사용 시작 (Zoom, Teams 등)\n" +
            "  ⑤ ⏹️  앱이 웹캠 사용 종료\n" +
            "  ⑥ 🔴 감시 종료 / 프로그램 종료\n\n" +
            "설정 → '일반' 탭에서 아래 알림 옵션을 조정할 수 있습니다.\n" +
            "  • 트레이 풍선 알림 표시 — PC 화면 우하단 팝업 on/off\n" +
            "  • 감시 시작·종료 알림 전송 — ①⑥ 이벤트 알림 on/off\n\n" +
            "각 채널은 독립적으로 활성화·비활성화할 수 있으며,\n" +
            "'테스트 전송' 버튼으로 연결을 즉시 확인할 수 있습니다.");

        AppendSeparator();

        AppendHeader("텔레그램 봇 준비 방법");
        AppendBody(
            "① Bot Token 발급\n" +
            "   텔레그램에서 @BotFather 를 검색해 채팅을 시작합니다.\n" +
            "   /newbot 명령 입력 후 안내에 따라 봇 이름을 설정하면\n" +
            "   Bot Token (예: 123456:ABC-DEF...) 이 발급됩니다.\n\n" +
            "② Chat ID 확인\n" +
            "   텔레그램에서 @userinfobot 을 검색해 /start 를 입력하면\n" +
            "   본인의 Chat ID (예: 123456789) 를 확인할 수 있습니다.\n\n" +
            "③ 앱에서 설정\n" +
            "   '알림 설정' → '텔레그램' 탭 → Bot Token과 Chat ID 입력 후\n" +
            "   '테스트 전송'으로 연결을 확인하세요.");

        AppendSeparator();

        AppendHeader("Discord 웹훅 설정 방법");
        AppendBody(
            "① Discord 서버에서 채널 설정 열기\n" +
            "   알림을 받을 채널 옆 톱니바퀴(⚙) 클릭\n" +
            "   → '연동' 탭 → '웹후크' 클릭\n\n" +
            "② 웹훅 만들기\n" +
            "   '새 웹후크' 버튼 클릭\n" +
            "   이름을 설정하고 '웹후크 URL 복사' 클릭\n\n" +
            "③ 앱에서 설정\n" +
            "   '알림 설정' → 'Discord' 탭\n" +
            "   복사한 Webhook URL 붙여넣기\n" +
            "   'Discord 알림 활성화' 체크 후 '테스트 전송'으로 확인\n" +
            "   '확인' 버튼으로 저장");

        AppendSeparator();

        AppendHeader("웹캠 감시 기능");
        AppendBody(
            "감시 중에는 여섯 가지 상황을 감지합니다:\n\n" +
            "  • 감시 시작\n" +
            "  • 카메라 장치가 활성화될 때\n" +
            "  • 카메라 장치가 비활성화될 때\n" +
            "  • 앱이 카메라를 사용하기 시작할 때\n" +
            "    (Zoom, Teams, 브라우저 등)\n" +
            "  • 앱이 카메라 사용을 종료할 때\n" +
            "  • 감시 종료 / 프로그램 종료\n\n" +
            "감지 시 트레이 풍선 알림과 활성화된 알림 채널\n" +
            "(텔레그램/Discord)으로 즉시 전송합니다.");

        AppendSeparator();

        AppendHeader("감시 설정");
        AppendBody(
            "메인 화면 '설정' 버튼을 클릭하면 아래 항목을 조정할 수 있습니다.\n\n" +
            "① 감시 주기 (1~10초, 기본 3초)\n" +
            "   카메라 상태를 확인하는 간격을 설정합니다.\n\n" +
            "② 트레이 풍선 알림 표시\n" +
            "   PC 화면 우하단 팝업을 끄고 원격 알림만 받을 수 있습니다.\n\n" +
            "③ 감시 시작·종료 알림 전송\n" +
            "   감시 시작/중지/프로그램 종료 시 알림을 별도로 켜고 끌 수 있습니다.\n\n" +
            "④ 날짜별 로그 저장\n" +
            "   이벤트 발생 시각을 날짜별 텍스트 파일에 기록합니다.\n" +
            "   저장 위치: 사용자\\AppData\\Local\\WebCamControl\\logs\\\n" +
            "   '폴더 열기'로 탐색기에서 바로 확인,\n" +
            "   '오늘 로그 보기'로 앱 내에서 바로 확인할 수 있습니다.");

        AppendSeparator();

        AppendHeader("화면 녹화");
        AppendBody(
            "웹캠이 사용되는 순간 화면을 자동으로 녹화하거나,\n" +
            "메인 화면의 '녹화 시작' 버튼으로 수동 녹화할 수 있습니다.\n\n" +
            "설정 → '화면 녹화' 탭에서 아래 항목을 조정합니다.\n\n" +
            "① 웹캠 감지 시 화면 자동 녹화\n" +
            "   카메라 사용이 감지되면 자동으로 화면 녹화를 시작합니다.\n\n" +
            "② 녹화 대상 모니터\n" +
            "   여러 모니터 환경에서 녹화할 화면을 선택합니다.\n\n" +
            "③ 화질 (낮음 5fps / 보통 10fps / 높음 15fps)\n" +
            "   높을수록 파일 용량이 커집니다.\n\n" +
            "녹화 파일 저장 위치:\n" +
            "   사용자\\AppData\\Local\\WebCamControl\\recordings\\\n" +
            "   형식: yyyy-MM-dd_HH-mm-ss_태그_screen.avi");

        AppendSeparator();

        AppendHeader("실시간 자막 / 번역 개요");
        AppendBody(
            "시스템 오디오(스피커 출력)를 실시간으로 인식해\n" +
            "화면에 자막 오버레이로 표시합니다.\n\n" +
            "  • 메인 화면 '자막 시작' 버튼 → 오버레이 창이 나타납니다.\n" +
            "  • 오버레이는 마우스로 이동·크기 조절이 가능합니다.\n" +
            "  • 인식 중인 텍스트(회색)는 하단에, 확정된 자막은 위에 누적됩니다.\n" +
            "  • 번역 ON : 원문(밝은 회색) + 번역(노란색)으로 표시됩니다.\n" +
            "  • 번역 OFF : 원문만 흰색으로 표시됩니다 (거의 실시간).\n\n" +
            "자막 로그 저장\n" +
            "  설정 → '번역' 탭 → '자막 로그 저장' 체크 시\n" +
            "  원문·번역문이 날짜별 파일에 기록됩니다.\n" +
            "  저장 위치: 사용자\\AppData\\Local\\WebCamControl\\subtitle_logs\\\n" +
            "  '폴더 열기' 버튼으로 탐색기에서 바로 확인할 수 있습니다.");

        AppendSeparator();

        AppendHeader("Vosk 음성 인식 모델 설치 방법");
        AppendBody(
            "Vosk는 인터넷 없이 동작하는 오프라인 음성 인식 엔진입니다.\n" +
            "모델 파일을 직접 다운로드해서 경로를 등록해야 합니다.\n\n" +
            "① 모델 다운로드\n" +
            "   설정 → '번역' 탭 → 'Vosk 모델 다운로드' 링크 클릭\n" +
            "   또는 아래 주소에서 직접 다운로드합니다.\n" +
            "   https://alphacephei.com/vosk/models\n\n" +
            "② 권장 모델\n\n" +
            "   【 한국어 】\n" +
            "   vosk-model-small-ko-0.22  (약 82 MB)\n" +
            "     → 소형, 빠른 응답, 일상 대화·방송 자막에 적합\n\n" +
            "   【 영어 — 소형 】\n" +
            "   vosk-model-small-en-us-0.15  (약 40 MB)\n" +
            "     → 매우 가볍고 빠름, 명확한 발음 위주 환경 권장\n\n" +
            "   【 영어 — 고품질 】\n" +
            "   vosk-model-en-us-0.22  (약 1.8 GB)\n" +
            "     → 높은 인식률, 다양한 억양·어휘 지원\n\n" +
            "③ 압축 해제 및 등록\n" +
            "   다운로드한 .zip 파일을 원하는 폴더에 압축 해제합니다.\n" +
            "   예) C:\\Vosk\\vosk-model-small-ko-0.22\n\n" +
            "   설정 → '번역' 탭 → '모델 추가' 버튼\n" +
            "   모델 이름(자유 입력)과 압축 해제한 폴더 경로를 입력합니다.\n\n" +
            "④ 활성 모델 전환\n" +
            "   목록에서 사용할 모델을 클릭하면 파란색으로 활성화됩니다.\n" +
            "   여러 모델을 등록해 두고 언어에 따라 즉시 전환할 수 있습니다.");

        AppendSeparator();

        AppendHeader("DeepL API 키 발급 방법");
        AppendBody(
            "DeepL Free API는 월 500,000자까지 무료로 번역을 제공합니다.\n" +
            "신용카드 등록 없이 이메일만으로 가입할 수 있습니다.\n\n" +
            "① DeepL 계정 생성\n" +
            "   https://www.deepl.com/ko/pro-api 접속\n" +
            "   'Free 플랜' 또는 'API 무료 등록' 버튼 클릭\n" +
            "   이메일 주소로 계정을 만들고 이메일 인증을 완료합니다.\n\n" +
            "② API 키 확인\n" +
            "   로그인 후 https://www.deepl.com/account/summary 접속\n" +
            "   페이지 하단 'Authentication Key for DeepL API' 항목에서\n" +
            "   키를 복사합니다.\n\n" +
            "   Free 키 형태 예시:\n" +
            "   xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx:fx\n" +
            "   (끝에 ':fx' 가 붙으면 Free 플랜입니다)\n\n" +
            "③ 앱에서 등록\n" +
            "   설정 → '번역' 탭 → 'DeepL 번역 사용' 체크\n" +
            "   API Key 입력란에 복사한 키를 붙여넣고 '확인'으로 저장합니다.\n" +
            "   '사용량 조회' 버튼으로 이번 달 남은 한도를 확인할 수 있습니다.\n\n" +
            "④ 한도 초과 시\n" +
            "   월 500,000자를 모두 소진하면 번역이 자동으로 중단되고\n" +
            "   자막만 계속 표시됩니다. 다음 달 1일에 자동으로 초기화됩니다.");

        AppendSeparator();

        AppendHeader("시작 프로그램 관리");
        AppendBody(
            "Windows 설정 → 앱 → 시작 프로그램 에서\n" +
            "'WebCamControl' 항목을 켜거나 끌 수 있습니다.\n\n" +
            "처음 실행 시 묻는 질문에서 '아니요'를 선택했어도\n" +
            "위 경로에서 언제든지 등록할 수 있습니다.");

        AppendSeparator();

        AppendHeader("만든 사람");
        AppendBody(
            "제작자: jaeho\n" +
            "버전:   v1.06.01\n" +
            "메일:   jaeho9697@gmail.com");

        rtb.SelectionStart = 0;
    }

    private void AppendHeader(string text)
    {
        rtb.SelectionFont  = new Font("맑은 고딕", 10F, FontStyle.Bold);
        rtb.SelectionColor = Color.DarkSlateBlue;
        rtb.AppendText(text + "\n");
        rtb.SelectionFont  = new Font("맑은 고딕", 9F);
        rtb.SelectionColor = Color.Black;
    }

    private void AppendBody(string text)
    {
        rtb.SelectionFont  = new Font("맑은 고딕", 9F);
        rtb.SelectionColor = Color.Black;
        rtb.AppendText(text + "\n");
    }

    private void AppendSeparator()
    {
        rtb.SelectionFont  = new Font("맑은 고딕", 8F);
        rtb.SelectionColor = Color.Gray;
        rtb.AppendText("\n──────────────────────────────\n\n");
    }

    private void BtnClose_Click(object? sender, EventArgs e) => Close();
}
