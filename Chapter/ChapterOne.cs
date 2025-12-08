using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doublsb.Dialog;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using TMPro;
using UnityEngine.SceneManagement;

public class ChapterOne : MonoBehaviour
{
    //디버깅용
    public float timescales = 5;
    [SerializeField]
    private GameObject walloftutoPanel;
    public GameObject optionPanel;

    public event EventHandler FadeInOutPlay; //이벤트 정의
    public event EventHandler FinderMousePointerChange;
    public event EventHandler BasicMousePointer;

    //심문 이벤트
    public NoteItemsManager SubmitEvent;
    public GameObject SubmitPanel;
    private string submitTargetName = ""; // 심문 시 타깃 이름 체크

    //단어 맞히기 이벤트
    public WordQuizSystem wordQuizSystemEvent;
    public bool[] wordQuizNum = new bool[3];

    public DialogManager DialogManager;
    public GameObject[] cluePanel;
    public Image stateIcon;
    public GameObject[] Background;

    public GameObject DialogPanel;
    public GameObject FinderPanel;
    public GameObject FinderObjectPanel;
    public GameObject WordQuizPanel;
    public GameObject WordQuizPanel_02;
    public GameObject WordQuizPanel_03;

    //마음의 벽 이벤트
    public WallofInquirySystem wallofInquirySystem;
    public GameObject WallofInquiryManager;
    public GameObject WallofInquiryPanel;
    public GameObject WallofInquiryMarcelPanel;

    //마물 정체 맞히기 이벤트
    public DemonIdentitySystem demonIdentitySytem;
    public GameObject DemonIdentityPanel;
    public GameObject MarcelDemonIdentityPanel;

    //리네 cg 이미지 이벤트
    public GameObject RineVampPanel;
    public GameObject RucyCGPnael;

    //루시 직소 퍼즐 이벤트
    public GameObject ZigsawPuzzleRucyPanel;
    public GameObject ZigsawPuzzleSystemPanel;
    public ZigsawPuzzleSystem zigsawPuzzleSystem;

    // 단서 획득 프린터 투명화를 위한 변수 모음
    public Image printerImage;
    public Image namepanelmage;
    public TextMeshProUGUI namepanelText;

    [SerializeField]
    private Sprite[] backSprites;
    [SerializeField]
    private Sprite[] researchSprites;
    [SerializeField]
    private Sprite[] finderSprites;
    [SerializeField]
    private Sprite[] stateChangeIcons;

    public GameObject[] items = new GameObject[8];
    public GameObject[] submititems = new GameObject[8];
    public GameObject deepfinderIcon; // 냄새 수사 버튼

    private int itemsCount = 6;




    //표준 이벤트 구독자 시그니처
    public void Test_FadeInOutPlay(object sender, EventArgs eventArgs)
    {
        Debug.Log("페이드 인아웃 재생");
    }

    //마우스 포인트 변경 이벤트 구독자 시그니처
    public void FinderBasicChanger(object sender, EventArgs eventArgs)
    {
        Debug.Log("찾기 마우스 포인터 기본 변경");
    }

