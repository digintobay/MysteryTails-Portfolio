using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject uiNotePanel;
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            uiNotePanel.SetActive(!uiNotePanel.activeSelf);
        }

      
    }
}
