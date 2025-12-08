using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DemonIdentitySystem : MonoBehaviour
{

    public event EventHandler ThisisRine;
    public event EventHandler NotRine;

    public event EventHandler ThisisRucy;
    public event EventHandler NotRucy;

    public GameObject failPanel;
    public GameObject DemonIdentityPanel;

    public GameObject M_failPanel;
    public GameObject M_DemonIdentityPanel;

    [Header("버튼")]
    public Transform buttonParent;
    public Transform M_buttonParent;

    [Header("버튼 프리팹")]
    public GameObject buttonPrefab;
    public GameObject M_buttonPrefab;

    [Header("팝업 프리팹")]
    public GameObject popupPrefab;
    public GameObject M_popupPrefab;

    [Header("버튼 텍스트 배열")]
    public string[] buttonTexts;
    public string[] M_buttonTexts;

    [Header("팝업 이미지 스프라이트 배열")]
    public Sprite[] popupSprites;
    public Sprite[] M_popupSprites;

    [Header("팝업 메인 텍스트 배열")]
    public string[] mainTexts;
    public string[] M_mainTexts;


    [Header("팝업 인포 텍스트 배열")]
    public string[] infoTexts;
    public string[] M_infoTexts;

    [Header("팝업 부모 패널")]
    public Transform popupParent;
    public Transform M_popupParent;

    private void Start()
    {
        GenerateButtons();
        M_GenerateButtons();
    }

    private void GenerateButtons()
    {
        for (int i = 0; i < buttonTexts.Length; i++)
        {
            int index = i;


            GameObject newButton = Instantiate(buttonPrefab, buttonParent);


            TextMeshProUGUI textComp = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
                textComp.text = buttonTexts[index];


            Button btn = newButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => OnButtonClicked(index));
            }
        }
    }

    private void M_GenerateButtons()
    {
        for (int i = 0; i < M_buttonTexts.Length; i++)
        {
            int index = i;

            GameObject newButton = Instantiate(M_buttonPrefab, M_buttonParent);

            TextMeshProUGUI textComp = newButton.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
                textComp.text = M_buttonTexts[index];

            Button btn = newButton.GetComponent<Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(() => M_OnButtonClicked(index));
            }
        }
    }

    private void OnButtonClicked(int index)
    {
        GameObject newPopup = Instantiate(popupPrefab, popupParent);

        Transform mask = newPopup.transform.Find("Mask");
        Transform imageTransform = mask.transform.Find("Image");
        if (imageTransform != null)
        {
            Image img = imageTransform.GetComponent<Image>();
            if (img != null && index < popupSprites.Length)
                img.sprite = popupSprites[index];
        }
        TextMeshProUGUI[] texts = newPopup.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var t in texts)
        {
            if (t.name == "MainText" && index < mainTexts.Length)
                t.text = mainTexts[index];
            else if (t.name == "InfoText" && index < infoTexts.Length)
                t.text = infoTexts[index];
        }

        Button[] buttons = newPopup.GetComponentsInChildren<Button>(true);
        foreach (var b in buttons)
        {
            if (b.name == "ConfirmButton")
            {
                int capturedIndex = index;
                b.onClick.AddListener(() => OnConfirmClicked(capturedIndex));
            }
            else if (b.name == "ExitButton")
            {
                GameObject popupToClose = newPopup;
                b.onClick.AddListener(() => ClosePopup(popupToClose));
            }
        }
    }

    private void M_OnButtonClicked(int index)
    {
        GameObject newPopup = Instantiate(M_popupPrefab, M_popupParent);

        Transform mask = newPopup.transform.Find("Mask");
        Transform imageTransform = mask.transform.Find("Image");
        if (imageTransform != null)
        {
            Image img = imageTransform.GetComponent<Image>();
            if (img != null && index < M_popupSprites.Length)
                img.sprite = M_popupSprites[index];
        }
        TextMeshProUGUI[] texts = newPopup.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var t in texts)
        {
            if (t.name == "MainText" && index < M_mainTexts.Length)
                t.text = M_mainTexts[index];
            else if (t.name == "InfoText" && index < M_infoTexts.Length)
                t.text = M_infoTexts[index];
        }

        Button[] buttons = newPopup.GetComponentsInChildren<Button>(true);
        foreach (var b in buttons)
        {
            if (b.name == "ConfirmButton")
            {
                int capturedIndex = index;
                b.onClick.AddListener(() => M_OnConfirmClicked(capturedIndex));
            }
            else if (b.name == "ExitButton")
            {
                GameObject popupToClose = newPopup;
                b.onClick.AddListener(() => M_ClosePopup(popupToClose));
            }
        }
    }

    private void OnConfirmClicked(int index)
    {
        Debug.Log($"확정 버튼 클릭됨 - 인덱스: {index}");


        if (index == 1)
        {
            DemonIdentityPanel.SetActive(false);
            ThisisRine(this, EventArgs.Empty);
        }
        else
        {
            failPanel.SetActive(true);

        }
    }

    private void M_OnConfirmClicked(int index)
    {
        Debug.Log($"확정 버튼 클릭됨 - 인덱스: {index}");


        if (index == 2)
        {
            M_DemonIdentityPanel.SetActive(false);
            ThisisRucy(this, EventArgs.Empty);
        }
        else
        {
            M_failPanel.SetActive(true);

        }
    }

    private void ClosePopup(GameObject popup)
    {
        if (popup != null)
        {
            Destroy(popup);
        }
    }

    private void M_ClosePopup(GameObject popup)
    {
        if (popup != null)
        {
            Destroy(popup);
        }
    }
}