    public void BasicMouseChanger(object sender, EventArgs eventArgs)
    {
        Debug.Log("기본 마우스 포인터 연결");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            DialogManager.Click_Window();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (walloftutoPanel.activeSelf)
            {
                walloftutoPanel.SetActive(false);
                StartWallOfInquiry();
                return;
            }

            optionPanel.SetActive(!optionPanel.activeSelf);
        }


    }

    private void StructCollect()
    {
        //이벤트 관련
        SubmitEvent = SubmitEvent.GetComponent<NoteItemsManager>();
        wordQuizSystemEvent = wordQuizSystemEvent.GetComponent<WordQuizSystem>();
        wallofInquirySystem = wallofInquirySystem.GetComponent<WallofInquirySystem>();

        // 티모테 리네 단서 제출 파트 구독

        SubmitEvent.TimoteSubmit += TimoteSumbit; //구독
        SubmitEvent.TimoteErrorSubmit += TimoteErrorSumbit; // 구독 2
        SubmitEvent.RineSubmit += RineSubmit; //구독
        SubmitEvent.RineErrorSubmit += RineErrorSumbit; // 구독 2

        // 문장 완성 구독
        wordQuizSystemEvent.First_WordComplete += F_WordQuizText;
        wordQuizSystemEvent.Secon_WordComplete += S_WordQuizText;
        wordQuizSystemEvent.Third_WordComplete += T_WordQuizText;
        wordQuizSystemEvent.Error_Word += Error_WordQuizText;

        // 전, 후반부 리네 마르셀 추리 단서 제출 파트 구독

        SubmitEvent.RineCCTVSubmit += RineCCTVSubmit;
        SubmitEvent.RineReporterSubmit += RineReporterSubmit;
        SubmitEvent.RineBrokenElevatorSubmit += RineBrokenElevatorSubmit;
        SubmitEvent.RineOpenWindowSubmit += RineOpenWindowSubmit;
        SubmitEvent.RineMultipleSubmitError += RineMultipleSubmitError;

        SubmitEvent.RineVampBloodyPackSubmit += RineVampBloodyPackSubmit;
        SubmitEvent.RineVampVictimInfoSubmit += RineVampVictimInfoSubmit;
        SubmitEvent.RineVampSubmitError += RineVampSubmitError;

        SubmitEvent.MarcelBloodyPackSubmit += MarcelBloodyPackSubmit;
        SubmitEvent.MarcelAnoSuspectSubmit += MarcelAnoSuspectSubmit;
        SubmitEvent.MarcelRinesMaskSubmit += MarcelRinesMaskSubmit;
        SubmitEvent.MarcelAnomalySmellSubmit += MarcelAnomalySmellSubmit;
        SubmitEvent.MarcelSubmitError += MarcelSubmitError;

        // 마음의 벽 정답 파트 구독
        wallofInquirySystem.RineCorrectWallofInquiry += WhoisRine;
        wallofInquirySystem.MarcelCorrectWallofInquiry += WhoisMarcel;

        // 마물 정체 파트 구독
        demonIdentitySytem.ThisisRine += ThatisRine;
        demonIdentitySytem.ThisisRucy += ThatisMarcel;

        //직소 퍼즐 파트 구독
        zigsawPuzzleSystem.ZigsawRucyEnd += FindRucy;

    }


    private void Awake()
    {

        SoundManager.instance.BGMChgnaer("BackAlley");
        StructCollect(); // 이벤트 구독, 컴포넌트들 모음


        var dialogTexts = new List<DialogData>();


        #region HotelLobby




        dialogTexts.Add(new DialogData("/emote:Latte_Left+Smile/조수~! 조수~! 여기가 호텔이라는 곳이야! 우와~ 나 이런 곳은 처음이야!", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Ridic/탐정님, 저희는 놀러 온 게 아닙니다. 살인 사건을 수사하러 온 거죠.", "Latte_Left + Siwoo_Right", "시우"));



        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Ridic/알아~ 그런데 이런 멋진 곳에 왔는데, 그냥 살인범만 잡고 떠나면 아쉽잖아? /emote:Latte_Left+FSmile/다 끝나고 여기서 쉬고 가자!", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/...사건부터 잘 마무리하시면 고려해 보겠습니다. \r\n", "Latte_Left + Siwoo_Right", "시우"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/진짜?? 진짜지?? 약속했어, 조수! 이번 사건까지 멋지게 끝내면, 호텔에서 1박2일이야!\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/나 호텔 침대에서 뛰어보고 싶고~ 맛있는 조식 뷔페도 가보고 싶고~. 그리고 또......\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Ridic/하아... 탐정님. 곧 밀령사의 직원이 올 거라고 했으니까, 일에 집중하시죠.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Normal/으엑... 밀령사? 그 탈 쓴 녀석들? 난 그놈들은 싫은데.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Normal/하지만 수사를 하려면 그쪽의 협력이 필요하잖아요... 제발 참아주세요\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Emb//emote:Siwoo_Right+Normal/조수가 그렇게까지 부탁한다면야...\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/물론 기분 나쁘게 생겼고, 냄새도 이상하고, /emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Ridic/말하는 것도 짜증 나고, 그쪽도 날 싫어하지만...\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/조수가 부탁했으니까 참아볼게! 대신 이따가 많이 칭찬해 줘!\r\n", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/네네, 알겠습니다. 마침 저쪽에서도 도착한 모양이네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal/너를 다시 보는 건 언제나 유쾌하지 않군.\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "밀령사 눈"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+FNormal/흥! 나도 너희 보기 싫지만, 우리 조수가 부탁해서 참는 거거든?!\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "라떼",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[2];
                SoundManager.instance.BGMMuteOnOff(true);
            }));

        dialogTexts.Add(new DialogData("(밀령사(密靈司)...)\r\n", "", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Observer_Center+Normal/(항상 저런 가면을 쓰고 다니는 정체불명의 집단. 일단은 나와 이해관계가 일치해서 일시적 협력하고는 있는 곳.)\r\n", "Observer_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Observer_Center+Normal/(인간 세상에 넘어온 마물들이 안전하고 평화롭게 생활하도록 도와주는 조직이라고 소개하고 있지만...)\r\n", "Observer_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Observer_Center+Normal/(말만 들으면 좋아 보이지, 실상은 마물들을 감시하고 문제가 생기면 바로 추방시키는 놈들.)\r\n", "Observer_Center", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Observer_Center+Normal/(그리고 오늘처럼 인간 사이에 숨은 마물이 저지른 사건을 관리하는 놈들.)", "Observer_Center", "라떼",
             () =>
             {
                 Background[0].GetComponent<Image>().sprite = backSprites[0];
                 SoundManager.instance.BGMMuteOnOff(false);
             }));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal/입은 여전하군. 아직도 정식 테스트를 받지 않은 마물을 무조건 신뢰할 수는 없다.\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal/다만, 너의 능력이 꽤나 쓸모가 있어서 수장님께서 특별히 봐주시는 거다. 우리를 실망시키지 말아라.\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "밀령사 눈"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal//sound:female_growl/으으으... 으르릉...", "Latte_Left + Siwoo_Center + Observer_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+Ridic//emote:Observer_Right+Normal/탐정님... 참으세요. 탐정님이 수사하실 수 있는 것도 저 분들 덕분이잖아요.\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "시우"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Center+Ridic//emote:Observer_Right+Normal/.../emote:Latte_Left+FAnnoy/좋아, 그래서 이번에는 무슨 사건이야? 빨리 본론으로 들어가자고.\r\n", "Latte_Left +Siwoo_Center+ Observer_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal/......그건 직접 가서 확인하도록. 간단한 정보 정도는 따로 전달하지.", "Latte_Left+Siwoo_Center + Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Center+FNormal//emote:Observer_Right+Normal/이번에도 제대로 인간들 사이에 숨은 마물을 찾아내도록.", "Latte_Left+Siwoo_Center + Observer_Right", "밀령사 눈"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Center+FSmile//emote:Observer_Right+Normal/네, 감사합니다. 그 정보는 제게 주시면 됩니다.\r\n", "Latte_Left + Siwoo_Center + Observer_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/그럼 시신이 있는 방만 조사하고 빠르게 지하로 와라. 호텔 전체에 결계는 치지 못했으니까 말이지.", "Latte_Left + Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Observer_Right+Normal/(결계... 우리 모습이 의심 받지 않도록 만드는 그 인지 저해 결계를 말하는 거구나.)\r\n", "Latte_Left + Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/이봐, 애완견. 미리 말하지만 너는 아직 요주의 마물이다. 이번 사건에서 엉뚱한 녀석을 마물이라고 체포하면......", "Latte_Left + Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/우리 밀령사는 인류의 안전을 위해 너를 마계로 추방시키겠다. 주의하도록.\r\n", "Latte_Left + Observer_Right", "밀령사 눈"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Ridic/...이제 갔네요. /emote:Siwoo_Right+Smile/잘 참으셨어요 탐정님.\r\n", "Latte_Left + Siwoo_Right", "시우"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Smile/으으으.... 진짜 짜증 나서 목을 꽉! 물고 싶기는 했지만, /emote:Latte_Left+FNormal/조수랑 약속했으니까.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Smile/그러니까 칭찬해 줘! 나 착하지?? 기특하지??\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Smile/네, 잘하셨어요. 이 기세로 사건까지 끝내서 추방 당할일은 없도록 하죠.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Smile//emote:Siwoo_Right+Normal/좋아! 반드시 범인을 제대로 찾아내서, 평생 조수의 집에서 살 거야!!\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Center+Ridic/...제 의견은 듣지도 않고 정하는 건가요./sound:walking/\r\n", "Siwoo_Center", "시우",
            () =>
            {
                StartCoroutine(TalkPrinterOnOFF());
                StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));
                FadeInOutPlay(this, EventArgs.Empty); // 이벤트 핸들러들을 호출
                Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 화면

            }));


        #endregion




        DialogManager.Show(dialogTexts);



    }




    private void HotelHallWay()
    {
        Background[0].GetComponent<Image>().sprite = backSprites[1]; // 호텔 복도
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//sound:walking/그래서 조수! 아까 그 녀석한테 받은 종이 있잖아. 거기에 뭐라고 적혀 있어?", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/피해자의 정보와 복도 CCTV 기록이 적혀있네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/ /speed:0.1/피해자는 \"/color:red//speed:0.5/에이든/color:white//speed:0.1/\"이라는 27세 남성이며, 이 호텔에 \"/color:red//speed:0.5/티모테/color:white//speed:0.1/\"라는 인물과 함께 숙박하고 있었다고 합니다.", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/ 키는 184cm에 몸무게는 76kg... 혈액형은 AB형... 나머지는 이런 정보들 뿐이네요.\r\n", "Latte_Left + Siwoo_Right", "시우",
            () => { Show_Clue(0); items[0].SetActive(true); submititems[0].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText)); })); // <단서 : "피해자의 정보" 획득>
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal//wait:2/에이~ 그런 자잘한 정보는 넘어가고! 가장 중요한 CCTV 기록은??\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Think/CCTV 기록에 따르면 피해자가 마지막으로 살아있는 모습으로 찍혔던 시각은 22시 38분. 자신의 방으로 돌아가는 모습이었다고 하네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/ 그리고 23시 12분에 \"/color:red//speed:0.5/리네/color:white//speed:0.1/\"라는 이름의 인물이 피해자의 방에 들어가는 모습이 찍혔고요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/리네? 같은 방을 쓰는 사람은 티모테 아니었어?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/리네는 이 호텔의 의무실에서 근무하는 의사라고 합니다. 호텔에 응급 환자가 나오면 객실로 직접 찾아가기도 한다네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/흐음~ 그렇구나? /emote:Latte_Left+FExc/ 그리고 다음은?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/이후 23시 13분에 티모테가 객실로 복귀를 했고...\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/23시 14분에 \"마르셀\"이라는 인물이 그 앞을 지나가다가 뭔가 느꼈는지 당황하는 모습이 찍혔고, 직후 프런트에 신고.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/이후 근처에 있던 밀령사가 용의자들을 구속하면서 시체를 확보한 것이 23시 16분이라고 합니다.\r\n", "Latte_Left + Siwoo_Right", "시우"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/신기하네~ 거의 동시에 딱딱딱 들어오다니. CCTV에 더 내용은 없어?", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Smile/네, 어째서인지 그 앞 부분도, 뒷부분도 짤린 상태로 받았어요. 그래도 밀령사에서 직접 감시하던 기록이니까 신뢰성은 충분해요.", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Smile/(...? 호텔의 CCTV 영상을 구한게 아니라 직접 감시하던 기록이라고? 사건 발생 전부터 뭘 감시하고 있던 거지...?)", "Latte_Left + Siwoo_Right", "라떼"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Smile/(마치 호텔에 뭔가 있다는 뜻처럼 들리는데.)", "Latte_Left + Siwoo_Right", "라떼",
                  () => { Show_Clue(1); items[1].SetActive(true); submititems[1].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText)); })); // <단서: "CCTV 기록" 획득>
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Exc//emote:Siwoo_Right+Normal//wait:2/그럼 슬슬 들어갈까? 실례하겠습니다~./sound:door_open/", "Latte_Left + Siwoo_Right", "라떼",
             () =>
             {
                 // 대화 끝난 후 콜백

                 FadeInOutPlay(this, EventArgs.Empty); // 이벤트 핸들러들을 호출 

                 Invoke(nameof(SwitchResearchBackground), 0.7f);
                 SoundManager.instance.BGMChgnaer("waste");
                 // 피해자의 객실 시작, 수색 이벤트 이전

             }
        ));


        dialogTexts.Add(new DialogData("/emote:Siwoo_Right+Surp//sound:ding/......!", "Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Right+Surp/......이 시체, 마치 미라처럼 말라버렸네요./click/ ", "Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Right+Think/이렇게까지 끔찍하게 죽다니... 도대체 누가... /click/ ", "Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Exc//emote:Siwoo_Right+Think/와아~!! 진짜네?? 방금 전에 죽었는데 이렇게 되다니, 신기하다~!\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Exc//emote:Siwoo_Right+Annoy/탐정님... 신기하다고 웃을 일이 아닙니다.\r\n", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Annoy/아... 그래? /emote:Latte_Center+FEmb/하하, 인간들은 이런 거를 신기하게 보지 않는구나.\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Annoy/음! 덕분에 하나 더 배웠어. /emote:Latte_Center+FSmile/역시 우리 조수는 똑똑해!\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Ridic/....기쁘다는 표정도 하지 마세요. \r\n", "Latte_Center + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Ridic/아, 알겠어. 이제 진짜 진지하게 할게.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Ridic/하아... 얼른 수사를 시작하죠. 이분을 위해서라도 범인을 잡아야 해요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal//sound:ding/범\"/color:red/인/color:white/\"이라고 하기에는 조금 다르지만 말이지~.", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Ridic/그럼 시작해 볼까? 이미 떠나간 영혼을 위해, 그리고... 우리를 위해서!\r\n", "Latte_Left + Siwoo_Right", "라떼",

             () =>
             {
                 // 대화 끝난 후 콜백

                 FadeInOutPlay(this, EventArgs.Empty); // 이벤트 핸들러들을 호출 
                 SwitchResearchBackground();
                 StartFinderSystem();
                 PrinterOnOff(false);
                 SoundManager.instance.BGMChgnaer("Thereshould");

             }
        ));





        DialogManager.Show(dialogTexts);
    }


    private void HotelInquiry()
    {
        Background[0].GetComponent<Image>().sprite = backSprites[0]; // 호텔 로비 변경
        var dialogTexts = new List<DialogData>();


        dialogTexts.Add(new DialogData("/emote:Latte_Right+Smile//sound:walking/흐흥~ 조수는 잘 해주고 있겠지??", "Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Right+Smile/나도 탐정으로서 가만히 있을 수는 없지~ 용의자들 냄새를 싹! 맡아보고 파바박! 잡아야지!\r\n", "Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Right+Normal/그래서... 그 눈알이 말했던 계단이 여기 어딘가였는데...\r\n", "Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Right+Exc/아! 여기 있다, 지하실 계단! 왜 굳이 여기로 했는지는 모르겠지만, 얼른 내려가 보자./sound:walking/\r\n", "Latte_Right", "라떼",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 화면 변경

            }));
        dialogTexts.Add(new DialogData("/sound:walkingup/(으... 내려갈 수록 몸이 무거워... 얼마나 결계를 강하게 친 거야?)\r\n", "", "라떼"));
        dialogTexts.Add(new DialogData("/sound:walkingup/(이 정도로 심하면 나도 힘든데! 코가 벌써 먹먹하잖아.)\r\n", "", "라떼"));
        dialogTexts.Add(new DialogData("(이쪽에서 특히 강한 결계가 느껴져. 여기가 용의자들을 격리한 공간인 게 분명해.)/sound:walking/\r\n", "", "라떼",
            () =>
            {
               

                SoundManager.instance.BGMChgnaer("japansong"); // 심문 파트 bgm 변경
                SoundManager.instance.BGMMuteOnOff(false);

            }));
        dialogTexts.Add(new DialogData("/emote:Observer_Right+Normal/...늦었다. 산책이라도 다녀온 거냐?\r\n", "Observer_Right", "밀령사 눈\r\n", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[3]; // 지하실 변경
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/(으엑... 결계 때문에 기분 나쁜데, 하필 이 녀석을 먼저 보네.)\r\n", "Latte_Left+Observer_Right", "라떼\r\n"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/속마음이 표정에 다 드러날 거면 그냥 말하지 그러나.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/너가 싫은 것도 싫은 거지만... /emote:Latte_Left+FAnnoy/여기는 뭐야?! 왜 이렇게 결계를 강하게 했어??", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/이 정도로 강하면 나도 힘들다고!!", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/당연한 조치다. 용의자 중에는 마물이 섞여있다. 그리고 언제 본성을 드러낼지 모르지.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/결계를 강하게 해서 힘을 억제시키지 않으면 무고한 인간 뿐만 아니라, 이 호텔의 외부인들까지 피해를 입을 수 있다.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/그치만... 이러면 내 코도 먹먹해지는데...", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/왜 그렇게 과민반응하지? 설마 '코'를 못 쓰면 범인도 못 잡는 건가?", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Observer_Right+Normal/아, 아니거든?! 난 우리 조수가 자랑스럽게 여기는 천재 탐정이라고!!", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Observer_Right+Normal/(으으으... 원래는 냄새로 빠르게 인간이 아닌 녀석을 찾으려고 했는데...)", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/(아니야, 나는 조수가 믿는 탐정이잖아? 나를 믿어주는 만큼 나도 노력해야지.)", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/이제 슬슬 심문을 시작하려고 하는데, 용의자들은 저기 저 사람들이야?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("(아까부터 신경 쓰였던 방 구석에 있는 사람들.)", "", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Normal//emote:Rine_Center+Normal//emote:Marcel_Right+Normal/(저 3명이 용의자인 거지?)", "Timote_Left+Rine_Center+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Annoy//emote:Rine_Center+Normal//emote:Marcel_Right+Normal/대체 얼마나 기다려야 하는 건지. 더 말할 것도 없는데, 빨리 끝내면 안 되는 거냐고.", "Timote_Left+Rine_Center+Marcel_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Annoy//emote:Rine_Center+Normal//emote:Marcel_Right+Smile/하하, 너무 그렇게 찌푸리지 마세요. 이왕 이렇게 된 거, 색다른 경험이라고 생각하면 편하지 않나요?", "Timote_Left+Rine_Center+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Annoy//emote:Rine_Center+Normal//emote:Marcel_Right+Normal/살인 사건의 용의자라니. 그것도 우리 중에 괴물이 있다면서요?", "Timote_Left+Rine_Center+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Annoy//emote:Rine_Center+Normal//emote:Marcel_Right+Smile/이런 신기한 일에 평생 엮일 일이 얼마나 있겠어요~.", "Timote_Left+Rine_Center+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Annoy//emote:Rine_Center+FSurp//emote:Marcel_Right+Smile/저, 저기... 아무리 그래도... 저쪽 친구분이 돌아가셨는데... 그런 말씀은...", "Timote_Left+Rine_Center+Marcel_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Normal//emote:Rine_Center+FSurp//emote:Marcel_Right+Smile/아니, 상관없어. 언제 칼에 찔려 죽어도 이상하지 않을 놈이었거든. 그냥 때가 왔구나 싶은 정도.", "Timote_Left+Rine_Center+Marcel_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Normal//emote:Rine_Center+FSurp//emote:Marcel_Right+Exc/뭐~ 칼에 찔린 거는 아니지만요~. 아니면 칼이 사실 괴물 칼이었다거나?", "Timote_Left+Rine_Center+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Normal//emote:Rine_Center+Surp//emote:Marcel_Right+Smile/아으... 그... 저, 저기... 그...", "Timote_Left+Rine_Center+Marcel_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Timote_Left+Normal//emote:Rine_Center+Emb//emote:Marcel_Right+Smile/......네.", "Timote_Left+Rine_Center+Marcel_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/.......", "Latte_Left+Observer_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/응! 역시 인간들은 신기하네. 조수랑 정말 달라!", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/말 조심해라. 저기에는 진짜 인간도 있으니, 너희 마물 따위가 할 말은 아니다.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/가장 이상하게 생긴 너한테 듣고 싶지는 않거든?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/아무튼 지금부터 용의자들의 말을 듣고서 범인을 찾아내라.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(그래, 조수가 돌아오기 전까지 이야기를 한번 들어보자.)", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(결국 코는 못 쓰게 되었지만, 나는 탐정이니까. 잘 생각하면 마물을 찾을 수 있을 거야.)", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc/자! 그럼 시작해 보실까!", "Latte_Left", "라떼",

            () =>
            {
                stateIcon.sprite = stateChangeIcons[2]; // 심문 아이콘 변경
                TimoteInquiry();
            }));



        DialogManager.Show(dialogTexts);

    }

    private void TimoteInquiry()
    {
        SoundManager.instance.BGMChgnaer("Umwelt");
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/(이쪽이 피해자의 친구라는 \"티모테\")", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/(친구가 죽었는데도 생각보다 충격을 덜 받은 느낌이란 말이지.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/(표정에서부터 느껴지는 귀찮음. 그냥 빨리 끝내고 싶다는 마음이 얼굴에 다 보여.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Annoy/안녕! 내가 이번 사건 수사를 맡은 탐정이야.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Think/뭐야... 귀? 이상한 장식을 달고 다니는... 사람은 아닌 거 같고.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Normal/당신도 그쪽이야? 인간들 사이에 숨어 산다는 마물인가 뭔가.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Normal/오~ 생각보다 눈치가 빠른걸? 보통은 마물이란 게 있다는 사실도 믿지 못 하던데.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Annoy/쯧... 당연하지. 사람들 사이에 괴물이 숨어있다는데, 쉽게 믿을 수 있을 리가 없잖아.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Annoy/그런데 방금 전까지 쌩쌩하던 녀석이 말라서 뒈져있고, 저 괴상한 탈 쓴 사람이 부적으로 뭔가를 하니까 사람들이 우리를 못 보고.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Think/어쩌겄냐... 이미 이 상황 자체가 비현실적인데 믿어야지.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/흠흠, 빠르게 상황을 받아들이는 태도가 정말 좋아!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Think/그런데 의외로 침착하네? 같은 방을 쓰던 사람이 죽은 건데 슬프지 않아? 친구 사이 같은 거 아니야?", "Latte_Left+Timote_Right", "라떼"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/친구? ...그렇게 부르던 때가 있었지. 그 자식이 날 '도구' 정도로 생각했단 걸 알기 전까진.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Annoy/나 말고도 그 자식을 원망하는 사람이라면 널렸을 거야. 특히 여자들 중에서.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/분명 지옥이 있다면 지옥에 갈 놈이라고 생각해서 딱히? 그냥 때가 된 거였다고 봐.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(으음... 피해자는 생각보다 더 쓰레기인가 보네.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/그런 남자랑 왜 같은 방을 쓴 거야? 너도 걔 싫어하는 거 같은데.", "Latte_Left+Timote_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/쯧... 인정하기 싫지만, 그 자식 곁에 남은 마지막 바보가 나라서.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/뜬금없이 나한테 같이 여행을 가자고 연락이 먼저 왔어. 여자에 미친 놈이 남자랑 단 둘이 여행? 딱 봐도 수상하잖아.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/그런데... '이번엔 정말 다르다'길래 속아준 거야. 하, 내가 멍청했지. 사람은 안 변한다는 진리를 또 잊다니.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/괜히 따라왔다가 이런 이상한 일까지 휘말리고.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/왜? 무슨 일이 있었는데?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/사실 이번에도 여자를 만나려고 온 거였어. 데이트 매칭 어플에서 뭐라고 했더라... 루시라는 여자랑 만나기로 약속 했다고.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Annoy/웃기는 건 그 새끼, 지금 여친이 있어. 혼자 간다고 하면 의심받을까 봐 나랑 간다고 했다더라.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Annoy/말 그대로. 난 핑계를 위한 '미끼'였던 거지.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Timote_Right+Annoy/으윽... 어떤 사람인지 파악했으니까 이 이야기는 여기서 끝!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Annoy/그러니까 슬슬 본론으로 들어가서... 그 잘 죽은 남자의 시체가 발견된 당일에 뭘 하고 있었어?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Emb//sound:ding/...몰라. 기억 나질 않아.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Emb/응? 아무것도? CCTV 상으로는 시체가 발견되기 조금 전에 피해자의 방에 들어갔잖아?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Annoy/모른다고. 젠장... 나도 미치겠다고. 머릿속에 안개가 낀 것처럼 멍청해진 기분이야.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Annoy/(혹시 묵비권 행사인가? 내 코만 멀쩡했으면 거짓말인지 아닌지 단번에 알았을 텐데...!)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Annoy/(하지만 마물의 능력에 당해서라면 납득은 가. 기억을 만지는 녀석들도 있기는 하니까. 그럼 다르게 접근해보자.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/흐음... 그럼 마지막 기억이 뭔데? 그거 정도는 알려줄 수 있지?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/...몇 시였는지 기억 나질 않지만, 밤에 밖에서 담배를 피고 있었어. 당연히 혼자 있었고.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Think/혼자서? 피해자랑은 같이 있지 않았고?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/하, 퍽이나 그딴 녀석이랑 같이 있고 싶겠다.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/...이 호텔에 온 이후로 계속 혼자 돌아다녔어. 어차피 그 자식은 여자 만나고 있을 거고, 나도 그 새끼 얼굴 보기 싫으니까.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/그러면 호텔 도착 이후로 단 한 번도 피해자랑 접촉이 없었던 거네?", "Latte_Left+Timote_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/그래. 내 기억이 맞다면 말이지.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(응, 이 부분은 명백하게 내가 기억하고 있는 정보랑 \"모순\"되고 있어.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(진짜 기억이 나지 않는 건지, 연기하는 건지 아직은 모르겠지만. 발뺌 할 수 없는 확실한 \"단서\"를 보여주자)", "Latte_Left+Timote_Right", "라떼",
            () =>
            {
                stateIcon.sprite = stateChangeIcons[3]; // 추리 아이콘 변경
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "Timote"; //티모테 초기화
                SubmitPanel.SetActive(true);

            }));

        DialogManager.Show(dialogTexts);

    }

    private void MarcelInquiry()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink/(그래서 다음은... 이쪽 차례?)", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/(사건의 신고자인 \"마르셀\".)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/(피해자 방 앞을 지나가다가 신고했다던가? 어쩌다가 신고했는지 물어봐야겠어.)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/(그보다 이 사람은 뭐랄까... 다른 사람들이랑 다르게 즐거워 보이네?)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/반가워~ 내가 이번 사건을 맡은 탐정, 라떼야~.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/네가 마르셀 맞지? 이 사건의 신고자라고 들었어! 살인 사건에 엮인 사람 치고는 표정이 꽤 밝은걸?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/하하, 그야 신기하잖아요? ", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/갑자기 저희 중에 괴물이 숨어있다면서 조사를 받게 되었고... 지금 눈 앞의 탐정님도 정말 귀여운 강아지가 귀가 달렸죠.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/흐흥~ 그야 진짜니까! 우리 조수도 관심 없는 척하면서 은근슬쩍 만질 때가 있다니까?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/그리고... 신고했을 때는 시체를 못 봤지만, 아까 전에 다른 분들 이야기를 듣고 알게 됐어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Exc/세상에, 사람이 그런 식으로 죽었다니. 무섭다기 보다는 신기하지 않나요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/그치?? 나도 인간이 미라처럼 되는 건 처음 봐서 엄청 흥미로웠어!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/으으으... 그런데 조수가 그런 말은 하면 안 된다면서 혼났어...", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Observer_Center+Normal/.......", "Observer_Center", "밀령사"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Normal/아, 알겠어. 제대로 심문할 테니까 그렇게 보지 마!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/더 수다 떨다가는 진짜 혼날 수도 있겠다. 이제 시작할게.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/좋아요. 저도 탐정님한테 질문 받는 경우는 처음이어서 살짝 두근두근 하네요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/좋아, 그럼... 신고했을 때의 상황에 대해서 설명해줄 수 있어?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/네, 저는 방에 나와서 복도를 지나가고 있었어요. 옥상 정원에 가서 밤하늘을 보면서 산책하려고 했거든요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Exc/밤 산책은 좋아하세요? 낮에는 해가 너무 뜨거워서 나가기 싫지만, 밤에는 시원하고 조용해서 전 정말 좋아해요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Exc/맞아, 나도 산책 엄청 좋아해서 매일 최소 3번씩 조수한테 산책 가자고...", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Exc/...아! 또 엉뚱한 이야기를 할 뻔했다. 조수가 돌아오기 전에 얼른 끝내지 않으면 혼날 수도 있다고!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Exc/자, 잡담 금지! 지금부터는 진짜 조사에 집중! 난 조수한테 미움 받기 싫단 말이야.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Smile/하하, 주인에게 이쁨 받고 싶어하는 강아지 같아서 귀엽네요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/아무튼 저는 복도를 걷고 있었는데, 딱 돌아가신 분의 방을 지나가는 순간에 비명 소리를 듣고 깜짝 놀랐어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/마치 공포 영화에나 나올 법한 비명이랄까요? 그래서 처음에는 영화 소리인 줄 알았죠.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/하지만 비명 이후 확 조용해지는게 이상해서, 만일이라는 것도 있으니까 신고하자고 생각했어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/흐음... 그래서 곧바로 프런트에 신고를 했고, 직후에 밀령사에서 시체를 발견...", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/만약 진짜 영화 보는 중이었으면 민폐였겠지만, 진짜 사람이 죽은 일이어서 다행이네!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/...탐정님은 표현이 특이하시네요. 마치 사람이 죽은 일이 생겨서 좋다는 듯이요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/아, 아니야! 그런 의미는 절대로 아니었어!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Exc/그래요~? 조금 더 솔직해지셔도 좋을 텐데 말이죠.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/어쨌든! 다시 한번 정리해보자.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/너는 옥상 산책을 위해서 방에서 나왔고, 복도를 걷는데... 비명 소리를 들었다.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/놀란 너는 복도에 있는 인터폰을 통해서 곧바로 프런트에 신고. 맞지?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/맞아요~ 역시 탐정님은 정리도 잘 하시네요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/흥~ 그야 탐정이니까 당연하지.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/(신고 당시의 상황은 언제나 중요한 단서였으니까 기억해두자!)", "Latte_Left+Marcel_Right", "라떼",
              () => { Show_Clue(9); items[9].SetActive(true); submititems[9].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText)); })); // <단서 : "비명 소리" 획득>));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal//wait:2/혹시 신고한 다음에는 특별한 일은 없었어? 이상한 사람을 봤다거나~ 수상한 소리를 들었다거나~.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/사소한 거라도 좋으니까 전부 말해봐!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/사소한 거라... 아! 그러고 보니 신기한 경험을 했었어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/인터폰으로 프런트에 신고를 하던 중에 갑자기 전화가 뚝 끊어졌어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/신호가 끊긴 건가? 생각하던 그 순간... 갑자기 눈앞에 사람들이 확 튀어나왔어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/아~ 그거 못생긴 탈을 쓴 녀석들이지? 그놈들은 항상 신출귀몰하거든.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/마물 사건이라면 언제든 나타나. 신호가 끊긴 건, 결계로 우리들이랑 바깥을 끊어놓은 거지.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Exc/아하~ 그런 것도 가능한가요? 역시 신기하네요~.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/그럼 저 눈알 가면을 만나고는 바로 여기로 온 거야?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/으음~ 바로는 아니에요. 몇몇 분이 시체를 보러 가신 사이에 복도에서 몇 마디 물어보더라고요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Marcel_Right+Normal//sound:ding/(질문? 딱히 밀령사에서 용의자들에게 뭘 들었다는 말은 없었는데...)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/(중요하지 않은 내용일 수도 있지만... 그래도 확인 해 보자.)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal/그거 나한테도 말해줘! 뭐 물어봤는데?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/어라? 이미 그쪽 관계자 분에게 다 말했는데... 못 들으셨나요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/응...? 그런 얘기는 못 들었는걸?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Exc/그럼, 별로 중요한 얘기가 아니었다는 뜻이겠네요. 저도 이 사건과는 관련 없는 이야기였다고 생각하니까요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Exc/아니! 그럴 수는 없어! 나는 탐정이니까 뭐든지 샅샅이 조사를 해야 해!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/그래야지 조수가 기뻐하거든.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/......흐음. 그래요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/좋아요! 대신 조건이 있어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/조, 조건? 뭔데...?", "Latte_Left+Marcel_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Normal/이 사건이 끝나면 친구가 되고 싶어요. 저도 조수로 삼아주실래요? ", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/뭐어...?! 안돼! 내 조수가 될 수 있는 사람은 조수 뿐이란 말이야!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Exc/하하, 아쉽네요. 그럼 그냥 알려드릴 테니까, 제 부탁 진지하게 생각 해주세요. 근데... 진짜 별 내용은 없으니까, 큰 기대는 마시고요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/그러니까...... 제가 방에서 나오고, 신고하기 전까지 마주친 사람은 없었는지 물어봤었어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/그래서 없었다고 말했죠. 제가 엘리베이터를 타고 올라오는 동안 아무도 못 봤다고요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile//sound:ding/응...? 엘리베이터? 피해자랑 같은 층이 아니었던 거야?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/아~ 모르셨군요? 저는 그 아래층 방에서 지냈어요. 옥상 정원에 가려면 마지막 층에 있는 전용 계단으로 올라가야 해서, 거기 내린 거고요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/흐음... 어쨌든 올라오는 동안 아무도 못 봤단 거지?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/맞아요~ 근데 이 호텔 엘리베이터는 2개니까 다른 쪽에서 탔다면 의미 없겠지만요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Exc/(정말로 사건에 도움 되지 않는 내용이었던 걸까? 그래도 뭔가 찝찝한데...)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/(일단은 이 내용도 기억을 해두자.)", "Latte_Left+Marcel_Right", "라떼",
             () => { Show_Clue(10); items[10].SetActive(true); submititems[10].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText)); })); // <단서 : "신고자의 상황" 획득>));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal//wait:2/(이쪽한테 들을 수 있는 단서는 아마 이게 전부겠지?)", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/휴우... 협조해줘서 고마워! 일단 이야기는 여기까지야.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/하하, 저도 색다른 경험을 해서 좋았어요. 제 증언, 도움이 됐을까요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/응! 덕분에 꽤 유용한 정보들을 알 수 있었어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/후훗... 뭐랄까, 탐정님이라면 분명 이 사건의 진상을 밝혀내실 수 있을 거 같아요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Marcel_Right+Normal/...잉? 갑자기? /emote:Latte_Left+FSmile/일단 날 믿어줘서 고마워~. 나는 반드시 진범을 잡아야 하거든.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/나는... 조수가 자랑스러워하는 탐정이야 하니까.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/정말 멋지네요~. 기대하고 있을게요, 탐정님.", "Latte_Left+Marcel_Right", "마르셀", ()
            =>
        {
            RineInquiry();

        }));

        DialogManager.Show(dialogTexts);
    }

    private void RineInquiry()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(자... 마지막은 피해자의 방에 들어간 기록이 있는 \"리네\"...)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSurp/(어라? 방금 전까진 여기 근처에 있었던 것 같은데… 어디 갔지?)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal/저기, 웃상! 혹시 그 여자 의사 못 봤어?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/웃상...? 아~ 저 말인가요? 리네 씨라면 저쪽 구석에 있을 거에요.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal/좀 힘들어서 잠깐 혼자 있고 싶다고 하더라고요.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Marcel_Right+Normal/엥? 아직 시작도 하지 않았는데?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Marcel_Right+Smile//sound:ding/하하, 아마 마스크를 못 써서 그런 걸지도요? 리네 씨가 그게 없으면 사람 대하기 힘들다고 하셨어요.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/마스크는 왜? 감기라도 걸린 거야?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/글쎄요? 이유는 아직 못 들었는데, 그냥 본인에게 직접 물어보면 되지 않을까요?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(음… 사건이랑은 상관없을지도. 그래도 신경 쓰이는데… 한번 물어볼까?)", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/저기~? 의사 선생님? 그렇게 구석에 웅크리고 있으면 친구 없어 보여.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/…아! 죄, 죄송합니다… 그, 그냥 지금은... 혼자 있고 싶어서요...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Surp/그래도 탐정인 나한테는 얘기해줘야지? 안 그러면 정말~ 정말 수상해 보일걸?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/저, 저는... 그... 그러니까...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Emb/맞다! 그러고 보니 너는 마스크가 없어서 지금 힘들다고 들었는데, 정말이야? 왜 힘든 거야??", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Surp/그, 그걸 어떻게...... 일단은... 맞아요. 제가 다, 다른 사람들 앞에서 맨 얼굴을 보이는건... 불편해서...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Surp/왜? 얼굴 꽤 예쁘게 생겼는데! 이런 얼굴 가리면 아깝지 않아?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Ignore/...아, 아니에요! 저는 평범하게 생겼고... /sound:ding/평소에도 마스크를 쓰고 일해요. 다른 사람한테 얼굴 보여주는 건 부끄러워서...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Ignore/뭐?! 평상시에도 계속 마스크를 쓰는 거야? 겨우 낯을 가린다는 이유로...?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/...너 친구 없지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/친구... 있어요...! 진짜 있어요!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Surp/오오~ 진짜? 몇 명?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Emb/오, 온라인에서 만난 사람인데... 그 사람도 치, 친구...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/그래서 몇 명?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Ignore/...1명이요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/(마르셀 말이 맞았네. 얼굴 보여주기 부끄럽다고 항상 마스크를 쓴다니... 얼마나 부끄럼이 많은 거야?)", "Latte_Left+Rine_Right", "라떼",
                  () => { Show_Clue(12); items[12].SetActive(true); submititems[12].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText)); })); // <단서 : "리네의 마스크" 획득>

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore//wait:2/아무튼 사건 이야기로 넘어가 볼까? 사건 당시에 뭐하고 있었어?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/실은... 전 에이든 씨의 방에 갔었어요... 의무실에서 근무하고 있었는데, 긴급 호출이 울려서요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Normal/긴급 호출? 뭐 아프면 띵동! 하고 누르는 그런 벨이야?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Ignore/비슷... 해요. 물론 거의 울리지는 않고... 가끔 있는 일도 대부분... 장난이지만...", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Emb/그래도 만약이라는게 있잖아요...? 호, 혹시라도 진짜 다친 사람이 있으면... 구해줘야 하니까.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그래?? 그럼 방에 도착했을 때에 피해자는 어땠어? 여자는 없었어?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp/여, 여자요? 솔직히...... 잘 모르겠어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그게... 시신을 보고 놀라서, 다른 사람이 있는지 확인은 못 했어요......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/그, 그래도 어디에 숨었던게 아닌 이상... 봤을 거라고 생각해요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/(여자는 못 봤다라... 그럼 루시라는 사람이 있었다는 건 역시 티모테의 착각인가?)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/그러면... 시체를 발견하고서는? 뭘 했어?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/...저는 너무 놀랐어요. 의사지만 시체는 처음 보기도 했고, 사... 사람이 그렇게 변하는 건... 너무 이상하잖아요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/그 자리에서 아무것도 못 하고... 완전히 얼어버렸어요. 차마 신고할 생각조차 못 했어요......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp//sound:ding/(음... 의사가 겨우 시체로 그렇게 놀라도 되는 건가? 그리고 아무것도 못 했다기에는, 분명 리네가 움직인 증거가 있어.)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                stateIcon.sprite = stateChangeIcons[3]; // 추리 아이콘 변경
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "Rine"; //리네 초기화
                SubmitPanel.SetActive(true);

            }));




        DialogManager.Show(dialogTexts);
    }

    private void First_WordQuizComplete()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Smile/늦어서 죄송합니다. 그래도 표정을 보니, 심문은 순조로웠나 보군요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/응! 다들 얘기를 잘 해줘서 엄청 편했어! 그보다 내가 부탁한 일은 어땠어?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/네, 확인해달라고 하셨던 의무실... 탐정님의 예상대로 수상한 것들이 있었어요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Think/호텔 의무실인데도, 혈액팩이 잔뜩 있었어요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/혈액팩...? 그게 뭐가 이상한데? 병원 같은 곳에는 원래 있지 않아?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Normal/뭐... 일반적인 의무실에는 혈액팩을 준비하는 경우는 거의 없어요. 물론 만약이라는 경우도 있긴 한데...", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think//sound:ding/심지어 이상할 정도로 양이 많았습니다. 열린 창문 쪽 바닥에 빈 팩이 있었고, 심지어 시약 보관용 냉장고에도 잔뜩 쌓여있었죠.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Think/(그 정도로 많았다고? 확실히 내가 들어도 평범하지는 않아 보이네.)", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/(의료용이 아니라 마치 다른 용도가 있는 느낌이야.)", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/그 혈액팩에 이상한 점은 없었어? /emote:Latte_Left+FExc/혹시 색이 다르다거나? 막! 움직인다거나?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Ridic/그건 더 이상 피가 아니잖아요. 그거 말고는 딱히 이상한 건 없었습니다.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Think/굳이 말하자면, AB형 혈액팩만 없었어요. 우연히 혈액팩 주문 기록을 봤는데, AB형만 쏙 빼고 받았더라고요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Think/(특정 혈액형만 피한다라... 이것도 중요한 정보 같아, 기억해 두자.)", "Latte_Left+Siwoo_Right", "라떼",
            () =>
            {
                // 단서 혈액팩 획득 인덱스 13
                Show_Clue(13);
                items[13].SetActive(true);
                submititems[13].SetActive(true);
                StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));

            }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Think//wait:2/그나저나 왜 이렇게 늦은 거야~! 조수가 늦는 동안, 난 이미 모두의 이야기를 들었다고!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Annoy/그건 어쩔 수 없었어요. 저도 바로 오려고 했는데......", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Ridic/변명은 듣지 않겠다! 오늘 밤 산책형이다, 조수!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Ridic/하아... 그건 어차피 매일 하는 거 아닙니까.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Ridic/아... 하긴 그렇네. 그래서 늦은 이유가 뭐야?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Normal//sound:ding/엘리베이터가 고장이 났더라고요. 그래서 하나만 운행 중이었는데, 하필 그걸 놓쳤습니다.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Normal/얘기를 들어보니까, 4시간 전부터 사용 불가 상태였더군요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/4시간...? 그럼 사건 발생하기 전부터 엘리베이터를 못 썼다는 거야??", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Think/그렇죠, 피해자의 시신이 발견되었던 거는 대략 1시간 전이니까요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/그치만 우리가 아까 방 조사하러 올라갔을 때는 딱히 점검 중이라는 표시를 못 봤는데...?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Normal/애초에 버튼 자체가 작동하지 않도록 막았더군요. 벽에 있는 종이들 중에서 공지도 적혀 있었지만, 그냥 지나쳤다면 충분히 모를 법했겠어요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Normal/(그래서 몰랐던 건가? 근데 엘리베이터가 고장나서 하나만 운행 가능했다면... 역시 그 녀석이 수상해.)", "Latte_Left+Siwoo_Right", "라떼",
            () =>
            {
                // 단서 고장 난 엘리베이터 획득 인덱스 14
                Show_Clue(14);
                items[14].SetActive(true);
                submititems[14].SetActive(true);
                StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));

            }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal//wait:2/조수. 이제 상황을 어느 정도 알 거 같아. 저들 중에서 \"/color:red/인간인 척 연기하는 마물/color:white/\"이 누구인지.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/...!! 그 말씀은 범인을 알아내셨다는 건가요?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Surp/전에도 말했지만, 범\"인\"은 아니지~. 그리고 진범인지는 얘기를 나눠보면 알겠지.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/그전에... 상황을 정리해 보자.", "Latte_Center", "라떼", () =>
        {
            // 문장 완성 시작 전 bgm 변경
            SoundManager.instance.BGMChgnaer("DallTube");
        }));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink/(생각하자, 라떼. 저들 중에서 가짜를 찾으려면 먼저 \"/color:red/거짓말/color:white/\"을 찾아야해.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink/(거기서부터 시작하는 거야. 그러면 지금까지 들었던 증언을 떠올려 보자.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(먼저... 용의자들은 밀령사가 시체를 발견했을 당시에 어디에 있었다고 했지?)", "Latte_Center", "라떼",
            () =>
            {
                PrinterOnOff(false); // 프린터 꺼주기
                A_WordQuizPop();
                // 문장 완성 a 시작
            }));





        DialogManager.Show(dialogTexts);
    }

    private void RineMultipleDeducation()
    {
        SoundManager.instance.BGMChgnaer("Umwelt");
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/조수, 지금부터 추리 시간이야. 가면 쓴 마물을 벗겨버리자!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Smile/의심이 가는 용의자가 있는 건가요? 그 사람은 누구죠?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Smile/찐따 의...... 아, 아니지. 리네야. 명백하게 뭔가를 숨기고 있어. 이제 진짜 모습을 밝힐 차례야!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/리네 씨 말씀이시죠? 네, 바로 데려오겠습니다.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/응! 부탁할게~.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Normal/자, 조수가 의사를 데리러 간 사이에...... /emote:Latte_Left+FAnnoy/거기 탈쟁이!", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/뭐냐, 애완견.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/지금부터 '그거' 할 거야. 슬슬 준비해.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/......알겠다. 이번엔 누구를 상대로 할 셈이지? 전에 경고했을 거다. 엉뚱한 자를 잡으면 이번엔 너도 징계야.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Observer_Right+Normal/아, 알겠다고! 나는 우리 조수가 자랑스러워하는 탐정이야. 진실을 꼭 밝히겠어.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSmile//emote:Latte_Center+FAnnoy//emote:Observer_Right+Normal/탐정님, 여기 리네 씨를 모셔 왔습니다.", "Siwoo_Left+Latte_Center+Observer_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSmile//emote:Latte_Center+Exc//emote:Observer_Right+Normal/고마워 조수~. 역시 조수 밖에 없단 말이지! 야, 탈쟁이 빨리 시작해.", "Siwoo_Left+Latte_Center+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSmile//emote:Latte_Center+Exc//emote:Observer_Right+Normal/하아....... 그래, /color:red/눈/color:white/을 준비하겠다......", "Siwoo_Left+Latte_Center+Observer_Right", "밀령사 눈",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 화면 변경
                SoundManager.instance.BGMMuteOnOff(true); // 사운드 뮤트

            }));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//sound:magic/(좋아, 밀령사 눈알이 또 알 수 없는 주문을 중얼거리기 시작했어.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("(슬슬 눈을 감고... 다시 눈을 뜨면......)", "", "라떼",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[4]; // 눈알 화면 변경
                SoundManager.instance.BGMChgnaer("SchoolGhost"); // 스쿨 고스트 bgm 변경
                SoundManager.instance.BGMMuteOnOff(false);

            }));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+Emb/뭐, 뭐죠...?! 갑자기 주변이......", "Rine_Center", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/걱정 마~ 그냥 잠깐 우리 둘이 조용히 얘기 나눌 수 있는 공간을 만든 거야.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Emb/탈쟁이들이 쓰는 그 '결계'를 응용하면 이런 것도 가능하더라고. 신기하지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Ignore/이, 이게 신기하다고요...? 숨이 답답하고, 어지러운데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Ignore/맞아, 이 공간은 그 눈쟁이들이 특히 힘을 쓴 결계거든. 이곳에서는 그 어떤 '/color:red/거짓/color:white/'도 유지하기 힘들어져.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Surp/거, 거짓이요...? 그, 그게 무슨......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/그리고... 여기는 조수가 날 볼 일이 전혀 없는 장소. 나도 이번에는 진지하게 갈 거야.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/이제부터 진실을 밝혀내는 건 내 몫. 위태로운 거짓 가면을 벗겨내는 게, 탐정의 일이니까.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/시작하자-- 의사 선생님.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/제, 제가 거짓말을 하고 있다는 건가요...? 아, 아니에요! 정말이에요, 탐정님이 오해하신 거예요...!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/도대체... 어느 부분에서 절 의심하시는 거죠?! 전 진짜 아무 짓도 하지 않았는데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/너는 피해자의 방에 들어갔다가 티모테가 쓰러진 모습을 보고 패닉이 왔다고 했어. 그리고 그 상태로 방에서 나와, 의무실로 복귀한 게 맞지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Normal/맞아요... 처음부터 그렇게 말했잖아요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/하지만 그랬으면 너의 모습이 /color:red/여기에 없는 게/color:white/ 이상해!", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 리네 다중 추리 파트 연결
                stateIcon.sprite = stateChangeIcons[3]; // 추리 아이콘 변경
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "MultipleRineCCTV"; //리네 씨씨티비 초기화
                SubmitPanel.SetActive(true);

            }));

        DialogManager.Show(dialogTexts);


    }


    private void PrinterOnOff(bool onoff)
    {
        DialogPanel.SetActive(onoff);
        FinderObjectPanel.SetActive(!onoff);
    }

    private void SwitchResearchBackground()
    {
        Background[0].SetActive(false);
        Background[1].SetActive(true);
    }

    private void StartFinderSystem()
    {
        stateIcon.sprite = stateChangeIcons[1]; // 수색 아이콘 변경
        FinderPanel.SetActive(true);
        FinderMousePointerChange(this, EventArgs.Empty);
    }

    private void EndFinderSytem() // 파인드 끝나고 심문 호출
    {
        stateIcon.sprite = stateChangeIcons[0]; // 수색 아이콘 스토리로 변경
        FinderPanel.SetActive(false);
        BasicMousePointer(this, EventArgs.Empty);

    }

    private void Show_Clue(int index)
    {
        cluePanel[index].SetActive(true);

    }


    #region FinderFunc

    public void Finder_Window()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        Debug.Log(itemsCount);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//sound:ding/여기 창문도 원래 이렇게 열려 있었어?", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/밀령사에서 시체 발견 당시의 모습을 그대로 보존했다고 했으니... 적어도 그 이후에 열리지는 않았을 거예요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/그럼 범인이 여기로 탈출했을 수도 있겠네!\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Think/이 정도 높이에서요? 아무리 그래도 뛰어내리기에는 너무 높은 곳인데요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Think/조수! 잊었어? 이 사건은 마물 사건이라고! 조금 더 넓게 생각을 해야지.\r\n", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/마물 중에는 날 수 있는 녀석들도 있고, 이 정도 높이에서 떨어져도 죽지 않는 녀석들도 있어.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/하긴 그렇네요. 마물은 종류가 다양하다고 들었으니, 많은 가능성을 고려해야 겠네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/맞아! 심지어 우리는 단순히 범인만 잡는 게 아니라, 정체도 맞춰야 하잖아?\r\n 마물 중에는 외모, 나이, 심지어 성별도 바꾸는 놈들이 있어.", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(그러니까 이런 사소한 정보들 중에도 중요한 단서가 있을지도 모르니까, 탐정으로서 전부 기억해야 해)", "Latte_Left", "라떼",
            () =>
             {
                 Show_Clue(2);
                 items[2].SetActive(true);
                 submititems[2].SetActive(true);
                 StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));

                 if (itemsCount == 3)
                 {
                     BasicEndFinder();
                 }
                 else
                 {
                     PrinterOnOff(false);
                     FinderMousePointerChange(this, EventArgs.Empty);
                 }

             }
        ));




        DialogManager.Show(dialogTexts);


    }

    public void Finder_Corpse()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        Debug.Log(itemsCount);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/ emote:Latte_Left + FNormal//sound:ding/흠흠... 역시 미라 상태야. 완전히 쫙 빨려서 죽은 느낌.", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/대체 어떤 식으로 죽였길래 시체가 이런 모양인 거죠?\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Surp/글쎄? 시신을 조사해보면 뭔가 흔적이 있지 않을까?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSmile//emote:Siwoo_Right+Surp/그럼~ 어디어디... 뭔가 없을까나~? 이런 시체는 실제로는 처음 봐서 조금 두근두근 한데.\r\n", "Latte_Center + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSmile//emote:Siwoo_Right+Annoy/으윽... 저번 사건에서도 느꼈지만, 탐정님은 시체에 거부감이 없으시네요.\r\n", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Annoy/응? 그냥 죽은 인간의 몸이잖아? 그렇게 놀랄 일도 아닌데 뭐~.\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSurp//emote:Siwoo_Right+Surp/...어라? 여기 목 쪽에 상처가 있네?\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSurp//emote:Siwoo_Right+Surp/상처요?\r\n", "Latte_Center + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/응! 목 쪽을 자세히 보니까 뭔가 날카로운 것에 찔린 듯한 작은 구멍이 2개 있어.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/...기분 탓인지 모르겠는데 상처 모양이 마치 짐승한테 물린 상처 같네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink/(조수 말대로 범인한테 물린 상처인 걸까? 그럼 범인은 입으로 물어 죽이는 마물?)\r\n", "Latte_Left", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(어쨌든 이것도 기억해두자.)\r\n /click/\r\n", "Latte_Left", "라떼",
            () =>
            {
                Show_Clue(3);
                items[3].SetActive(true);
                submititems[3].SetActive(true);
                StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));
                if (itemsCount == 3)
                {
                    BasicEndFinder();
                }
                else
                {
                    PrinterOnOff(false);
                    FinderMousePointerChange(this, EventArgs.Empty);
                }
            }
        ));


        DialogManager.Show(dialogTexts);

    }

    public void Finder_Phone()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        Debug.Log(itemsCount);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal//sound:ding/응? 여기에 휴대폰이 떨어져 있네?", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Normal/정말이네요. 피해자의 휴대폰인 걸까요?\r\n", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Normal/어디 보자... /emote:Latte_Center+FSurp/오! 잠금도 걸려있지 않네! 안에 한번 확인해 볼까~.\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FAnnoy//emote:Siwoo_Right+Normal/...으엑, 이 휴대폰 주인은 어떤 생활을 하고 다니는 거야?!\r\n", "Latte_Center + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+FAnnoy//emote:Siwoo_Right+Think/흐음... 갤러리에 있는 사진들을 보니 피해자인 에이든의 휴대폰이 맞는 거 같네요.\r\n", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FAnnoy//emote:Siwoo_Right+Think/사건 당시의 사진이라도 있나 보려고 했는데, 사진의 절반이 자기 셀카고 나머지 절반은 여자잖아?\r\n", "Latte_Center + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal//emote:Siwoo_Right+Normal/심지어 최근 어플 사용 기록도 데이트 어플... 아무래도 피해자는 문란한 생활을 했나 보네요.\r\n", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/조수! 너에게 중요한 임무를 맡겨줄게. 이 휴대폰은 너가 조사하도록!\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Ridic/하아... 알겠습니다. 탐정을 보조하는 게 조수가 할 일이니까요.\r\n", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/...더 조사해 봤는데, 최근에 용의자인 티모테와 23시 02분에 통화한 기록이 있네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Normal/23시 02분? 그때쯤이면...\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Normal/네, 리네가 피해자의 방에 들어오기 전이네요.\r\n", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal/(적어도 23시 02분까지는 살아있었다는 뜻인데... 밀령사가 시체를 확보한 시간이 15분.)\r\n", "Latte_Left", "라떼"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink/(그러면 피해자의 사망 추정 시각은 그사이라는 거잖아. 혹시 티모테라는 자가 뭔가 알려나?)\r\n", "Latte_Left", "라떼",
            () =>
            {
                Show_Clue(4);
                items[4].SetActive(true);
                submititems[4].SetActive(true);
                StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));
                if (itemsCount == 3)
                {
                    BasicEndFinder();
                }
                else
                {
                    PrinterOnOff(false);
                    FinderMousePointerChange(this, EventArgs.Empty);
                }
            }
        ));


        DialogManager.Show(dialogTexts);

    }


    public void DeepFinder_Reverb()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal//sound:sniff/(킁킁... 뭔가 바닥에서 진하고 이상한 냄새가 나고 있어.)", "Latte_Center", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/(그런데 또 냄새는 별로 나지 않아서 이상해. 냄새가 보이지 않았다면 애초에 있는지도 몰랐겠어.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal/(도대체 이거 정체는 뭐지... 생긴 것만 보면 무슨 구멍 같은데.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/(현실에는 없는 구멍. 이 정도 크기면 사람 하나는 지나갈 수 있겠는걸?)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/(여기를 통해서 뭔가가 이동했다는 뜻일까? 아니면 뭔가를 여기에 빠뜨린 걸까?)", "Latte_Center", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal/(어느 쪽이든 평범한 인간이 낼 수 있는 흔적은 아니야.)", "Latte_Center", "라떼",
            () =>
            {
                Show_Clue(5);
                items[5].SetActive(true);
                submititems[5].SetActive(true);
                PrinterOnOff(false);
                FinderMousePointerChange(this, EventArgs.Empty);
                Debug.Log(itemsCount);
                if (itemsCount <= 0)
                { EndFinder(); }
                else
                {
                    PrinterOnOff(false);
                    FinderMousePointerChange(this, EventArgs.Empty);
                };
            }
        ));


        DialogManager.Show(dialogTexts);
    }

    public void DeepFinder_Bloody()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Surp//sound:sniff/(이건... 꽤 진하게 남아 있네. 덕분에 형태가 명확하게 잘 보여.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Surp/날개 같은 게 보이는데 새인가?\r\n", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("적어도 창문을 통해서 방에 있던 뭔가가 밖으로 나간 거는 확실해졌고...\r\n", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/그보다 이 냄새는... 피 냄새 아니야? 왜 창문에서 피 냄새가 나는 거지?\r\n", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/(너무 수상해. 이 정도로 진한 피 냄새면 단순히 인간이 다쳐서 흘린 피 냄새 수준이 아니야.)\r\n", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("(그냥 핏덩어리 그 자체의 냄새.)\r\n", "Latte_Center", "라떼"));

        dialogTexts.Add(new DialogData("(아무래도 엄청 중요한 단서인 거 같아.)\r\n", "Latte_Center", "라떼",
            () =>
            {
                Show_Clue(6);
                items[6].SetActive(true);
                submititems[6].SetActive(true);
                PrinterOnOff(false);
                FinderMousePointerChange(this, EventArgs.Empty);
                Debug.Log(itemsCount);
                if (itemsCount <= 0)
                { EndFinder(); }
                else
                {
                    PrinterOnOff(false);
                    FinderMousePointerChange(this, EventArgs.Empty);
                };
            }
        ));


        DialogManager.Show(dialogTexts);
    }

    public void DeepFinder_AnomalySmell()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        itemsCount -= 1;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal//sound:sniff/(아까부터 신경 쓰였는데, 이 방 전체에 희미하게 향이 나고 있어.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("(무취에 가깝기는 하지만, 어딘가 불쾌한 느낌이야.)\r\n", "Latte_Center", "라떼"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/조수! 혹시 이 방에서 무슨 냄새 나지 않아?\r\n", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/네? 글쎄요, 전 아무 냄새도...\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/...아니다, 자세히 맡아보니 살짝 달달한 냄새가 나네요. 향수 느낌의 냄새.\r\n", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Surp/달달한 냄새? 향수?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/조수? 무슨 말을 하는 거야? 여기에 그런 냄새는 없어.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/냄새만큼은 내가 확실하게 말할 수 있어! 내 코는 최고의 탐정 코니까!", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/그러니까 그건 조수의 착각인 거야 착각~. 조수가 웬일로 실수도 하고, 귀엽네~.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/......\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/...조수? 왜 그래?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Siwoo_Right+Normal/......\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FExc//emote:Siwoo_Right+Normal/조오오오수우우우!!! 정신 차려!!!\r\n", "Latte_Center + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FExc//emote:Siwoo_Right+Surp/...네, 네?! 아, 죄송합니다. 제가 잠깐 집중을 못 했네요.", "Latte_Center + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/무슨 일 있어? 평소 같지 않았는데 어디 아픈 거는 아니지?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/그... 죄송합니다. 이 냄새를 맡으니까 정신이 멍해져서요. 마치 잠이 오듯이......", "Latte_Left + Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/(냄새를 맡았더니 조수가 이상해졌어. 평범한 향이 아니라는 건가.)\r\n", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Siwoo_Right+Think/(그런데 왜 나랑 다르게 느끼는 거지? 뭔가 조건이 있는 걸까? 나랑 조수의 차이라면...)", "Latte_Left + Siwoo_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/(조금 더 진했다면 명확하게 형태도 보였을 텐데. 하필 창문이 열려있어서 환기로 흔적이 사라진 거 같아. 설마... 의도인가?)", "Latte_Left + Siwoo_Right", "라떼",
            () =>
            {
                Show_Clue(7);
                items[7].SetActive(true);
                submititems[7].SetActive(true);
                PrinterOnOff(false);
                Debug.Log(itemsCount);
                FinderMousePointerChange(this, EventArgs.Empty);
                if (itemsCount <= 0)
                { EndFinder(); }
                else
                {
                    PrinterOnOff(false);
                    FinderMousePointerChange(this, EventArgs.Empty);
                };
            }
        ));


        DialogManager.Show(dialogTexts);
    }

    #endregion


    #region FinderExit

    // (일반 수사 파트 종료)

    public void BasicEndFinder()
    {
        BasicMousePointer(this, EventArgs.Empty);
        PrinterOnOff(true);
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/wait:2//emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/일단은 이 정도인가. 눈으로 봤을 때에 신경 쓰이는 단서들은 이게 끝이야!\r\n", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/벌써 끝이신가요? 생각보다 금방 끝나셨네요.\r\n", "Latte_Left+Siwoo_Right", "시우"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/후훗, 조수. 무슨 소리야? \"/color:red/눈/color:white/\"으로 하는 조사가 끝이라고.\r\n", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Surp/...아, 그거를 하실 차례군요. 뭐라고 하셨더라... 냄새를 /color:red/본다/color:white/고 하셨던가요?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Surp/걱정 마~. 우리 조수는 내 \"/color:red/코/color:white/\"만 믿고 있으라고~.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal/(현장에 남아 있는 단서는 눈으로 보이는 물체들만이 아니야.)\r\n", "Latte_Center+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Exc/(지금부터는 보이지 않는 단서. 나만이 감지할 수 있는 \"/color:red/냄새/color:white/\"들을 찾아내자!)\r\n", "Latte_Center+Siwoo_Right", "라떼",
            () =>
            {
                FinderMousePointerChange(this, EventArgs.Empty);
                deepfinderIcon.SetActive(true);
                PrinterOnOff(false);
            }));

        DialogManager.Show(dialogTexts);

    }

    // (냄새 수사 파트 종료)

    public void EndFinder()
    {

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/wait:2//emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal//sound:ding/이 정도면 충분해! 계속 킁킁거렸더니 코에 쥐가 날 거 같아.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/수고하셨습니다. 이제 바로 용의자들이 모여있다는 곳으로 갈 건가요? 밀령사에서 객실 수사가 곧바로 오라고 했으니까.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Smile/가야지~. /emote:Latte_Left+FNormal/근데 그 전에!", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Smile/조수, 내 부탁 하나만 들어줘. 의무실에 가서 뭔가 수상한 게 있는지 확인해 줘.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/의무실이요? 왜 탐정님이 직접 가시지 않고요?\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Surp/아까 그 눈알 가면이 모든 곳에 결계가 쳐져 있지는 않았다고 했잖아. 괜히 결계가 없는 곳에 돌아다니다가 인간을 마주치면 어쩌려고?\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/그리고 그 녀석들은 인간한테는 협조적이니까, 아마 네가 부탁하면 의무실에 들어갈 수 있게 해줄 거야.\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Smile/...알겠습니다. 그럼, 제가 그곳을 조사하고 나중에 합류할게요.\r\n", "Latte_Left + Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/오케이~! 조수만 믿고 있을게! 자, 다녀와서 꼭 뭐라도 건져 와!\r\n", "Latte_Left + Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/...네, 책임지고 확인해 보겠습니다.\r\n", "Latte_Left + Siwoo_Right", "시우",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[2]; 
                FadeInOutPlay(this, EventArgs.Empty);
                StartCoroutine(InquiryStarter());
                DialogPanel.SetActive(false);
                SoundManager.instance.BGMMuteOnOff(true);

            }));

        DialogManager.Show(dialogTexts);
    }


    #endregion




    public IEnumerator TalkPrinterOnOFF()
    {
        DialogPanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        DialogPanel.SetActive(true);
        HotelHallWay();
    }

    public IEnumerator TalkPrinterOff(Image printer, Image name, TextMeshProUGUI nametext)
    {

        // 현재 컬러 가져오기
        Color c = printer.color;
        Color c2 = name.color;
        Color c3 = nametext.color;

        // 알파 0으로
        c.a = 0f;
        c2.a = 0f;
        c3.a = 0f;
        printer.color = c;
        name.color = c2;
        nametext.color = c3;

        var delay = 1.5f;
        // delay 시간만큼 대기
        yield return new WaitForSeconds(delay);

        // 알파 1로 되돌리기
        c.a = 1f;
        c2.a = 1f;
        c3.a = 1f;
        printer.color = c;
        name.color = c2;
        nametext.color = c3;



    }



    public IEnumerator InquiryStarter()
    {
        EndFinderSytem();
        Background[1].SetActive(false);
        Background[2].SetActive(false);
        Background[0].SetActive(true);
 
        yield return new WaitForSeconds(2f);
        DialogPanel.SetActive(true);
        HotelInquiry();
    }


    #region TimoteSubmit

    public void TimoteSumbit(object sender, EventArgs eventArgs)
    {
        SubmitEvents_Timote_Items04();
    }

    public void TimoteErrorSumbit(object sender, EventArgs eventArgs)
    {
        SubmitEvents_Timote_ErrorSumbit();
    }

    public void SubmitEvents_Timote_Items04()
    {
        PrinterOnOff(true);
        stateIcon.sprite = stateChangeIcons[2]; // 심문 아이콘 변경

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Normal//sound:ding/아, 맞다! 나 지금 생각났어!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Normal/이거면 네가 잃어버린 기억을 되찾아줄지도 몰라!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/아까 조사하면서 피해자의 휴대폰이 떨어져 있었는데... 거기에 너랑 통화하던 기록이 있더라고?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Emb/통화...? 내가 그 녀석한테?", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Emb/응! 기록상으로는 23시 02분에 피해자랑 너랑 통화를 했어. 길게 하지는 않았지만.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Emb/전화를... 내가...", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Emb/혹시 휴대폰을 잃어버렸다거나, 그런 적은 없었지?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/아니, 마지막으로 기억하는 순간에도 내가 가지고 있었고, 깨어났을 때도 그대로였어.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Emb/그나저나 내가 그 녀석한테 전화를 했다고...? 전혀 기억이 나질 않는......", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/아니... 잠시만... 맞아, 나는 분명 전화를 했었어. 왜 이걸 기억 못 한 거지?", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Think/오! 뭔가 떠올랐어?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Think/확신은 없는데... 흐릿하게 떠오르긴 해. 뭔가 내 기억이 맞는 지도 헷갈릴 정도로 흐릿하지만...", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Normal/내 짐을 가지러 가야 했어. 그놈이 꼴 보기 싫어서 밖에서 버티다가, 방에 있나 확인하려고 전화한 거야.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Timote_Right+Think/만약 안에 있으면 대충 내 짐만 밖에다가 빼놓으라고 시킬 거였어. 딱 얼굴 보지 않고 바로 떠나려고.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Think/음~ 그렇게까지 보기 싫었구나? 뭐, 이제는 보고 싶어도 영영 못 보겠지만.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Smile/뭐, 그건 그렇지. 아무튼 전화를 했고, 대화 내용까지는 기억 나지 않지만...", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Normal/그 녀석 혼자가 아니었어. /color:red/여자 목소리/color:white/가 들렸는데... 아마 만나기로 했다는 루시겠지.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal//sound:ding/(흐음... 여자 목소리? 그럼 그 시간에 루시라는 사람이랑 같이 있었다?)", "Latte_Left+Timote_Right", "라떼",
            () =>
            {
                // 단서 04 인덱스 갱신
                NoteItemsManager.Instance.itemsDescription[4] = "23시 02분부터 티모테와 전화하는데, 여자 목소리가 들렸다고 했어. 루시 같다고 했는데 과연 착각일까?";
            }));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(아까 머리가 멍하다고 표현해서 짐작했던 거였는데... 역시 기억이 지워진 게 아니었어.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(기억을 \"/color:red/가렸다/color:white/\"는 느낌에 더 가까워. 물론 이 남자가 거짓말을 한다는 가능성은 아직 남았지만.)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Normal/혹시 더 떠오른 기억은 없어? 예를 들어... 피해자의 방에 들어간 순간이라거나. CCTV에 들어가는 모습이 찍혔잖아.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Emb/아니, 그건 여전히 모르겠어. 방에 돌아간 기억은 없고, 그냥 정신 차리고 보니까 방 안에 있었어.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Think/아까 이상한 탈을 쓴 녀석이 방 안에 기절해 있던 나를 깨워줬었지. 그리고 눈을 뜨고 본 게 미라처럼 변한 녀석의 시체...", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/하, 사람이 그런 식으로도 죽는 구나 싶었지.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Annoy/그럼 정신을 차렸을 때 방에는 너랑 저쪽 못생긴 탈이랑, 그리고 또 누구 더 있었어?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal//sound:ding/아니, 나 말고는 아무도 없었어.", "Latte_Left+Timote_Right", "티모테"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/뭐?? 그게 정말이야? 정말로 없었어?", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Timote_Right+Normal/그래, 그것만큼은 확실해. 나랑 그 죽어버린 자식밖에 없었어. 그 놈들... 그 탈 쓴 놈들이 날 깨웠고.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(리네도, 통화로 들렸던 여자도 없었다고?)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(여자는 착각이었다고 쳐도... 리네가 없는 건 이상한데?)", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Timote_Right+Normal/(그럼... 리네는 그때 어디에 있던 거지?)", "Latte_Left+Timote_Right", "라떼",
             () =>
             {
                 //단서 사라진 리네 획득
                 Show_Clue(8);
                 items[8].SetActive(true);
                 submititems[8].SetActive(true);
                 StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));
             }));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Normal//wait:2/흐음~ 알겠어. 일단은 네 이야기는 여기서 끝!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Annoy/드디어 끝난 거냐? 하아... 아직도 머리가 멍해서 짜증 나 죽겠네.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/그렇게 귀찮아 하면서도 의외로 많이 알려주던데? 의외로 협조적이고!", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Emb/...그냥 확실히 끝내고 싶은 거야. 괜히 어설프게 말했다가는 계속 물어볼 수도 있으니까.", "Latte_Left+Timote_Right", "티모테"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Timote_Right+Emb/안심해~! 난 기억력이 좋은 편이라, 같은 질문은 또 하지 않을 거야.", "Latte_Left+Timote_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Timote_Right+Emb/(...아마도.)", "Latte_Left+Timote_Right", "라떼",
            () =>
            {
                //마르셀 심문 파트 시작
                stateIcon.sprite = stateChangeIcons[2]; // 심문 아이콘 변경
                MarcelInquiry();

            }));



        DialogManager.Show(dialogTexts);
    }


    public void SubmitEvents_Timote_ErrorSumbit()
    {
        PrinterOnOff(true);

        var dialogTexts = new List<DialogData>();
        stateIcon.sprite = stateChangeIcons[2]; //심문 아이콘 변경

        dialogTexts.Add(new DialogData("/emote:Latte_Left + FNormal//emote:Timote_Right+Annoy/...그게 뭔데? 지금 장난치냐?", "Latte_Left+Timote_Right", "티모테"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Timote_Right+Annoy/아, 아! 실수야 실수~. 강아지도 나무 위에서 떨어지는 법이잖아?", "Latte_Left+Timote_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Timote_Right+Annoy/(신중하게 다시 생각하자. 여기서 보여줄 단서는 뭐지?)", "Latte_Left+Timote_Right", "라떼",
            ()
            =>
            {
                stateIcon.sprite = stateChangeIcons[3]; //추리 아이콘 변경
                PrinterOnOff(false);
                SubmitPanel.SetActive(true);
            }));

        DialogManager.Show(dialogTexts);
    }


    #endregion

    #region RineSubmit

    public void RineSubmit(object sender, EventArgs eventArgs)
    {
        SubmitEvents_Rine_Items08();
    }

    public void RineErrorSumbit(object sender, EventArgs eventArgs)
    {
        SubmitEvents_Rine_ErrorSubmit();
    }

    public void SubmitEvents_Rine_Items08()
    {
        PrinterOnOff(true);
        stateIcon.sprite = stateChangeIcons[2]; // 심문 아이콘 변경

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/담배 안경이 깨어났을 때는 널 못 봤다고 했어. 혹시 중간에 나갔어?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/네...? 아, 호, 혹시 티모테 씨 말씀이신가요?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/못 들으셨나요? 그... 남성 분이 방에 들어오시고 저랑 마주쳤는데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Emb/아~ 이유는 모르겠는데, 기억이 애매하다고 하더라. 아니면 치매거나!", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Jinji/기억... /sound:ding/혹시 그 일이랑 관련... 있는 걸까요...?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Jinji/오?! 뭔가 아는 거야??", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Surp/아...! 그게 말이죠... 그 분이 방에 들어온 순간에... 이상한 냄새가 난다면서 기절하셨어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Emb/그, 그때 머리부터 넘어지셨는데... 뭔가 충격이 간게 아닐... 까요?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/(냄새...? 조수는 맡았다고 했지. 그런데 나는 전혀 못 맡았단 말이야? 내 코는 누구보다 정확한데!)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/혹시 너도 그 냄새 맡았어? 그... 달콤한 냄새?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/달... 콤하다고요? 아니요...? 아무 냄새도 못 맡았어요. 그래서 그 분이 하신 말씀을 잘... 몰랐어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/(흐음... 나도 모르고, 의사도 모르는데 조수랑 안경만 맡았지. 나랑 조수의 차이... 그리고 안경이랑 의사의 차이...)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore//sound:ding/(으음... 그나마 떠오르는 차이는 성별 정도인가? 아니면 의사랑 나랑 근본적으로 닮은 뭔가가 있거나.)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                //단서 이상한 향기 갱신 인덱스 7
                NoteItemsManager.Instance.itemsDescription[7] = "피해자 방의 향기는 나와 리네는 괜찮았는데, 조수랑 티모테는 졸리게 만들었어. 우리 차이는 성별 정도 아닌가?";

            }));



        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Ignore/...근데 안경이 다시 깨어났을 때도 너는 없다고 했는데? 결국 어디에 갔던 거야??", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Emb/사실...... 시체도 보고... 갑자기 눈 앞에서 사람도 쓰러져서... 그대로 패닉이 왔어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Emb/진짜 아무 생각도 들지 않아서... 얼른 의무실로 돌아갔어요. 이 상황이 너무 무섭고, 혹시 꿈인 게 아닐지 생각하고 있었더니, 그분들이 왔고요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그분들...? 아~ 이상한 탈 쓴 녀석들?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Normal/네, 네...! 그분들이요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/(그럼 요 의사는 의무실에 있었다는 거네. 정말이지, 겨우 시체로 얼마나 겁을 먹은 거야. 자세히 보면 재밌는데.)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 단서 리네의 이동 획득 인덱스 11
                Show_Clue(11); items[11].SetActive(true); submititems[11].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));

            }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Normal//wait:2/근데 아까는 말도 제대로 못 하더니, 이제 제법 잘 말하네?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Smile/가, 감사합니다. 뭐랄까... 탐정님은 편안한 느낌이 들어서요. 귀여운 강아지 같아서 일까요...?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Smile/그래?? 나 귀여워?? 헤헤, 조수한테도 그런 말 듣고 싶......", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Smile/아! 아니지, 그보다 더 물어볼게 있는데 너 혹시......", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Center+FNormal//emote:Rine_Right+Smile//sound:walking/탐정님, 돌아왔습니다.", "Siwoo_Left+Latte_Center+Rine_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Center+Exc//emote:Rine_Right+Normal/아! 조수!! 드디어 왔구나? 기다리고 있었어!", "Siwoo_Left+Latte_Center+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Center+FSmile//emote:Rine_Right+Normal/일단 우리 조수가 왔으니까, 나머지는 이따가 다시 물어볼게.", "Siwoo_Left+Latte_Center+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Center+FSmile//emote:Rine_Right+Emb/네...? 아, 알겠습니다.", "Siwoo_Left+Latte_Center+Rine_Right", "리네", () =>
        {
            // 심문 파트 종료
            // 문장완성 파트 이동 (좌측 파트 아이콘 스토리로 시작)
            stateIcon.sprite = stateChangeIcons[0]; // 스토리 아이콘 변경
            First_WordQuizComplete();

        }));


        DialogManager.Show(dialogTexts);
    }

    public void SubmitEvents_Rine_ErrorSubmit()
    {
        PrinterOnOff(true);

        var dialogTexts = new List<DialogData>();
        stateIcon.sprite = stateChangeIcons[2]; //심문 아이콘 변경

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/그, 그건... 무슨 뜻이죠?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Surp/오? 이건 아닌가? 아님 말고~ 미안! 다시 해도 괜찮지??", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Emb/네? 아... 네......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/(신중하게 다시 생각하자. 여기서 보여줄 단서는 뭐지?)", "Latte_Left+Rine_Right", "라떼",
             ()
            =>
             {
                 stateIcon.sprite = stateChangeIcons[3]; //추리 아이콘 변경
                 PrinterOnOff(false);
                 SubmitPanel.SetActive(true);
             }));

        DialogManager.Show(dialogTexts);


    }

    #endregion


    #region WordQuiz


    private void A_WordQuizPop()
    {
        WordQuizPanel.SetActive(true);
        wordQuizNum[0] = true;
    }

    private void B_WordQuizPop()
    {
        WordQuizPanel_02.SetActive(true);
        wordQuizNum[1] = true;

    }

    private void C_WordQuizPop()
    {
        WordQuizPanel_03.SetActive(true);
        wordQuizNum[2] = true;
    }


    public void F_WordQuizText(object sender, EventArgs eventArgs)
    {
        wordQuizNum[0] = false;
        DialogPanel.SetActive(true);

        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FExc//sound:ding/(맞아! 티모테는 시신이 있던 방에 같이 있었고, 마르셀은 복도에서 기다리고 있었어. 그리고 리네는 놀라서 의무실에 돌아갔다고 했지.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(하지만 3명 중에서 말이 되지 않는 증언을 한 자가 있어. 그게 누구지?)", "Latte_Center", "라떼",
            () =>
            {
                // 문장 완성 두 번째 파트 시작
                B_WordQuizPop();

            }));
        DialogManager.Show(dialogTexts);

    }

    public void S_WordQuizText(object sender, EventArgs eventArgs)
    {
        wordQuizNum[1] = false;
        DialogPanel.SetActive(true);

        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink//sound:ding/(그래... 리네가 이상해.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(피해자의 방에 들어갔던 리네가 의무실로 돌아갔다? 그건 다른 사람들의 증언과 비교하면 수상한 부분이 있어.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink/(그리고 리네의 수상한 부분은 바로......)", "Latte_Center", "라떼",
        () =>
            {
                // 문장 완성 세 번째 파트 시작
                C_WordQuizPop();

            }));

        DialogManager.Show(dialogTexts);
    }


    private void T_WordQuizText(object sender, EventArgs eventArgs)
    {
        wordQuizNum[2] = false;
        DialogPanel.SetActive(true);

        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink//sound:ding/(.......)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(리네가 의무실로 돌아간 방법. 그게 미스테리야.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink/(리네는 밀령사가 의무실에 있는 자신을 데려왔다고 했지. 아마 그건 거짓이 아닐 거야.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FThink/(즉, 그곳에 있었다는 \"/color:red/결과/color:white/\"는 진실이지만, 거기까지 가는 \"/color:red/과정/color:white/\"이 거짓말이야.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FNormal/(그리고 리네가 거기까지 간 방법이야말로, 그 여자의 가면을 벗길 열쇠야.)", "Latte_Center", "라떼",
        () =>
        {
            //리네 다중 추리 파트 이동
            RineMultipleDeducation();

        }));

        DialogManager.Show(dialogTexts);

    }

    private void Error_WordQuizText(object sender, EventArgs eventArgs)
    {
        DialogPanel.SetActive(true);

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("(아니, 이게 아니야.)", "", "라떼"));
        dialogTexts.Add(new DialogData("(집중하자, 라떼. 조수에게 칭찬 받으려면 제대로 해야 해.)", "", "라떼",
            () =>
        {
            if (wordQuizNum[0])
            {
                A_WordQuizPop();
            }
            else if (wordQuizNum[1])
            {
                B_WordQuizPop();
            }
            else if (wordQuizNum[2])
            {
                C_WordQuizPop();
            }
            else Debug.Log("단어 맞히기 패널 오류:" + string.Join(", ", wordQuizNum));


        }));

        DialogManager.Show(dialogTexts);

    }


    #endregion

    #region Rine, VampRine Reasoning

    public void RineCCTVSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Normal//sound:ding/CCTV 기록에서 너가 나가는 모습이 없었어. 마르셀이 신고한 이후 밀령사가 거의 즉시 시체를 확인하러 방으로 갔었지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Normal/그때 티모테가 깨어나면서 너는 '/color:red/못 봤다/color:white/'라고 했어. 이 부분은 너도 인정한 부분이잖아.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/그럼 넌 도대체 어떻게 방에서 나온 거야?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Jinji/CCTV요...? 그게 무슨 말씀이시죠? 그 복도엔 CCTV가 없을 텐데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Jinji/뭐? 하지만 저 탈쟁이들이 분명 CCTV로 감시하던 기록이라고 했는데?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Surp/아... 아!! 마, 맞아요. CCTV에요. 새, 생각해 보니까 복도에 CCTV가 있었네요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp/(뭐지...? 방금까지는 CCTV가 없다고 부정했으면서, 갑자기 인정하는 거지?)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Surp/(탈쟁이들 언급이 나오자마자 반응이 달라졌는데... 혹시 설마...??)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/(......내 생각이 맞다면, 이 부분은 나중에 따로 확인해 볼 필요가 있겠어.)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/그, 그런데... 혹시 그 영상은 끝까지 전부 나와 있었나요...?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Emb/응? 그건 아닌데? 앞뒤로 조금 잘려있었어.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Surp/그, 그러면 그 기록의 중간이 잘렸을 가능성도 있잖아요! 그것만으로 절 의심하시기는 이르지... 않을까요...?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/하지만 너는 티모테가 쓰러진 직후에 바로 방에서 나왔다고 했잖아?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/너가 티모테가 들어온 직후에, 방에서 나왔다면 /color:red/이상한 부분/color:white/이 하나 더 있어.", "Latte_Left+Rine_Right", "라떼",
             () =>
             {
                 // 리네 다중 추리 파트 연결
                 PrinterOnOff(false);
                 NoteItemsManager.Instance.submitTargetName = "MultipleRineReporter"; //신고자의 상황 초기화
                 SubmitPanel.SetActive(true);

             }));


        DialogManager.Show(dialogTexts);
    }


    public void RineReporterSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Ignore//sound:ding/웃상-- 그러니까 마르셀은 아래층에서 엘리베이터를 타고 올라가는 동안 아무도 못 봤다고 했어.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Ignore/의무실은 1층이니까 내려가야 하잖아? 그 과정에서 왜 서로를 못 봤을까?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그, 그건... 당연히 다른 에, 엘리베이터를 탔으니까요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/(동요하고 있어! 역시 의무실에 돌아간 /color:red/방법/color:white/에 뭔가가 있구나?)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그것도 불가능해. 너는 오히려 엘리베이터를 탔다면, 마르셀과 무조건 마주쳤을 거야!", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 리네 다중 추리 파트 연결
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "MultipleRineBrokenElevator"; // 고장 난 승강기 초기화
                SubmitPanel.SetActive(true);
            }));

        DialogManager.Show(dialogTexts);

    }

    public void RineBrokenElevatorSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb//sound:ding/조수가 그랬어. 4시간 전부터 엘리베이터 중 하나가 사용 불가였다고. 그 시간이면 사건이 일어나기 전부터야.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Emb/그럼 결국 엘리베이터는 마르셀이 사용한 시간에는 1개만 정상 작동했을 텐데... 너가 탔다는 엘리베이터는 뭐야?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp/......고, 고장......? 저는, 그러니까...... 엘리베이터를......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Surp/봤다고? 눌렀다고? 탔다고? 근데 그게 고장이라면, 넌 대체 어떻게 의무실로 내려간 거야?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+Ignore/아... 그, 그러니까.... 저기... 마, 맞아! 저도 당시 충격이 커서 착각했나 봐요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/(눈에 띄게 동요하고 있어. 결계까지 이 정도로 일렁이는 거면, 거의 다 왔어...!)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/리네. 너가 피해자의 방에 갔던 거는 사실이야. 그리고 의무실에 도착해 있는 것도 사실이지.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Ignore/하지만 너는 그 중간 과정을 계속 숨기고 있어. 왜? 어째서?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/저, 저는......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/답은 하나야. 그게 너의 /color:red/정체/color:white/와 연결되어 있으니까. 맞지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/......무, 무슨 말씀을.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp/그리고 그런 너가 방에서 나올 수 있었던 루트도... 하나 있었어.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Surp/(리네가 방에서 빠져나올 수 있는 길은 단 하나뿐이야. 누구에게도 들키지 않고, 흔적도 남기지 않는- 그 길.)", "Latte_Left+Rine_Right", "라떼,",
            () =>
            {   // 리네 다중 추리 파트 연결
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "MultipleRineWindow"; // 고장 난 승강기 초기화
                SubmitPanel.SetActive(true);

            }));


        DialogManager.Show(dialogTexts);

    }

    public void RineOpenWindowSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp//sound:ding/창문.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/......!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/그때 방 창문은 열려 있었지. 그리고 너는- 거기로 나간 거야.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Ignore/아, 아니에요! 창문에서 1층까지 내려오다니... 그러다가 크게 다친다고요! 제, 제가 그런 대담한 일을 할 수 있을 리도 없고요... 네?!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Ignore/글쎄? 탈출 루트가 창문이라면, 너의 /color:red/정체/color:white/를 충분히 유추할 수 있거든.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/아... 아... 아니야......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Surp/아니야... 전 진짜 아니에요...... 그러니까 제발 멈춰주세요... 네...?!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Surp/(슬슬 가짜 가면이 벗겨지기 시작했어. 이제 확실하게 정체를 밝혀내자!)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 리네 마음의 벽 파트 이동
                SoundManager.instance.BGMChgnaer("Monochrome"); // 모노크롬 bgm
                walloftutoPanel.SetActive(true);
           
            }));


        DialogManager.Show(dialogTexts);

    }
        public void StartWallOfInquiry()
    {
        WallofInquiryManager.SetActive(true);
        WallofInquiryManager.GetComponent<WallofInquirySystem>().StartRine();
        WallofInquiryPanel.SetActive(true);
    }



    public void RineMultipleSubmitError(object sender, EventArgs eventArgs)
    {
        PrinterOnOff(true);
        var dialogTexts = new List<DialogData>();



        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/그거는... 무슨 뜻이죠? 역시 뭔가 오해를 하신 건가요?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Normal/(아차! 이게 아니구나.)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Normal/(신중하게 다시 생각하자. 반드시 진짜 모습을 드러내게 만들어야 해.)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                PrinterOnOff(false);
                SubmitPanel.SetActive(true);

            }));


        DialogManager.Show(dialogTexts);

    }


    // 마물 정체 찾기 파트
    public void WhoisRine(object sender, EventArgs eventArgs)
    {

        WallofInquiryManager.SetActive(false);
        DialogPanel.SetActive(true);
        var dialogTexts = new List<DialogData>();


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/그래, 너의 가장 결정적인 증거는 바로 창문에서 나던 피 냄새야. 그 피 냄새는 일반적인 냄새랑 달랐거든.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Emb/......아, 알겠어요. 인정할 테니까, 제발 여기서 그만......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rine_Right+Emb/미안, 난 정체까지 밝히는게 일이거든. /emote:Latte_Left+FThink/그 피 냄새는 마치 안개 같으면서도 박쥐 같은 날개가 희미하게 보였지.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left + FThink//emote:Rine_Right+Emb/의사 선생님. 이걸로 확실해졌어... 너의 정체는 바로......", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                SoundManager.instance.BGMChgnaer("DreamintheFog"); // 드림 인 포그 bgm
                DialogPanel.SetActive(false);
                DemonIdentityPanel.SetActive(true);

            }));

        DialogManager.Show(dialogTexts);
    }

    public void ThatisRine(object sender, EventArgs evnetArgs)
    {
        SoundManager.instance.BGMMuteOnOff(true);
        DialogPanel.SetActive(true);
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("흡혈귀......", "", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+Surp/피를 마시는 밤의 마물. 그게 바로 너의 진짜 모습이야!", "Rine_Center", "라떼"));
        dialogTexts.Add(new DialogData("으으으...... 저, 저는......", "", "리네"));

        dialogTexts.Add(new DialogData("안 되는데... 진짜 안 되는데......", "", "리네",
              () =>
              {  // 리네 panel on
                  SoundManager.instance.BGMChgnaer("Emptyhouse"); // 엠티하우스 bgm
                  SoundManager.instance.BGMMuteOnOff(false);
                  RineVampPanel.SetActive(true);

              }));
        dialogTexts.Add(new DialogData("/sound:female_crying/진짜 그동안 잘 숨기고 있었는데... 어째서.", "", "리네"));
        dialogTexts.Add(new DialogData("후훗, 탐정인 나한테는 아무것도 못 숨겨!", "", "라떼"));
        dialogTexts.Add(new DialogData("드디어 잡았네, 이번 사건에서 숨어있던 마물을.", "", "라떼",
            () =>
            {
                // 리네 PANEL OFF
                RineVampPanel.SetActive(false);
                Background[0].GetComponent<Image>().sprite = backSprites[3]; // 호텔 베이스먼트 이미지
                RineVampStory();
            }));


        DialogManager.Show(dialogTexts);

    }

    // 루시 추리 전 문장완성 파트 (뱀파이어 리네)

    public void RineVampStory()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Center+Emb//sound:magic/휴우... 드디어 끝났구나. 나도 아까 그 공간은 기분 나빠서 싫단 말이지.", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Emb//emote:Siwoo_Right+Smile/탐정님! 다 끝나신 건가요? 정체를 밝히는 데 성공하셨군요?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Smile/아, 조수!! 후훗~ 당연하지, 내가 누구인데~.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/그럼... 정말로 리네 씨가......", "Latte_Left+Siwoo_Right", "시우",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[2]; // 블랙 이미지
            }));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+Vamp/맞아. 저 의사 선생이 바로 인간인 척을 하며 숨어있던 /color:red/마물/color:white/이었어.", "Rine_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+Vamp/(피를 빨아 먹어서 살아가는 흡혈귀. 의사라는 직업을 가진 흡혈귀라니, 뭔가 재밌는 조합이야.)", "Rine_Center", "라떼",
            () =>
            {
                Background[0].GetComponent<Image>().sprite = backSprites[3]; // 호텔 베이스먼트 이미지

            }));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+VampScared/으으... 겨우 피 냄새도 참고 있었는데......", "Rine_Center", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/어쨌든 슬슬 전부 이야기를 들어보실까? 박쥐 언니?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Think/탐정님, 정말로 괜찮으시겠어요? 갑자기 저희를 물기라도 하면 어쩌죠?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Think/걱정 마~ 만약에라도 조수 목을 물려고 하면, 내가 먼저 저 여자 목을 물어뜯을 거니까.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Ridic/아니, 탐정님도 물지 마세요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Rine_Right+VampScared/무, 물지 않을 게요...... 그러니까 제 얘기를 들어주세요......!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+VampScared/알겠으니까, 진정하고 말해봐. 너가 누구고, 무슨 일 있었는지 전부!", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Vamp/ㄴ, 네...! 저는 여기 호텔 의무실에서 의사로서 살고 있던 흡혈귀예요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Vamp/거의 탐정 님께서 하신 말씀이 다 맞아요. 의무실로 비상 콜이 울려서 갔더니, 이미 말라서 사망하신 상태였어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+VampScared/그런 상황에서... 갑자기 들어오신 남성 분까지 쓰러지시고, 밖에서는 다른 사람의 목소리가 들려왔어서 너무 무서웠어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+VampScared/이거 오해 받기 좋은 상황이다... 여기서 얼른 나가야 한다... 그런 생각만 해서, 창문을 통해서 의무실로 돌아왔어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+VampScared/흐음~ 즉, 네가 죽인게 아니라고 말하고 싶은 거지?? 그런데 진짜 의사는 맞았던 거야? 용케도 인간 세계에서 직업을 구했네.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/그게... 아까 그 밀령사 분들의 정식 검사를 통과해서 합법적으로 살고 있었어요...... 비록 24시간 감시를 받지만요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Vamp/아... 너도 감시 당하는 쪽이구나? 고생이 많겠네.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+VampScared/그래도 밀령사 분들이 나름 챙겨주실 잘 챙겨주셔요. 평상시에 식사도 챙겨주시고......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+VampScared/(밥? 혹시 의무실에 있던 그것들 말하는 건가?)", "Latte_Left+Rine_Right", "라떼", () =>
        {
            // 리네 뱀파이어 다중 추리 파트 연결
            PrinterOnOff(false);
            NoteItemsManager.Instance.submitTargetName = "VampRineBloodyPack"; // 뱀파 리네 혈액팩 초기화
            SubmitPanel.SetActive(true);

        }));


        DialogManager.Show(dialogTexts);
    }

    // 뱀파이어 리네 시작

    public void RineVampBloodyPackSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+VampScared//sound:ding/의무실에 가득 했던 그것들이 전부 밥이었어...? 너 은근 많이 먹는 구나?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Vamp/드, 든든하게 먹어야지 냄새를 맡아도 본능을 참을 수 있어서요. 특히 지금처럼 마스크를 쓰지 않았을 때는......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/그런데 왜 AB형은 없던 거야? 조수가 다른 혈액형은 다 있는데, 그것만 없다고 하더라고.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/아... 제가 알레르기가 있어서요. AB형은 못 먹어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Vamp//sound:ding/(AB형은 못 먹는다고...? 잠깐만... 그러면 문제가 생기는데?)", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+Vamp/(/color:red/그 단서/color:white/ 때문에 지금까지의 추리가 틀려지잖아!)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 리네 뱀파이어 다중 추리 파트 연결
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "VampRineVictimInfo"; // 뱀파 리네 피해자의 정보 초기화
                SubmitPanel.SetActive(true);

            }));
        DialogManager.Show(dialogTexts);

    }

    public void RineVampVictimInfoSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/너... 그거 정말이야? AB형은 못 먹는다는 게?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/네? 아... 네, 맞아요. 밀령사 측에서도 인증 받은 사실이니까... 여기 지갑에 인증서도 있어요!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Vamp//sound:ding/이거는 진단서? 밀령사 인증 마크가 있잖아? 근데 이거를 왜 지갑에 넣고 다녀?? 신분증이야??", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+VampScared/실은... 이것도 감시용 위치 추척 부적이어서요......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rine_Right+VampScared/우와... 어지간히도 심하네.", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+VampScared/(어쨌든 그 녀석들 인증이 있으니까 거짓말은 아니겠어. 그럼 리네의 범행은 애초에 성립할 수가 없잖아?)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {

                // 단서 13 혈액팩 인덱스 갱신
                NoteItemsManager.Instance.itemsDescription[13] = "흡혈귀인 리네가 먹는 밥이었어. 근데 AB형 알러지 때문에 AB형만 없더라? 탈쟁이들 인증도 있으니 거짓말은 아니야.";

            }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+Vamp/그, 그런데... 혹시 제 마스크 관련해서는 어떻게 아셨어요?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Vamp/마스크? 아~ 마스크 쓰지 않으면 힘들다는 거?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Vamp/네... 실은 제가 마스크를 쓴 이유는 평상시에 피 냄새를 맡지 않으려고 가리는 용도였어요. 그 사실을 아는 분은 별로 없는데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+Vamp/그래? 누구누구 아는데?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rine_Right+VampScared//sound:ding/인간계에 있는 분들 중에서는... 온라인에서 만난 /color:red/루시/color:white/라는 친구뿐이에요. 너무 답답해서 확 김에 말했는데... 다행히 흡혈귀 컨셉으로 알아들으셨어요.", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+VampScared/(루시...? 루시는 분명 피해자가 만나려고 했던 여자잖아?! /emote:Latte_Left+FSurp/ 여기서 그 이름이 나온다고...? 아니, 그보다 내가 마스크에 대해서 들었던 사람은 분명......)", "Latte_Left+Rine_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+Vamp/혹시... 그 마스크에 관한 이야기를 혼잣말한 적은 없지?", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+VampScared/네...?! 그, 그럴리가요...!! 안 그래도 온라인 친구한테 멋대로 말했다가 밀령사 분들한테 경고받았는데......", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+VampScared/경고 3번 받으면 추방인데... 그, 그러니까 절대 그런 일은 없었어요! 맹세해요!", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rine_Right+VampScared/(하긴 우리 마물들은 정체를 숨겨야 하는데, 혼잣말을 했을 가능성은 낮지. 지금 말이 사실이라면...... 그럼 그 녀석의 말이 이상해.)", "Latte_Left+Rine_Right", "라떼",
            () =>
            {
                // 단서 15 또다른 용의자 획득

                Show_Clue(15); items[15].SetActive(true); submititems[15].SetActive(true); StartCoroutine(TalkPrinterOff(printerImage, namepanelmage, namepanelText));
            }));

        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Right+Smile//wait:2/조수~ 미안한데 한번만 더 부탁할게. 박쥐 언니는 정답이 아니었어!", "Siwoo_Left+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Right+Smile/네? 그럼 살인자가 따로 있다는 건가요?", "Siwoo_Left+Latte_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Right+Exc/맞아, 그러니까 조수는 내가 말하는 사람... /emote:Latte_Right+Normal/아니, 사람이 아닐지도 모르는 그 녀석을 데려와 줘.", "Siwoo_Left+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Right+Normal/이제는 진짜 끝낼 시간이야.", "Siwoo_Left+Latte_Right", "라떼", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[2]; //블랙 스크린
            SoundManager.instance.BGMMuteOnOff(true); // 노 비지엠
        }));
        // 마르셀 최종 추리 파트 이동
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Think/(리네는 흡혈귀였다. 마물을 찾아낸 건 성공이지만... 사건은 아직 끝나지 않았어.)", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+Normal/(그래도 괜찮아! 덕분에 새로운 정보를 알았으니까.)", "Latte_Center", "라떼", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[3]; //호텔 베이스먼트
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+Think//emote:Siwoo_Right+Normal/탐정님, 데려왔습니다.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/그래, 다시 한번 확인할게 생겨서 불렀어. 이야기를 해줄 수 있지, 마르셀?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/어라~? 아까 다 이야기했는데, 더 궁금한게 있나요?", "Latte_Left+Marcel_Right", "마르셀",
            () =>
            {
                SoundManager.instance.BGMMuteOnOff(false); //  비지엠 뮤트 해제
                SoundManager.instance.BGMChgnaer("Umwelt"); // BGM UMWELT

            }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Smile/맞아~ 지금은 탐정으로서 진지해질 시간이야. 몇 가지만 더 물어볼 테니까, 너무 긴장하지는 말고.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/좋아요, 뭐든지 제가 아는 선에서 답해드릴게요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal/오케이! 조수는 이따가 내가 부탁하면, 탈쟁이한테 가서 말해줘.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Center+Normal//emote:Marcel_Right+Smile/아, 알겠습니다. 그럼 밀령사 쪽에도 진행하라고 신호 넣겠습니다.", "Latte_Left+Siwoo_Center+Marcel_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Exc/하하, 뭐를 그렇게 준비하시는 건지 모르겠지만, 분위기가 너무 무겁네요. 아까처럼 조금 더 즐겁게......", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Exc/미안~ 나도 아까처럼 산책 얘기나 수다 떠는 걸 더 좋아하는데... 아무래도 지금은 힘들 거 같아.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/(어쩌면 지금이 가장 중요한 순간이니까.)", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                SoundManager.instance.BGMChgnaer("SchoolGhost"); // BGM UMWELT

            }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/그나저나 범인은 이미 잡힌 거 아니었나요? 반대쪽에서 다 듣고 있었어요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/모기도 못 잡으실 것처럼 소심해 보였던, 그 리네 씨가 괴물이었다니. 역시 현실은 신기한 일이 많다니까요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/뭐, 의사 선생이 흡혈귀인 거는 사실이지만. 사람을 죽이지는 않았어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Exc/어라? 정말요? 와~ 그런 반전이 있었다니! 탐정님께서 그렇게 생각하신 이유가 뭐죠?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/흡혈귀면 어떤 피든지 다 빨아서 먹을 수 있을 거 같은데. 혹시 아닌가요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/아쉽게도 못 먹는 피도 있다고 하더라고~. 그게 뭐냐면......", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                // 마르셀 루시 다중 추리 파트 혈액팩 이동
                MarcelStart();

            }));
        DialogManager.Show(dialogTexts);

    }

    public void RineVampSubmitError(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();
        PrinterOnOff(true);

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+VampScared/......? 죄송해요. 그건 무슨 뜻이죠...?", "Latte_Left+Rine_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Rine_Right+VampScared/아하하... 실수야, 실수!!", "Latte_Left+Rine_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rine_Right+Vamp/이번에는 제대로 보여줄게, 기다려!", "Latte_Left+Rine_Right", "라떼", () =>
        {
            PrinterOnOff(false);
            SubmitPanel.SetActive(true);
        }));

        DialogManager.Show(dialogTexts);

    }


    #endregion

    #region MarcelReasoning

    public void MarcelStart()
    {
        PrinterOnOff(false);
        NoteItemsManager.Instance.submitTargetName = "MarcelBloodyPack"; // 마르셀 루시 다중 추리 파트 혈액팩 초기화
        SubmitPanel.SetActive(true);
    }


    public void MarcelBloodyPackSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();

        PrinterOnOff(true);


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal//sound:ding/리네는 피를 마시는 흡혈귀가 맞지만, AB형은 알레르기 때문에 못 마셔. 이건 탈쟁이들도 인정한 사실이야!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/오~ 괴물들도 알레르기가 있나요? 조금 의외네요. 그리고 알레르기가 무죄라는 증거면... 피해자가 AB형인 건가요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/맞아! 피해자는 AB형이었어. 그러니까 리네가 피를 빨아 먹어서 죽이는 건 불가능해.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/으음... 그러면 도대체 누가 한 걸까요? 당시의 시신이 있던 방에 들어가신 분은 리네 씨랑 티모테 씨니까......", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/아! 그럼 티모테 씨가 범인인가요? 하긴 그분은 기억이 애매하다고 하신 게 수상했죠.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/아니야, 그 안경 말고 더 수상한 용의자가 있어. 피해자가 이곳으로 오게 유도하고, 또 이곳에 왔던 흔적까지 있는 인물.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/(처음에는 그냥 넘어갔지만, 이제는 확실해. 그자가 가장 수상해!)", "Latte_Left+Marcel_Right", "라떼", () =>
        {
            PrinterOnOff(false);
            NoteItemsManager.Instance.submitTargetName = "MarcelAnoSuspect"; // 마르셀 또다른 용의자 초기화
            SubmitPanel.SetActive(true);
        }));
        DialogManager.Show(dialogTexts);

    }

    public void MarcelAnoSuspectSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();
        PrinterOnOff(true);

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile//sound:ding/루시, 그렇게 불리고 있는 여자야.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/루시요...? 처음 듣는 이름이어서 잘 모르겠는데, 설명해주실 수 있나요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/티모테가 그랬는데, 이번에 죽은 피해자는 '루시'라는 여자를 만나기 위해서 이 호텔에 왔다고 말했어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/심지어 아까 전에 통화를 했던 기록에 따르면, 피해자가 죽기 직전에 어떤 여자랑 함께 있는 소리를 들었다고 했어. 수상하지 않아?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/그런가요? 전화 너머로 들었다는 목소리가 정말로 루시인지도 모르고, 겨우 그런 걸로는 뭔가 아쉬운데요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/맞아, 이것만 있었으면 단순히 우연이라고 생각했을 거야. 그런데 이게 끝이 아니야.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Smile/루시는 리네랑도 연결되어 있어. 리네의 단 하나 뿐인 친구라고 했거든!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Exc/와~ 하나뿐인 친구라니, 그 정도로 소중한 존재라는 뜻인가요? 좀 부럽네요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Exc/음... 그런 뜻은 아니지만, 지금 중요한게 아니니까 넘어가고./emote:Latte_Left+FNormal/루시는 이 사건에서 많이 중요한 비밀을 알고 있어.", "Latte_Left+Marcel_Right", "라떼"));


        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/비밀이요? 도대체 무슨 비밀이죠?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/리네의 정체, 자신이 /color:red/흡혈귀/color:white/라는 사실을 밝혔다고 했지. 근데 이번 사건 현장은 리네가 의심 받도록 몇몇 단서들이 유도하고 있어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/하필 그 타이밍에 리네를 피해자 방으로 누군가가 불렀고, 하필 그 방에 루시가 있었다는 흔적이 있고, 심지어 미라 시신의 목 상처도 마치 전형적인 흡혈귀에 물린 느낌. 쎄하지?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/즉, 루시라는 여자가 리네 씨에게 누명을 씌우려고 했다는 건가요? 확실히 수상하네요~. 그런데 그 여성분은 어떻게 잡죠? 이 방에는 없잖아요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/아니, 루시는 이 안에 있어. 우리가 하는 일이 뭐라고 했는지 기억해?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/......저희 안에 숨어있는 괴물을 찾는 거였죠?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/맞아! 그리고 우리 중에는 자신의 모습을 숨긴 루시가 있을 가능성이 높아.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal/(그리고 숨어있는 루시의 정체를 알 수 있는 단서는 이거야!)", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "MarcelRinesMask"; // 마르셀 리네의 마스크 초기화
                SubmitPanel.SetActive(true);

            }));


        DialogManager.Show(dialogTexts);
    }

    public void MarcelRinesMaskSubmit(object sender, EventArgs eventArgs)
    {
        var dialogTexts = new List<DialogData>();
        PrinterOnOff(true);

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal//sound:ding/리네가 루시에게 밝힌 비밀은 정체뿐만이 아니야. 리네한테는 피 냄새를 가리는 마스크가 없으면 일상이 힘들다는 비밀이 있었어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/본인 말로는 그 비밀을 '단 한 명의 친구'. 즉, /color:red/루시/color:white/에게만 밝혔다고 말했어.", "Latte_Left+Marcel_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/.......", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/자, 이제 말해볼까? 너는 어떻게 알고 있던 거야?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Smile/흐음... 제가 의사 선생님께서 혼잣말하시는 거를 우연히 들었다면요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/그건 아니야. 리네가 직접 혼잣말을 한 적이 없다고 밝혔어. 상식적으로 우리 마물들이 스스로 자기 비밀을 혼잣말 할 일도 없잖아?", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/제가 사실 루시 씨랑 아는 사이인데 괜히 의심 받기 싫어서 모르는 척을 했다면요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Exc/그건 더 이상해. 마치 루시가 유력 용의자가 될 거를 처음부터 알았다는 뜻이잖아.", "Latte_Left+Marcel_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/.......즉, 탐정님은 제가 /color:red/루시/color:white/라고 생각하시는 거죠?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Smile/하지만 전 남자인데요? 루시 씨는 여자라고 하지 않았나요?", "Latte_Left+Marcel_Right", "마르셀"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/우리 마물 중에는 변신할 수 있는 마물들도 있어. 그중에는 성별까지 바꾸는 놈들도 있지.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/하하... 너무 억지 아닌가요? 탐정님은 상상력이 참 풍부하시네요.", "Latte_Left+Marcel_Right", "마르셀"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Normal/내가 머리가 좀 좋기는 하지! 그게 망상일지 아닐지는 지금부터 알아보자고.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/.......우리 둘만 있을 수 있는 장소에서 말이야.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Surp/......네?", "Latte_Left+Marcel_Right", "마르셀"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+Exc//emote:Marcel_Right+Surp/조수!! 지금이야!!", "Latte_Left+Marcel_Right", "라떼", () =>
        {
            SoundManager.instance.BGMMuteOnOff(true);
            Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 화면
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Surp//sound:magic/......? 지금 뭘 하시는...... 으악!!", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("(좋아, 이제 슬슬 끝낼 시간이야. 이 사건의 진실을 밝힐 시간.)", "", "라떼",
            () =>
            {
                SoundManager.instance.BGMMuteOnOff(false);
                Background[0].GetComponent<Image>().sprite = backSprites[4]; //스피릿츄어아이즈

            }));
        dialogTexts.Add(new DialogData("/emote:Marcel_Center+Surp/으윽... 여기는 뭐지...? 이상하게 기분 나빠......", "Marcel_Center", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Surp/지금부터 너의 가면을 벗길 무대야! /emote:Latte_Left+FNormal/......지금부터는 조금 더 진지하게 갈게.", "Latte_Left+Marcel_Right", "라떼"));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/하하... 재밌네요. 이런 장소까지 오다니, 오늘은 정말 신기한 경험을 많이 하네요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/심지어 제가 사실 여자라니. 우와~ 남자 모습을 한 용의자가 사실 여성 괴물이었다! 꽤 참신한 추리인걸요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Normal/그럼 말씀해보세요. 제가 루시가 맞는지를 떠나서......", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/애초에 그 루시라는 분, 진짜 있다면 '정체’가 뭐죠? 그걸 알아낼 정보가 있나요?", "Latte_Left+Marcel_Right", "마르셀"));

       
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Smile/(루시의 정체... 만약 사건을 일으킨 녀석이 루시가 맞다면, 그곳에 있던 단서들 중에 /color:red/정체/color:white/를 말해주는게 있어!)", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                SoundManager.instance.BGMChgnaer("Monochrome"); // 모노크롬 bgm
                // 루시 마음의 벽 파트 이동
             
                WallofInquiryManager.SetActive(true);
                WallofInquiryManager.GetComponent<WallofInquirySystem>().StartMarcel();
                WallofInquiryMarcelPanel.SetActive(true);

            }));
        DialogManager.Show(dialogTexts);
    }


    public void WhoisMarcel(object sender, EventArgs eventArgs)
    {
        SoundManager.instance.BGMChgnaer("SchoolGhost"); // 스쿨 고스트 bgm 변경

        WallofInquiryManager.SetActive(false);
        WallofInquiryMarcelPanel.SetActive(false);
        PrinterOnOff(true);
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Normal/\"/color:red/루시가 피해자 방에 남긴 흔적은 보이지 않는 냄새야!/color:white/\"", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Surp/냄새요...? 설마 시신이 있는 방에서 뭔가를 맡으신 건가요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Marcel_Right+Surp/맞아, 방을 조사했을 때에 여러 냄새들을 맡았는데... 그중에서 특히 이상한 효과를 가진 냄새가 있었어.", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                //단서 제시 이상한 향기
                PrinterOnOff(false);
                NoteItemsManager.Instance.submitTargetName = "MarcelAnomalySmell"; // 마르셀 또다른 용의자 초기화
                SubmitPanel.SetActive(true);

            }));



        DialogManager.Show(dialogTexts);
    }

    public void MarcelAnomalySmellSubmit(object sender, EventArgs eventArgs)
    {
        PrinterOnOff(true);

        var dialogTexts = new List<DialogData>();
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Surp/정확히는 나는 보이기만 하고 못 맡았는데, 조수가 느낀 향기가 있어. 조수는 그걸 맡았더니 정신이 멍해진다고 했고.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Surp/심지어 리네랑 티모테 때도 비슷한 상황이 나왔어. 리네는 아무 냄새도 못 맡았는데, 티모테는 뭔가 달콤한 냄새가 난다면서 그대로 잠들었지.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Surp/마치... /color:red/남자들만 재우는 느낌/color:white/이야. 마침 피해자도 남자면서, 시신이 침대 위에 누워있는 모습이었어.", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/......그래서 결론은 뭐죠? 그렇게까지 확신을 가지셨다면, 답이 뭔지 들려주세요.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Marcel_Right+Normal//sound:ding/응, 하나 떠올랐어. 키워드가 만약 /color:red/잠/color:white/이라면...... 루시의 정체이자, 너의 정체는 바로......", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                //마물 정체 선택지 이동
                SoundManager.instance.BGMChgnaer("DreamintheFog"); // 드림 인 포그 bgm
                MarcelDemonIdentityPanel.SetActive(true);

            }));
        DialogManager.Show(dialogTexts);

    }

    // 마물 정체 파트 정답
    public void ThatisMarcel(object sender, EventArgs eventArgs)
    {
        SoundManager.instance.BGMMuteOnOff(true);
        MarcelDemonIdentityPanel.SetActive(false);
        PrinterOnOff(true);

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("꿈을 이용해서 인간의 생명력을 빨아먹는 존재.", "", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FExc/바로 /color:red/몽마/color:white/야!", "Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Marcel_Right+Surp/......하하하! /emote:Marcel_Right+Exc/와, 정말 대단하시네요! 역시 탐정님답달까요?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Marcel_Center+Smile/즐거웠으니까, 특별히 탐정님이라면 보여드릴게요. 저의 또 하나의 모습을./sound:evil_girl_smile/", "Marcel_Center", "마르셀", ()
            =>
        {
            SoundManager.instance.BGMChgnaer("AsyourWish"); // bgm 애즈 유어 위시
            SoundManager.instance.BGMMuteOnOff(false);
            RucyCGPnael.SetActive(true);
            Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 배경 이미지
        }));

        dialogTexts.Add(new DialogData("맞아요. 제가... 아니, 내가 바로...... 루시야.", "", "루시"));
        dialogTexts.Add(new DialogData("어때? 이게 내 진짜 모습이야. 이쪽이 훨씬 보기 좋지?", "", "루시"));
        dialogTexts.Add(new DialogData("생각보다 쉽게 인정하네...? 네가 피해자까지 죽인 게 맞아?", "", "라떼"));
        dialogTexts.Add(new DialogData("으음... 솔직히 아직 부족해. 벌써 결론을 내기에는 성급하지 않아?", "", "루시"));
        dialogTexts.Add(new DialogData("뭐...?! 아직 뭐가 부족하다는 거야?", "", "라떼", () =>
        {
            RucyCGPnael.SetActive(false);
            Background[0].GetComponent<Image>().sprite = backSprites[4]; // 스피릿츄어 아이즈 이미지
        }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Normal/혹시 모르잖아? 사실 방에 있던 달콤한 향기는 나랑 관련이 없었고, 사실 전화로 들었다는 여자 목소리가 착각일지.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Smile/정확히 내가 어떻게 범행을 저질렀는지 설명해 봐. 처음부터 끝까지 납득이 가도록. 그게 바로 /color:red/훌륭한 탐정/color:white/이잖아?", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Smile/(......탐정. 그래, 나는 탐정이야.)", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rucy_Right+Smile/(탐정으로서 사건의 진실을 확실하게 말해주자!)", "Latte_Left+Rucy_Right", "라떼",
        () =>
            {
                //루시 퍼즐 맞추기 진행
                SoundManager.instance.BGMChgnaer("DreamintheFog"); // 드림 인 포그 bgm
                RucyCGPnael.SetActive(false);
                ZigsawPuzzleSystemPanel.SetActive(true);
                ZigsawPuzzleRucyPanel.SetActive(true);
            }));

        DialogManager.Show(dialogTexts);

    }



    public void MarcelSubmitError(object sender, EventArgs eventArgs)
    {
        PrinterOnOff(true);
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Exc/탐정님도 참~ 그게 도대체 무슨 의미가 있다는 거죠?", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Marcel_Right+Smile/역시 본인 추리에 확신이 없으신 거죠? 그러니까 이런 실수를 하지.", "Latte_Left+Marcel_Right", "마르셀"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Marcel_Right+Smile/아, 아니야! 그냥 너가 제대로 집중하고 있는지, 집중력 테스트였어!", "Latte_Left+Marcel_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Marcel_Right+Normal/(또 망신 당할 수는 없어. 이번에는 제대로 단서를 보여주자.)", "Latte_Left+Marcel_Right", "라떼",
            () =>
            {
                PrinterOnOff(false);
                SubmitPanel.SetActive(true);

            }));

        DialogManager.Show(dialogTexts);

    }


    // 루시 직소 퍼즐 파트 종료

    public void FindRucy(object sender, EventArgs eventArgs)
    {
        SoundManager.instance.BGMChgnaer("AsyourWish"); // bgm 애즈 유어 위시
        PrinterOnOff(true);
        ZigsawPuzzleSystemPanel.SetActive(false);
        ZigsawPuzzleRucyPanel.SetActive(false);

        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Smile//sound:ding/와~ 훌륭해! 이렇게까지 완벽하게 정리할 줄은 몰랐는걸?", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rucy_Right+Normal/맞아, 나는 아래에서 위로. 그대로 쉽게 통과해서 에이든을 만나러 갔어. 마치 바닷속에서 모습을 드러내는 인어 아가씨처럼.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rucy_Right+Normal/우리는 즐거운 시간을 보냈어. 웃고 떠들고... 아마 그이는 사랑을 느꼈을 지도 몰라. 난 완벽하니까.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rucy_Right+Smile/그 뒤는...... 뭐, 예상 가지? 난 그이를 재웠어. 아주아주 깊은 잠에 말이지.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Normal/그러고서 난 그 '박쥐 아가씨'가 의심받도록, 시체 목에 '이빨 자국'도 일부러 남겨줬어. 우리 아가씨가 말한 탈 쓴 양반들이 시체를 발견했을 때의 보험으로.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Smile/그 아가씨가 패닉에 빠져 '창문'으로 도망치며 '피 냄새'까지 남길 줄은 몰랐지만! 꽤 명탐정이네? 인정할게! 넌 정말 재밌는 친구야.", "Latte_Left+Rucy_Right", "루시", () =>
        {
            Ending();
        }));


        DialogManager.Show(dialogTexts);
    }


    #endregion

    // 엔딩

    public void Ending()
    {
        PrinterOnOff(true);


        Background[0].GetComponent<Image>().sprite = backSprites[3]; // 호텔 베이스먼트






        ;
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSmile//emote:Latte_Right+FNormal/탐정님! 무사하셨... /emote:Siwoo_Left+FSurp/그쪽은 누구시죠...?", "Siwoo_Left+Latte_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Right+Exc/아~ 조수!! 이 여자가 바로 이번 미라 사건의 진범이야! 마르셀이면서 루시인, 2가지 모습을 가진 몽마!", "Siwoo_Left+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Center+FSmile//emote:Rucy_Right+Normal/자~ 나쁜 마물도 찾았으니 슬슬 추방을......", "Siwoo_Left+Latte_Center+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+FSmile//emote:Rucy_Right+Normal/왜... 그러셨죠? 도대체 왜 사람을 죽이신 거죠?", "Siwoo_Left+Latte_Center+Rucy_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+FSmile//emote:Rucy_Right+Smile/......어머, 나한테 하는 소리야?", "Siwoo_Left+Latte_Center+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Right+Surp/으잉? 조수! 그런 쓸데없는 건 왜 물어보는 거야? 쟤는 마물이야, 마물! 사람을 죽이는데 이유가 있을 리가 없잖아.", "Siwoo_Left+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FAnnoy//emote:Latte_Right+Surp/탐정님도 마물이지만 아무 이유 없이 사람을 해치지는 않으시잖아요. 리네 씨도 흡혈귀지만 의사로서 일하고 계셨고요.", "Siwoo_Left+Latte_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Right+Surp/단순히 마물이라는 이유로 인간을 죽였다...... 그거는 뭔가 납득이 가질 않습니다.", "Siwoo_Left+Latte_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Right+Think/이유가 그렇게 중요할까? 만에 하나 사연이 있더라도 결국 죽은 사람이 나온 일이야. 조수는 불쌍한 녀석일까 봐 걱정하는 거야?", "Siwoo_Left+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FNormal//emote:Latte_Right+Think/그런 거는 아닙니다. 죄를 지었으니 벌을 받는 것은 맞지만, 그래도 이야기를 들어보고 싶어요.", "Siwoo_Left+Latte_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Normal/으음~ 좋아. 이야기해 줄게. 나도 그냥 아무나 죽이는 미친 괴물이 아니거든.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Smile//sound:ding/나는...... 그래, \"사랑\"해줬으니까 죽인 거야.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Rucy_Right+Smile/......네?", "Siwoo_Left+Rucy_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Rucy_Right+Smile/조수 씨는... 좋아하는 사람이 있어? 그 사람의 모든 것을 사랑해? ...정말 상대의 모든 것을 알고 그러는 거야?", "Siwoo_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Rucy_Right+Smile/지, 지금 무슨 말씀 하시는......", "Siwoo_Left+Rucy_Right", "시우", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 배경 이미지
        }));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Normal/사랑, 애정, 연모... 모두 참 오만한 감정이야.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Normal/인간들은 멋대로 단편적인 모습만 보고서 '이해했다'고 착각해. 그게 상대에 대한 모독이자 오만인 줄도 모르고.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Normal/모든 생물은 본인이 아니면 상대의 전부를 알 수 없어. 그런데도 그 허상을 '사랑'이라 믿으며 평생을 바치다니......", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Smile/비극 아니야? 존재하지도 않는 감정에 자기 삶을 전부 소비하는 꼴이잖아.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Smile/그래서 그 비극을 멈춰준 거야. '진짜' 사랑이라고 믿는 그 행복한 꿈속에서... 편안하게 보내준 거지.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Smile/가장 행복한 순간에 멈춰준 거야. 내가 한 일은 살인이 아니라... 자비에 가까운 안락사.", "Rucy_Center", "루시", ()
            =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[3]; // 호텔 베이스먼트
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Smile/안락사는 무슨. 아무리 좋게 말해도, 네가 한 짓은 살인이 맞아.", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Rucy_Right+Smile/그 말이 맞다고 해도...... 딱히 문제 없잖아? 분명 그 인간들도 감사하고 있을 거야.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Center+Smile//emote:Rucy_Right+Normal/봤지, 조수? 모든 마물이 이상한 거는 아니지만, 대부분은 이런 녀석들뿐이야. 괜히 이해하려고 하지 마.", "Siwoo_Left+Latte_Center+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Center+Normal//emote:Rucy_Right+Normal/우리는 인간을 닮을 수 있지만, 끝까지 같아질 수는 없어.", "Siwoo_Left+Latte_Center+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+Normal//emote:Rucy_Right+Normal/......하지만 탐정님은 다르셨잖아요.", "Siwoo_Left+Latte_Center+Rucy_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+Exc//emote:Rucy_Right+Normal/나는 예외!! 나는 착한 마물이니까 안전해.", "Siwoo_Left+Latte_Center+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+Exc//emote:Rucy_Right+Smile/흐음~? 내가 보기에는 멍멍이도 내 쪽 같은데?", "Siwoo_Left+Latte_Center+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FThink//emote:Latte_Center+FNormal//emote:Rucy_Right+Smile/......조수 이제 슬슬 결계가 약해질 거 같으니까, 얼른 추방을 시작해야겠어. 잠시 나가줄래?", "Siwoo_Left+Latte_Center+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Siwoo_Left+FSurp//emote:Latte_Center+FNormal//emote:Rucy_Right+Smile/네? 아... 휘말리지 않기 위해서라고 하셨죠. 알겠습니다, 다른 분들도 데리고 밖에 있겠습니다.", "Siwoo_Left+Latte_Center+Rucy_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+VampScared/어... 어...? 저, 저도 나가도 괜찮은 건가요?", "Rine_Center", "리네"));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+VampScared//emote:Latte_Right+Smile/응! 넌 결국 착한 마물이었잖아. 괜히 너도 추방식에 끼어서 가버리면 불쌍하지.", "Rine_Center+Latte_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Rine_Center+FVampScared//emote:Latte_Right+Smile/하, 하지만... 저는 인간계에서 능력을 쓰면 안 된다는 규칙을 어겼는데......", "Rine_Center+Latte_Right", "리네"));
        dialogTexts.Add(new DialogData("/emote:Rine_Left+FVampScared//emote:Latte_Center+Exc/그거는 내가 알아서 처리할 테니까 얼른 나가! 훠이~ 훠이~!", "Rine_Left+Latte_Center", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Smile/후훗, 추방? 상관없어, 해봐.", "Latte_Left+Rucy_Right", "루시"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Rucy_Right+Smile/의외로 순순히 받아들이네. 인간계에 있고 싶지 않아?", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Normal/으음~ 여기가 더 재미있기는 한데, 저쪽도 싫지는 않아. 그야 우리들의 고향이잖아? 집으로 돌아가는 거지.", "Latte_Left+Rucy_Right", "루시", () =>
        {
            SoundManager.instance.BGMMuteOnOff(true);

        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rucy_Right+Normal/미안한데, 네가 돌아갈 곳은 없어.", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Rucy_Right+Surp/......뭐?", "Latte_Left+Rucy_Right", "루시", () =>
        {
            SoundManager.instance.BGMMuteOnOff(false);
            SoundManager.instance.BGMChgnaer("OnenightHappen");
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Rucy_Right+Surp/저쪽으로 돌려보내도 너는 또 말썽을 피울 거고, 또 모종의 방법으로 인간계로 돌아올 수도 있잖아?", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Surp/조수가 사는 세계는 안전해야 해. 모두가 행복하고, 웃을 수 있게.", "Latte_Left+Rucy_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Rucy_Right+Smile/하하... 재밌는 말을 하네 멍멍이? 추방이 아니면 뭔? 저기 탈을 쓴 녀석들한테 날 봉인시키게 하려고?", "Latte_Left+Rucy_Right", "루시", () =>
        {

            Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 배경 이미지
         
        }));
        dialogTexts.Add(new DialogData("아니, 봉인해도 결국 너는 있는 거잖아. 나도 영원히 꿈꾸게 해줄게. 물론 악몽이겠지만.", "", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Surp/잠깐만... 장난이지? 너 지금 뭘 하려는 거야...? /emote:Rucy_Center+Smile/너가 어떤 마물인지 모르겠지만, 겨우 멍멍이가 뭐를......", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Surp//sound:melting/......너, 너 지금 몸이...... /emote:Rucy_Center+Smile/ 아아, 그런 거구나? 그런 거였어...!! 역시 너도 우리 쪽이었어.", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("/emote:Rucy_Center+Normal/아아...... 조금만 더 일찍 만났으면 친구 했을 텐데, 아쉽네. 이렇게 재밌는 친구가 가까이 있었다니... 아쉬워./sound:finalmelting/", "Rucy_Center", "루시"));
        dialogTexts.Add(new DialogData("......", "", "루시", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[3]; // 호텔 베이스먼트
         
        }));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile/으으응....!! 끝났다. 이제야 겨우 사건 마무리!", "Latte_Left", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/결국 이번에도 그렇게 처리하는 건가... 역시 너는 위험한 마물이다. 너가 조수라고 따르는 자가 이 사실을 알면, 어떻게 생각할지 알고는 그러는 건가?", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/어차피 더러운 거는 서로 똑같으니까, 넘어가지? 너희가 'CCTV 기록'이라고 줬던 내용, 그거 흡혈귀를 감시하던 기록이지?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/......용케도 알았군.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Observer_Right+Normal/그래~ 의도가 너무 뻔하잖아. 감시하던 기록이면 그 여자가 무죄라는 사실을 알 텐데, 굳이 애매하게 단서를 준 이유가 뭐겠어?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Observer_Right+Normal/규칙을 어긴 마물이랑 눈엣가시였던 나까지 한 번에 추방하려고 했겠지.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Observer_Right+Normal/대충 흡혈귀는 규칙 위반으로, 나는 엉뚱한 자를 검거한 오판으로 둘 다 정리... 맞지?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Observer_Right+Normal/하지만 너희가 조직적으로 했다기에는 어딘가 어설프단 말이지... 너희라면 완전히 조작도 가능했을 텐데.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FThink//emote:Observer_Right+Normal/......하고 싶은 말이 뭐지?", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Observer_Right+Normal/간단해! 너도나도 서로 비밀! 조용히 넘어가자는 거야~.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/아니면... 너희 대표인 그 여우한테 가서 말해볼까?", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/...알겠다. 받아들이지. 이번만큼은 눈을 감아주마.", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Observer_Right+Normal/하지만 한 가지만 답해라. 도대체 무엇을 위해 이런 짓까지 하는 거지?", "Latte_Left+Observer_Right", "밀령사 눈"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSurp//emote:Observer_Right+Normal/응? 아까 말했잖아. 그야 당연히 조수를 위해서지.", "Latte_Left+Observer_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Center+FSmile/난 조수의 곁에 계속 있고 싶으니까.", "Latte_Center", "라떼", () =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[0]; // 호텔 로비
        }));

        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal//sound:walking/이걸로 이번 사건도 해결!! 조수~ 이제 놀자!!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/......네 수고하셨습니다. 밀령사에서 마물 흔적을 없애기 위해 티모테 씨의 기억을 조작해서 실종 사건으로 처리 중이라고 해요. 리네 씨는... 경고로 끝났고요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Think/세상에 마물 사건이 알려지면 안 된다는 점은 이해하지만, 기억까지 건드리는 건 찝찝하네요.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Think/뭐~ 어쩔 수 없잖아? 오히려 알려지면 사회가 혼란스러워질 거야.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FEmb//emote:Siwoo_Right+Annoy/그리고 루시 씨도... 저는 탐정님을 보고서, 마물이어도 인간과 지금보다 더 자유롭게 공존할 수 있을지도 모른다고 생각했는데...", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Annoy/조수!! 일 끝났으니까, 복잡한 생각은 끝!! 쉴 때는 확실하게 쉬어야지 나중에 능률도 오른다고. 그니까 얼른 산책 가자!!", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FExc//emote:Siwoo_Right+Normal/......네, 알겠습니다. 약속했으니까, 같이 산책하죠.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Normal/좋아! 오늘은 조금 많이 걷자. 적당히 3시간 정도?", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Right+Smile/하하... 그거는 조금 봐주시죠.", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Normal/......조수, 나 오늘 잘했지? 그러면 칭찬해 줘. 착한 강아지라고 머리 쓰다듬어줘.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Right+Surp/무슨 그런 칭찬을 듣고 싶어 하세요?", "Latte_Left+Siwoo_Right", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FAnnoy//emote:Siwoo_Right+Surp/빨리~! 머리 쓰다듬어주면서 말해줘. 나 이번에 노력했단 말이야.", "Latte_Left+Siwoo_Right", "라떼"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FNormal//emote:Siwoo_Center+Ridic/하아... 네네, 탐정님은 정말 훌륭하고 착한 탐정이에요.", "Latte_Left+Siwoo_Center", "시우"));
        dialogTexts.Add(new DialogData("/emote:Latte_Left+FSmile//emote:Siwoo_Center+Ridic/헤헤, 응! 나는 훌륭한 탐정이야! 앞으로도 조수를 위해서 어떤 사건이든 멋지게 해결할게!", "Latte_Left+Siwoo_Center", "라떼", ()
            =>
        {
            Background[0].GetComponent<Image>().sprite = backSprites[2]; // 검은 배경 이미지

        }));
        dialogTexts.Add(new DialogData("(...그래. 모든 것은 다 조수와 함께 있기 위해서야.)", "", "라떼"));
        dialogTexts.Add(new DialogData("(조수가 바라본 ‘탐정’은 이런 모습이었겠지.)", "", "라떼"));
        dialogTexts.Add(new DialogData("(그러니까... 마물인 나는 오늘도 그런 탐정이 되어야 해.)", "", "라떼"));
        dialogTexts.Add(new DialogData("/sound:ding/(우리는 인간을 닮을 수 있지만, 끝까지 같아질 수는 없으니까.)", "", "라떼", () =>
        {
            SceneManager.LoadScene("StartMain");
        }));

        DialogManager.Show(dialogTexts);
    }

}
