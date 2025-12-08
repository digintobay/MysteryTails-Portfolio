using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogManager : MonoBehaviour
{
    public TextMeshProUGUI printerText;

    public Transform logTransform;
    public TextMeshProUGUI[] logsTexts;
    int logIndex = 0;

    private void Start()
    {
        LogObjectStruct();


    }

    void LogObjectStruct()
    {
        logsTexts = new TextMeshProUGUI[logTransform.childCount];

        for (int i = 0; i < logsTexts.Length; i++)
        {
            logsTexts[i] = logTransform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }


 
    void AddLog()
    {
        logsTexts[logIndex].text = printerText.text;
        logIndex++;

        if (logIndex >= logsTexts.Length)
            logIndex = 0; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddLog(); 
        }
    }
}
