using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using System.Threading.Tasks;
using System;

public class ZigsawPuzzleSystem : MonoBehaviour
{
    public event EventHandler ZigsawRucyEnd;
    public GameObject ZigSawPanel;
    public GameObject thisPanel;

    [Header("References")]
    public Canvas mainCanvas;
    public Transform puzzlePiecesTrans;
    public Transform rucyImagesTrans;
    public Transform zigsawPoses;
    public Transform puzzleBase;

    [Header("루시 패널")]
    public GameObject RucyImage;

    [Header("Arrays")]
    public GameObject[] puzzlePieces = new GameObject[8];
    public GameObject[] rucysImages = new GameObject[8];
    public GameObject[] thispos = new GameObject[8];
    public string[] puzzlePiecesStrings = new string[8];

    private TextMeshProUGUI[] puzzlePiecesText = new TextMeshProUGUI[8];
    private RectTransform[] targetRects = new RectTransform[8];
    private bool[] isCompleted = new bool[8];
    private Vector2[] originalAnchoredPositions = new Vector2[8];

    private GameObject draggingClone;
    private int draggingIndex = -1;
    private RectTransform draggingRect;
    private Vector2 pointerOffset;

    private TextMeshProUGUI[] pPTextes = new TextMeshProUGUI[8];
    public string[] pPStrings = new string[8];


    void Start()
    {
        StateGroup();

        for (int i = 0; i < puzzlePieces.Length; i++)
        {
            if (puzzlePiecesTrans.childCount <= i) continue;

            puzzlePieces[i] = puzzlePiecesTrans.GetChild(i).gameObject;
            rucysImages[i] = rucyImagesTrans.GetChild(i).gameObject;
            thispos[i] = zigsawPoses.GetChild(i).gameObject;

            var rect = puzzlePieces[i].GetComponent<RectTransform>();
            targetRects[i] = thispos[i].GetComponent<RectTransform>();
            puzzlePiecesText[i] = puzzlePieces[i].GetComponentInChildren<TextMeshProUGUI>();

            if (puzzlePiecesStrings.Length > i)
                puzzlePiecesText[i].text = puzzlePiecesStrings[i];

            originalAnchoredPositions[i] = rect.anchoredPosition;

            int index = i;
            var btn = puzzlePieces[i].GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => OnPuzzlePieceClicked(index));
        }
    }

    void StateGroup()
    {
        pPTextes = puzzleBase.GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < pPTextes.Length; i++)
        {
            pPTextes[i].text = "Q. " + pPStrings[i];
        }
    }

    void Update()
    {
        if (draggingRect != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                mainCanvas.transform as RectTransform,
                Input.mousePosition,
                mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCanvas.worldCamera,
                out Vector2 localPoint
            );

            draggingRect.pivot = new Vector2(0.5f, 0.5f);
            draggingRect.anchorMin = new Vector2(0.5f, 0.5f);
            draggingRect.anchorMax = new Vector2(0.5f, 0.5f);

            draggingRect.anchoredPosition = localPoint;

            if (Input.GetMouseButtonDown(0))
            {
                TryPlacePiece();
            }
        }
    }

    void OnPuzzlePieceClicked(int index)
    {
        if (isCompleted[index]) return;
        if (draggingRect != null) return;


        puzzlePieces[index].SetActive(false);

        draggingClone = Instantiate(puzzlePieces[index], mainCanvas.transform);
        draggingClone.name = puzzlePieces[index].name + "_Clone";
        draggingRect = draggingClone.GetComponent<RectTransform>();

        draggingClone.SetActive(true);
        draggingClone.transform.SetAsLastSibling(); draggingIndex = index;


        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.transform as RectTransform,
            Input.mousePosition,
            mainCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCanvas.worldCamera,
            out Vector2 localPoint
        );

        pointerOffset = localPoint - draggingRect.anchoredPosition;
    }

    public async void TryPlacePiece()
    {
        if (draggingRect == null || draggingClone == null) return;

        bool placed = false;

        for (int i = 0; i < thispos.Length; i++)
        {
            if (isCompleted[i]) continue;

            if (IsRectOverlapping(draggingRect, targetRects[i]))
            {
                if (i == draggingIndex)
                {
                    isCompleted[i] = true;
                    rucysImages[i].SetActive(true);
                    placed = true;

                    if (isCompleted.All(x => x))
                    {
                        Debug.Log("모든 조각이 완성됨!");


                        Destroy(draggingClone);
                        draggingClone = null;
                        draggingRect = null;
                        draggingIndex = -1;

                        RucyImage.SetActive(true);
                        Image rucy = RucyImage.GetComponent<Image>();
                        Color rucyColor = rucy.color;
                        rucyColor.a = 0;
                        rucy.color = rucyColor;

                        float fadeDuration = 2f;
                        float elapsed = 0f;

                        while (elapsed < fadeDuration)
                        {
                            await Task.Yield(); elapsed += Time.deltaTime;
                            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                            rucyColor.a = alpha;
                            rucy.color = rucyColor;
                        }

                        rucyColor.a = 1f;
                        rucy.color = rucyColor;
                        StartCoroutine(RucyPuzzleEnding());

                    }
                }
                break;
            }
        }

        if (!placed)
        {
            puzzlePieces[draggingIndex].SetActive(true);
        }

        Destroy(draggingClone);
        draggingClone = null;
        draggingRect = null;
        draggingIndex = -1;
    }

    bool IsRectOverlapping(RectTransform rect1, RectTransform rect2)
    {
        Rect r1 = GetWorldRect(rect1);
        Rect r2 = GetWorldRect(rect2);
        return r1.Overlaps(r2);
    }

    Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        float xMin = corners[0].x;
        float yMin = corners[0].y;
        float width = corners[2].x - xMin;
        float height = corners[2].y - yMin;
        return new Rect(xMin, yMin, width, height);
    }

    IEnumerator RucyPuzzleEnding()
    {
        yield return new WaitForSeconds(3f);
        ZigSawPanel.SetActive(false);

        ZigsawRucyEnd(this, EventArgs.Empty);
        thisPanel.SetActive(false);
    }
}