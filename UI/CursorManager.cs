using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public ChapterOne finderMousePoint;
    public FinderSystem testingEvents;
    public WallofInquirySystem inquirySystem;

    Texture2D _original;
    Texture2D _leftoriginal;
    Texture2D _readingGlasses;
    Texture2D _pointGlasses;

    //마음의 벽
    Texture2D _wallofWord;
    Texture2D _wallofWordFinder;

    private bool _mouseDownCheck = false;
    private bool _finderCheck = false;
    private bool _wallofCheck = false;


    enum CursorType
    {
        None,
        Find
    }

    //CursorType _cursorType = CursorType.None;


    private void Awake()
    {
        finderMousePoint.FinderMousePointerChange += FinderMouseBasicEvents; //구독
        finderMousePoint.BasicMousePointer += BasicMouseEvnets;
        inquirySystem.BasicWallofWord += WallofWord;
        inquirySystem.PointWallofWord += WallofWordFinder;
        inquirySystem.BasicMouseChange += BasicMouseEvnets;
    }

    public void Start()
    {
        _original = Resources.Load<Texture2D>("Cursor/Basic/Basic");
        _leftoriginal = Resources.Load<Texture2D>("Cursor/Basic/LeftBasic");
        _readingGlasses = Resources.Load<Texture2D>("Cursor/ReadingGlasses/Glasses");
        _pointGlasses = Resources.Load<Texture2D>("Cursor/ReadingGlasses/PointGlasses");
        _wallofWord = Resources.Load<Texture2D>("Cursor/WallofWord/WallofWord");
        _wallofWordFinder = Resources.Load<Texture2D>("Cursor/WallofWord/WallofWordPoint");

        BasicMouse();



        testingEvents = testingEvents.GetComponent<FinderSystem>();
        testingEvents.OnFinderPressed += Test_OnSpacePressed; //구독

    }

    public void Test_OnSpacePressed(object sender, EventArgs eventArgs)
    {
        FinderMouseOn();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !_finderCheck && !_wallofCheck)
        {
            LeftBasicMouse();
            _mouseDownCheck = true;

        }


        if (Input.GetMouseButtonUp(0) && _mouseDownCheck && !_finderCheck && !_wallofCheck)
        {
            BasicMouse();
            _mouseDownCheck = false;
        }




    }


    public void LeftBasicMouse()
    {

        Cursor.SetCursor(_leftoriginal, new Vector2(0, 0), CursorMode.Auto);

    }

    public void BasicMouse()
    {
        Cursor.SetCursor(_original, new Vector2(0, 0), CursorMode.Auto);
    }

    public void FinderMouseOn()
    {
        _finderCheck = true;
        Cursor.SetCursor(_readingGlasses, new Vector2(0, 0), CursorMode.Auto);
    }

    public void FinderPointMouseOn()
    {
        _finderCheck = true;
        Cursor.SetCursor(_pointGlasses, new Vector2(0, 0), CursorMode.Auto);
    }

    // 마음의 벽

    public void WallofWord(object sender, EventArgs eventArgs)
    {
        _wallofCheck = true;
        Cursor.SetCursor(_wallofWord, new Vector2(0, 0), CursorMode.Auto);
    }

    public void WallofWordFinder(object sender, EventArgs eventArgs)
    {
        _wallofCheck = true;
        Cursor.SetCursor(_wallofWordFinder, new Vector2(0, 0), CursorMode.Auto);
    }


    //이벤트용

    public void FinderMouseBasicEvents(object sender, EventArgs eventArgs)
    {
        _finderCheck = true;
        Cursor.SetCursor(_readingGlasses, new Vector2(0, 0), CursorMode.Auto);

    }

    public void BasicMouseEvnets(object sender, EventArgs eventArgs)
    {
        _finderCheck = false;
        BasicMouse();
    }



}




