using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class NoteItemsManager : MonoBehaviour
{
    public static NoteItemsManager Instance { get; private set; }


    public event EventHandler TimoteSubmit; //티모테 심문 이벤트 
    public event EventHandler TimoteErrorSubmit; // 티모테 심문 에러



    public event EventHandler RineSubmit; // 리네 심문 이벤트
    public event EventHandler RineErrorSubmit; // 리네 심문 에러

    // 전반부 리네 추리 파트 <다중 이벤트>
    public event EventHandler RineCCTVSubmit;
    public event EventHandler RineReporterSubmit;
    public event EventHandler RineBrokenElevatorSubmit;
    public event EventHandler RineOpenWindowSubmit;
    public event EventHandler RineMultipleSubmitError;

    // 후반부 뱀파이어 리네 추리 파트 <다중 이벤트>
    public event EventHandler RineVampBloodyPackSubmit;
    public event EventHandler RineVampVictimInfoSubmit;
    public event EventHandler RineVampSubmitError;

    // 마르셀 추리 파트 <다중 이벤트>
    public event EventHandler MarcelBloodyPackSubmit;
    public event EventHandler MarcelAnoSuspectSubmit;
    public event EventHandler MarcelRinesMaskSubmit;
    public event EventHandler MarcelAnomalySmellSubmit;
    public event EventHandler MarcelSubmitError;


    [SerializeField] private GameObject SubmitInventoryPanel;

    static bool[] itemsIndex = new bool[16];

    private string[] itemsName = new string[16];
    public string[] itemsDescription = new string[16];

    public Sprite[] itemsImage = new Sprite[16];

    public TextMeshProUGUI PanelItemsName;
    public TextMeshProUGUI PanelItemsDescription;

    public TextMeshProUGUI SubmitPanelItems;
    public TextMeshProUGUI SubmitDescription;

    public Image PanelItemsImage;
    public Image SubmitImagePanel;

    public string submitTargetName = "";


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        ItemsNameInfo();
        ItemsDescriptionInfo();
        // 시작 시 모든 아이템 인덱스 false
        for (int i = 0; i< itemsIndex.Length-1; i++)
        {
            itemsIndex[i] = false; 
        }


    }

    //심문 이벤트 구독자 시그니처
    public void Event_Timote(object sender, EventArgs eventArgs)
    {
        Debug.Log("티모테 이벤트 재생");
    }

    private void item_Change(string name, string description, Sprite image)
    {
        PanelItemsName.text = name;
        PanelItemsDescription.text = description;
        SubmitPanelItems.text = name;
        SubmitDescription.text = description;

        PanelItemsImage.sprite = image;
        SubmitImagePanel.sprite = image;
    }

    public void Submit_Index(string index)
    {
        if (submitTargetName == "Timote") // 티모테 추리
        {
            if (index == itemsName[4])
            {
                Debug.Log(index + itemsName[4]);
                itemsIndex[4] = true;
                SubmitInventoryPanel.SetActive(false);
                TimoteSubmit(this, EventArgs.Empty); // 이벤트 핸들러들을 호출 
            }
            else
            {
                Debug.Log("다름");
                SubmitInventoryPanel.SetActive(false);
                TimoteErrorSubmit(this, EventArgs.Empty);


            }
        }
        else if (submitTargetName=="Rine")
        {
            if (index == itemsName[8])// 사라진 리네 호출 시
            {
                Debug.Log(index + itemsName[8]);
                itemsIndex[8] = true;
                SubmitInventoryPanel.SetActive(false);
                RineSubmit(this, EventArgs.Empty); // 이벤트 핸들러들을 호출 
            }
            else
            {
                Debug.Log("다름");
                SubmitInventoryPanel.SetActive(false);
                RineErrorSubmit(this, EventArgs.Empty);


            }
        }   // 다중 이벤트 리네 시작
        else if (submitTargetName == "MultipleRineCCTV")
        {
            if (index == itemsName[1]) // 씨씨티비
            {
                RineCCTVSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("다중 이벤트 리네 오답");
                RineMultipleSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
           
        }
        else if (submitTargetName == "MultipleRineReporter")
        {
            if (index == itemsName[10]) // 신고자의 상황
            {
                RineReporterSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("다중 이벤트 리네 오답");
                RineMultipleSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "MultipleRineBrokenElevator")
        {
            if (index == itemsName[14]) // 고장 난 승강기
            {
                RineBrokenElevatorSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("다중 이벤트 리네 오답");
                RineMultipleSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "MultipleRineWindow")
        {
            if (index == itemsName[2]) // 열린 창문
            {
                RineOpenWindowSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("다중 이벤트 리네 오답");
                RineMultipleSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        } // 뱀파이어 리네 다중 이벤트 시작
        else if (submitTargetName == "VampRineBloodyPack")
        {
            if (index == itemsName[13]) // 혈액팩
            {
                RineVampBloodyPackSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("뱀파이어 리네 오답");
                RineVampSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "VampRineVictimInfo")
        {
            if (index == itemsName[0]) // 피해자의 정보
            {
                RineVampVictimInfoSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("뱀파이어 리네 오답");
                RineVampSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        } // 마르셀 다중 이벤트 추리 파트 시작
        else if (submitTargetName == "MarcelBloodyPack")
        {
            if (index == itemsName[13]) // 혈액팩
            {
                MarcelBloodyPackSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("마르셀 다중 이벤트 오답");
                MarcelSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "MarcelAnoSuspect")
        {
            if (index == itemsName[15]) // 또다른 용의자
            {
                MarcelAnoSuspectSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("마르셀 다중 이벤트 오답");
                MarcelSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "MarcelRinesMask")
        {
            if (index == itemsName[12]) // 리네의 마스크
            {
                MarcelRinesMaskSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("마르셀 다중 이벤트 오답");
                MarcelSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else if (submitTargetName == "MarcelAnomalySmell")
        {
            if (index == itemsName[7]) // 이상한 향기
            {
                MarcelAnomalySmellSubmit(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }
            else
            {
                Debug.Log("마르셀 다중 이벤트 오답");
                MarcelSubmitError(this, EventArgs.Empty);
                SubmitInventoryPanel.SetActive(false);
            }

        }
        else
        {
            Debug.Log("이름이 없음");
        }

       
    }

    //제출 누를 시 들어가는 함수
    public void SubmitButton()
    {
        Submit_Index(SubmitPanelItems.text);
    }

    public void Items00()
    {
        PanelItemsImage.color = new Color(1, 1, 1, 1);
        item_Change(itemsName[0], itemsDescription[0], itemsImage[0]);
    }

    public void Items01()
    {
        item_Change(itemsName[1], itemsDescription[1], itemsImage[1]);
    }

    public void Items02()
    {
        item_Change(itemsName[2], itemsDescription[2], itemsImage[2]);
    }

    public void Items03()
    {
        item_Change(itemsName[3], itemsDescription[3], itemsImage[3]);
    }

    public void Items04()
    {
        item_Change(itemsName[4], itemsDescription[4], itemsImage[4]);
    }

    public void Items05()
    {
        item_Change(itemsName[5], itemsDescription[5], itemsImage[5]);
    }

    public void Items06()
    {
        item_Change(itemsName[6], itemsDescription[6], itemsImage[6]);
    }

    public void Items07()
    {
        item_Change(itemsName[7], itemsDescription[7], itemsImage[7]);
    }

    public void Items08()
    {
        item_Change(itemsName[8], itemsDescription[8], itemsImage[8]);
    }

    public void Items09()
    {
        item_Change(itemsName[9], itemsDescription[9], itemsImage[9]);
    }

    public void Items10()
    {
        item_Change(itemsName[10], itemsDescription[10], itemsImage[10]);
    }

    public void Items11()
    {
        item_Change(itemsName[11], itemsDescription[11], itemsImage[11]);
    }
    public void Items12()
    {
        item_Change(itemsName[12], itemsDescription[12], itemsImage[12]);
    }
    public void Items13()
    {
        item_Change(itemsName[13], itemsDescription[13], itemsImage[13]);
    }
    public void Items14()
    {
        item_Change(itemsName[14], itemsDescription[14], itemsImage[14]);
    }
    public void Items15()
    {
        item_Change(itemsName[15], itemsDescription[15], itemsImage[15]);
    }


    public void ItemsNameInfo()
    {
        itemsName[0] = "피해자의 정보";
        itemsName[1] = "CCTV 기록";
        itemsName[2] = "열린 창문";
        itemsName[3] = "시체 정보";
        itemsName[4] = "피해자의 통화 기록";
        itemsName[5] = "바닥 잔향";
        itemsName[6] = "피 냄새";
        itemsName[7] = "이상한 향기";
        itemsName[8] = "사라진 리네";
        itemsName[9] = "비명 소리";
        itemsName[10] = "신고자의 상황";
        itemsName[11] = "리네의 이동";
        itemsName[12] = "리네의 마스크";
        itemsName[13] = "혈액팩";
        itemsName[14] = "고장난 승강기";
        itemsName[15] = "또다른 용의자";

    }

    public void ItemsDescriptionInfo()
    {
        itemsDescription[0] = "그 가면 탈이 준 피해자 정보! 이름은 에이든, 남자고, 키가 184cm, 몸무게 76kg, 혈액형 AB형... 재미없는 내용뿐이야!";
        itemsDescription[1] = "피해자가 22시 38분에 찍혔고, 이후에 23시 12분에 리네, 13분에 티모테가 들어갔어! 14분에 마르셀이 지나가다가 신고. 근데 앞뒤가 왜 짤린 거지?";
        itemsDescription[2] = "창문이 열려 있었어! 조수가 말하기를 시체 발견 전부터 열려있었다고 하더라. ";

        itemsDescription[3] = "우와! 미라처럼 말라버린 시체라니! 엄청 신기했어~. 그리고 목에 송곳니 같은 거에 물린 상처가 있던데, 이거는 뭘까?";
        itemsDescription[4] = "23시 02분에 티모테라는 자와 통화를 했어. 그거 말고는 없어, 응! 아무 것도 없으니까 절대 보지 말기!!";

        itemsDescription[5] = "바닥에서 웅덩이 같은 냄새가 보였는데, 기분 나빠! 끈적해! 적어도 그 바닥에 뭔가 있었던 거는 분명해.";
        itemsDescription[6] = "창문 쪽에서 진한 피 냄새가 났어! 진짜 너무 진해! 마치 피로 이루어진 뭔가 같아. 아! 그리고 무슨 날개 같은 게 보였어.";
        itemsDescription[7] = "방 안에 희미하지만 향이 채워져 있었어. 나는 아무 냄새도 나지 않았는데, 조수는 달콤한 향이라면서 갑자기 멍해지더라?";
        itemsDescription[8] = "티모테가 깨어났을 때는 리네가 방 안에 없었다고 했어. 그럼 어디로 간 거지? 신기하네~.";

        itemsDescription[9] = "피해자의 방에서 남자 비명 소리가 들려서 마르셀이 신고했다고 했어. 그거 혹시 피해자의 목소리였을까?";
        itemsDescription[10] = "마르셀의 방은 피해자의 방 아래 층이였지? 거기서 엘리베이터를 타고 올라가는 동안 아무도 못 봤다고 말했어.";
        itemsDescription[11] = "시신이랑 쓰러진 티모테를 보고 놀란 리네는 의무실로 돌아갔다고 했어. 의사면서 시신도 못 보는 거야?";
        itemsDescription[12] = "마르셀이 알려줬는데, 겨우 마스크를 쓰지 않았다고 사람을 보기 힘들어한다네? 리네라는 여자는 낯을 엄청나게 가리나 봐.";
        itemsDescription[13] = "의무실에 왜 혈액팩이 가득했을까. AB형은 없다고 했는데 특별한 이유가 있는 걸까?";
        itemsDescription[14] = "조수가 엘리베이터 중 하나가 4시간 전부터 고장 때문에 이용 불가라고 했어! 다들 1개만 쓰고 불편했을 텐데 제대로 알려주지, 모르는 사람도 있었을 거야!";
        itemsDescription[15] = "계속 루시라는 여자가 사건에서 언급이 나오고 있어. 리네의 정체랑 마스크에 대해서 유일하게 아는 친구면서, 에이든의 소개팅 상대라니... 우연인가?";


    }




}
