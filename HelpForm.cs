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

        AppendHeader("카카오톡 준비 방법");
        AppendBody(
            "① Kakao Developers 앱 등록\n" +
            "   developers.kakao.com → 내 애플리케이션 → 애플리케이션 추가\n" +
            "   앱 이름 설정 후 REST API 키를 복사합니다.\n\n" +
            "② 카카오 로그인 활성화\n" +
            "   앱 설정 → 카카오 로그인 → 활성화 ON\n" +
            "   Redirect URI 에 다음 주소를 추가합니다:\n" +
            "   http://localhost:9697/\n\n" +
            "③ 동의항목 설정\n" +
            "   앱 설정 → 동의항목 → 카카오톡 메시지 전송 → 선택 동의\n\n" +
            "④ 앱에서 설정\n" +
            "   '알림 설정' → '카카오톡' 탭 → REST API 키 입력 후\n" +
            "   '카카오 로그인' 클릭 → 브라우저에서 로그인 완료");

        AppendSeparator();

        AppendHeader("웹캠 감시 기능");
        AppendBody(
            "감시 중에는 네 가지 상황을 감지합니다:\n\n" +
            "  • 카메라 장치가 활성화될 때\n" +
            "  • 카메라 장치가 비활성화될 때\n" +
            "  • 앱이 카메라를 사용하기 시작할 때\n" +
            "    (Zoom, Teams, 브라우저 등)\n" +
            "  • 앱이 카메라 사용을 종료할 때\n\n" +
            "감지 시 트레이 풍선 알림과 활성화된 알림 채널\n" +
            "(텔레그램/카카오톡)으로 즉시 전송합니다.");

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
            "버전:   v1.03\n" +
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
