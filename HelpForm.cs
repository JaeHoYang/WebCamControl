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
            "버전:   v1.05\n" +
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
