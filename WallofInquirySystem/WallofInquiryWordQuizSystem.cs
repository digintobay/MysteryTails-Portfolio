using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WallofInquiryWordQuizSystem : MonoBehaviour
{
    [Header("텍스트 패널")]
    [SerializeField] private TextMeshProUGUI[] wordQuizTexts; [SerializeField] private string[] originalSentences; [SerializeField] private string[] targetSentences;
    [Header("단어 버튼")]
    [SerializeField] private GameObject wordButtonPrefab;
    [SerializeField] private Transform[] buttonPanels; [SerializeField] private string[][] words;
    private int[] filledCounts; private int[] totalBlanks;
    private List<Button>[] createdButtons;
    public event EventHandler WordCompleteEvents; public event EventHandler ErrorWordEvent;

    private void Start()
    {
        int n = wordQuizTexts.Length;
        filledCounts = new int[n];
        totalBlanks = new int[n];
        createdButtons = new List<Button>[n];

        for (int i = 0; i < n; i++)
        {
            createdButtons[i] = new List<Button>();
            wordQuizTexts[i].text = originalSentences[i];
            totalBlanks[i] = Regex.Matches(originalSentences[i], @"\[\]").Count;

            CreateWordButtonsFromPanel(i);
        }
    }

    private void CreateWordButtonsFromPanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= buttonPanels.Length) return;

        Transform panel = buttonPanels[panelIndex];

        TextMeshProUGUI[] existingWords = panel.GetComponentsInChildren<TextMeshProUGUI>();

        createdButtons[panelIndex] = new List<Button>();

        foreach (var tmp in existingWords)
        {
            if (tmp == null) continue;

            string word = tmp.text;

            Button btn = tmp.GetComponentInParent<Button>();
            if (btn == null)
            {
                GameObject btnObj = tmp.gameObject; btn = btnObj.GetComponent<Button>();
                if (btn == null)
                {
                    btn = btnObj.AddComponent<Button>();
                }
            }

            createdButtons[panelIndex].Add(btn);

            int closureIndex = panelIndex; btn.onClick.AddListener(() => OnWordButtonClick(closureIndex, btn, word));
        }
    }

    private void OnWordButtonClick(int sentenceIndex, Button btn, string word)
    {
        if (sentenceIndex < 0 || sentenceIndex >= wordQuizTexts.Length) return;

        btn.interactable = false;

        wordQuizTexts[sentenceIndex].text = ReplaceFirst(wordQuizTexts[sentenceIndex].text, "[]", word);
        filledCounts[sentenceIndex]++;

        if (filledCounts[sentenceIndex] >= totalBlanks[sentenceIndex])
        {
            CheckAnswer(sentenceIndex);
        }
    }

    private void CheckAnswer(int sentenceIndex)
    {
        if (sentenceIndex < 0 || sentenceIndex >= wordQuizTexts.Length) return;

        if (wordQuizTexts[sentenceIndex].text == targetSentences[sentenceIndex])
        {
            WordCompleteEvents?.Invoke(this, EventArgs.Empty);
            Debug.Log($"문장 {sentenceIndex + 1} 정답!");
        }
        else
        {
            ErrorWordEvent?.Invoke(this, EventArgs.Empty);
            Debug.Log($"문장 {sentenceIndex + 1} 오답!");

            foreach (var btn in createdButtons[sentenceIndex])
                btn.interactable = true;

            wordQuizTexts[sentenceIndex].text = originalSentences[sentenceIndex];
            filledCounts[sentenceIndex] = 0;
        }
    }

    private string ReplaceFirst(string source, string target, string replacement)
    {
        int index = source.IndexOf(target);
        if (index < 0) return source;
        return source.Substring(0, index) + replacement + source.Substring(index + target.Length);
    }
}
