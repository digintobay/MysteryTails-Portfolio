using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;

    public GameObject[] Example;

    private void Awake()
    {
        var dialogTexts = new List<DialogData>();

        dialogTexts.Add(new DialogData("테스트 화면 비율 00", "Latte_Right"));
        dialogTexts.Add(new DialogData("테스트 화면 비율 01", "Latte_Left"));
        dialogTexts.Add(new DialogData("테스트 화면 비율 02", "Latte_Center"));

        dialogTexts.Add(new DialogData("테스트 화면 비율 03", "Latte_Right + Siwoo_Left"));
        dialogTexts.Add(new DialogData("테스트 화면 비율 04", "Latte_Center + Siwoo_Left + Timote"));

        dialogTexts.Add(new DialogData("/size:up/Hi, /size:init/ /size:down/ 텍스트는 이런 식으로 작성되며/size:init/, 사운드가 있음. ", "Timote"));

        dialogTexts.Add(new DialogData("캐릭터 사운드 재생 가능. /click//sound:ring/ 벨소리, /click//sound:haha/ 웃음소리.", "Lucy_Left"));

        dialogTexts.Add(new DialogData("테스트 화면 비율 05", "Rine"));

        dialogTexts.Add(new DialogData("테스트 화면 비율 06", "Marshel"));
        
        dialogTexts.Add(new DialogData("테스트 화면 비율 07", "Observer"));

        dialogTexts.Add(new DialogData("둘 다 같이 등장!", "TestDummy00 + TestDummy01"));

        dialogTexts.Add(new DialogData("둘 다 같이 표정 변경, /emote:Sad/ 슬픔 01, /emote:Happy/ 행복 00", "TestDummy01 + TestDummy00"));

        dialogTexts.Add(new DialogData("01 캐릭터 표정 변경에 사용될, /click/ /emote:Sad/ 두 번째, /click/ /emote:Happy/ 세 번째", "TestDummy01"));



        dialogTexts.Add(new DialogData("텍스트 색상 변경 /color:red/붉은색, /color:white/그리고 /size:up/ /size:up/ 크기 조정 /size:init/ 방식임.", "TestDummy00"));

      


        dialogTexts.Add(new DialogData("기다림 등의 효과도 사용 가능하다, /wait:0.5/ 웨이팅....", "TestDummy00"));

        dialogTexts.Add(new DialogData("텍스트는 /speed:down/ 느리게... /speed:init//speed:up/ 혹은 빠르게.", "TestDummy00"));

        dialogTexts.Add(new DialogData("/speed:0.1/ 빠를 수도 있다./close/", "TestDummy00"));

        dialogTexts.Add(new DialogData("/speed:0.1/이 것 은 스 킵 할 수 없는 메 시 지 만 들 기.", "TestDummy00", "", null, false));

        dialogTexts.Add(new DialogData("/emote:Happy/파이팅해요 우리 ㅎㅎ ", "TestDummy01"));

        DialogManager.Show(dialogTexts);
    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }
}
