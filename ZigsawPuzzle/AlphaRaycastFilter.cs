using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    private Image image;
    private Texture2D tex;

    void Awake()
    {
        image = GetComponent<Image>();
        if (image.sprite != null)
            tex = image.sprite.texture;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (tex == null || image.sprite == null)
            return false;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            image.rectTransform,
            sp,
            eventCamera,
            out Vector2 localPoint
        );

        Rect rect = image.rectTransform.rect;
        float x = (localPoint.x - rect.x) / rect.width;
        float y = (localPoint.y - rect.y) / rect.height;

        if (x < 0 || x > 1 || y < 0 || y > 1) return false;

        int texX = Mathf.FloorToInt(image.sprite.rect.x + image.sprite.rect.width * x);
        int texY = Mathf.FloorToInt(image.sprite.rect.y + image.sprite.rect.height * y);

        try
        {
            Color color = tex.GetPixel(texX, texY);
            return color.a > 0.1f;
        }
        catch
        {
            return false;
        }
    }
}