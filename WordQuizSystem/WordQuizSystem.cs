using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class WordQuizSystem : MonoBehaviour
{
    public event EventHandler First_WordComplete;
    public event EventHandler Secon_WordComplete;
    public event EventHandler Third_WordComplete;
    public event EventHandler Error_Word;

    [Header("텍스트 패널")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI wordQuizText;

    [SerializeField] private TextMeshProUGUI titleText_02;
    [SerializeField] private TextMeshProUGUI wordQuizText_02;

    [SerializeField] private TextMeshProUGUI titleText_03;
    [SerializeField] private TextMeshProUGUI wordQuizText_03;


    [Header("단어 패널")]
    [SerializeField] private GameObject wordQuizButton;

    public Transform buttonPanel;
    public Transform buttonPanel_02;
    public Transform buttonPanel_03;

    public GameObject wordQuizPanelA;
    public GameObject wordQuizPanelB;
    public GameObject wordQuizPanelC;

    private string[] titlePanelText = { "\"각 인물들이 피해자 시신 발견 당시 있던 위치는?\"", "\"3명 중 혼자 다른 정보를 말한 인물은?\"", "\"리네의 증언에서 수상한 부분은?\"" };

    private string[] word_01 = { "피해자의 방", "화장실", "옥상", "복도", "의무실", "로비" };
    private string[] word_02 = { "티모테", "마르셀", "리네" };
    private string[] word_03 = { "의무실", "피해자의 방", "들어가", "나가", "없다", "많다" };

    private string originalSentence = "티모테는 [], 마르셀은 [], 리네는 []에 있었다.";
    private string originalSentence_02 = "[]의 증언에서 모순이 있다.";
    private string originalSentence_03 = "리네가 []에서 []는 모습을 본 자가 [].";

    private string targetSentence = "티모테는 피해자의 방, 마르셀은 복도, 리네는 의무실에 있었다.";
    private string targetSentence_02 = "리네의 증언에서 모순이 있다.";
    private string targetSentence_03 = "리네가 피해자의 방에서 나가는 모습을 본 자가 없다.";

    private int filledCount = 0; private int totalBlanks = 0;
    private int filledCount_02 = 0;
    private int totalBlanks_02 = 0;

    private int filledCount_03 = 0;
    private int totalBlanks_03 = 0;

    private List<Button> createdButtons = new List<Button>();

    private List<Button> createdButtons_02 = new List<Button>();

    private List<Button> createdButtons_03 = new List<Button>();


    private void Start()
    {
        titleText.text = titlePanelText[0];
        titleText_02.text = titlePanelText[1];
        titleText_03.text = titlePanelText[2];

        wordQuizText.text = originalSentence;
        wordQuizText_02.text = originalSentence_02;
        wordQuizText_03.text = originalSentence_03;

        totalBlanks = Regex.Matches(originalSentence, @"\[\]").Count;
        Debug.Log(totalBlanks);
        totalBlanks_02 = Regex.Matches(originalSentence_02, @"\[\]").Count;
        Debug.Log(totalBlanks_02);

        totalBlanks_03 = Regex.Matches(originalSentence_03, @"\[\]").Count;
        Debug.Log(totalBlanks_03);
        Debug.Log("총 빈칸 개수: " + totalBlanks);

        CreateWordButtons();
        CreateWordButtons_02();
        CreateWordButtons_03();
    }


    private void CreateWordButtons()
    {
        for (int i = 0; i < word_01.Length; i++)
        {
            int index = i;
            GameObject buttonObj = Instantiate(wordQuizButton, buttonPanel);

            TextMeshProUGUI tmp = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = word_01[index];

            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                createdButtons.Add(btn);
                btn.onClick.AddListener(() => OnWordButtonClick(btn, word_01[index]));
            }
        }
    }

    private void CreateWordButtons_02()
    {
        for (int i = 0; i < word_02.Length; i++)
        {
            int index = i; GameObject buttonObj = Instantiate(wordQuizButton, buttonPanel_02);

            TextMeshProUGUI tmp = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = word_02[index];

            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                createdButtons_02.Add(btn);
                btn.onClick.AddListener(() => OnWordButtonClick_02(btn, word_02[index]));
            }
        }
    }

    private void CreateWordButtons_03()
    {
        for (int i = 0; i < word_03.Length; i++)
        {
            int index = i; GameObject buttonObj = Instantiate(wordQuizButton, buttonPanel_03);

            TextMeshProUGUI tmp = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null)
                tmp.text = word_03[index];

            Button btn = buttonObj.GetComponent<Button>();
            if (btn != null)
            {
                createdButtons_03.Add(btn);
                btn.onClick.AddListener(() => OnWordButtonClick_03(btn, word_03[index]));
            }
        }
    }




    private void OnWordButtonClick(Button btn, string word)
    {
        btn.interactable = false;

        wordQuizText.text = ReplaceFirst(wordQuizText.text, "[]", word);
        filledCount++;

        if (filledCount >= totalBlanks)
        {
            CheckAnswer();
        }
    }

    private void OnWordButtonClick_02(Button btn, string word)
    {
        btn.interactable = false;

        wordQuizText_02.text = ReplaceFirst(wordQuizText_02.text, "[]", word);
        filledCount_02++;

        if (filledCount_02 >= totalBlanks_02)
        {
            CheckAnswer_02();
        }
    }

    private void OnWordButtonClick_03(Button btn, string word)
    {
        btn.interactable = false;

        wordQuizText_03.text = ReplaceFirst(wordQuizText_03.text, "[]", word);
        filledCount_03++;

        if (filledCount_03 >= totalBlanks_03)
        {
            CheckAnswer_03();
        }
    }


    private void CheckAnswer()
    {
        if (wordQuizText.text == targetSentence)
        {
          
            Debug.Log(realAngs);
            Debug.Log("정답 a");

            First_WordComplete(this, EventArgs.Empty);
            wordQuizPanelA.SetActive(false);
        }
        else
        {
            Debug.Log("오답");

            foreach (Button btn in createdButtons)
            {
                btn.interactable = true;
            }

            wordQuizText.text = originalSentence;
            filledCount = 0;

            Error_Word(this, EventArgs.Empty);
            wordQuizPanelA.SetActive(false);


        }
    }

    private void CheckAnswer_02()
    {
        if (wordQuizText_02.text == targetSentence_02)
        {
       
            Debug.Log(realAngs);
            Debug.Log("정답");
            Secon_WordComplete(this, EventArgs.Empty);
            wordQuizPanelB.SetActive(false);
        }
        else
        {
            Debug.Log("오답");

            foreach (Button btn in createdButtons_02)
            {
                btn.interactable = true;
            }

            wordQuizText_02.text = originalSentence_02;
            filledCount_02 = 0;

            Error_Word(this, EventArgs.Empty);
            wordQuizPanelB.SetActive(false);
        }
    }

    private void CheckAnswer_03()
    {
        if (wordQuizText_03.text == targetSentence_03)
        {
           
            Debug.Log(realAngs);
            Debug.Log("정답");
            Third_WordComplete(this, EventArgs.Empty);
            wordQuizPanelC.SetActive(false);
        }
        else
        {
            Debug.Log("오답");

            foreach (Button btn in createdButtons_03)
            {
                btn.interactable = true;
            }

            wordQuizText_03.text = originalSentence_03;
            filledCount_03 = 0;

            Error_Word(this, EventArgs.Empty);
            wordQuizPanelC.SetActive(false);
        }
    }

    private string ReplaceFirst(string source, string target, string replacement)
    {
        int index = source.IndexOf(target);
        if (index < 0) return source;

        return source.Substring(0, index) + replacement + source.Substring(index + target.Length);
    }


}
