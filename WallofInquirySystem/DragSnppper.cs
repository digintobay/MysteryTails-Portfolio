using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class DragSnapper : UIBehaviour, IEndDragHandler, IBeginDragHandler
{
    public ScrollRect scrollRect;
    public SnapDirection direction = SnapDirection.Vertical;
    public int itemCount = 5;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    public float speed = 2f;

    private Coroutine snapRoutine;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (snapRoutine != null) StopCoroutine(snapRoutine);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float pos = (direction == SnapDirection.Horizontal)
            ? scrollRect.horizontalNormalizedPosition
            : scrollRect.verticalNormalizedPosition;
        snapRoutine = StartCoroutine(SnapRect(pos));
    }

    public void NextSnap()
    {
        float pos = (direction == SnapDirection.Horizontal)
            ? scrollRect.horizontalNormalizedPosition
            : scrollRect.verticalNormalizedPosition;

        pos += (1f / (float)(itemCount - 1)) / 1.5f;
        snapRoutine = StartCoroutine(SnapRect(pos));
    }

    public void PreviousSnap()
    {
        float pos = (direction == SnapDirection.Horizontal)
            ? scrollRect.horizontalNormalizedPosition
            : scrollRect.verticalNormalizedPosition;

        pos -= (1f / (float)(itemCount - 1)) / 1.5f;
        snapRoutine = StartCoroutine(SnapRect(pos));
    }

    private IEnumerator SnapRect(float startNormal)
    {
        if (scrollRect == null) yield break;
        if (itemCount <= 1) yield break;

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

       
        float scrollableRange = content.rect.height - viewport.rect.height;
        if (scrollableRange <= 0f) yield break;


        float delta = (1f / (itemCount - 1)) * (viewport.rect.height / content.rect.height);

        int target = Mathf.RoundToInt(startNormal / delta);
        target = Mathf.Clamp(target, 0, itemCount - 1);
        float endNormal = delta * target;

        float duration = Mathf.Abs((endNormal - startNormal) / speed);
        float timer = 0f;

        while (timer < 1f)
        {
            timer = Mathf.Min(1f, timer + Time.deltaTime / duration);
            float value = Mathf.Lerp(startNormal, endNormal, curve.Evaluate(timer));

            if (direction == SnapDirection.Horizontal)
                scrollRect.horizontalNormalizedPosition = value;
            else
                scrollRect.verticalNormalizedPosition = value;

            yield return null;
        }
    }
}

public enum SnapDirection
{
    Horizontal,
    Vertical,
}