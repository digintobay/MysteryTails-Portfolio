using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUISystem : MonoBehaviour
{
    Texture2D _original;
    Texture2D _leftoriginal;

    public GameObject optionPanel;
    public GameObject creditPanel;

    public bool _mouseDownCheck = false;
    public AudioClip clickSound;

    public void Start()
    {
        _original = Resources.Load<Texture2D>("Cursor/Basic/Basic");
        _leftoriginal = Resources.Load<Texture2D>("Cursor/Basic/LeftBasic");

        SoundManager.instance.BGMChgnaer("mainsong");
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            SoundManager.instance.SFXPlay("click", clickSound);
            LeftBasicMouse();
            _mouseDownCheck = true;
        }

        if (Input.GetMouseButtonUp(0) && _mouseDownCheck)
        {
            BasicMouse();
            _mouseDownCheck = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionPanel.SetActive(false);
            creditPanel.SetActive(false);

        }
    }

    public void PopOption()
    {
        optionPanel.SetActive(true);
    }

    public void PopCredit()
    {
        creditPanel.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("DialogueScene");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void LeftBasicMouse()
    {

        Cursor.SetCursor(_leftoriginal, new Vector2(0, 0), CursorMode.Auto);

    }

    public void BasicMouse()
    {
        Cursor.SetCursor(_original, new Vector2(0, 0), CursorMode.Auto);
    }
}
