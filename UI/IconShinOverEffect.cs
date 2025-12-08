using Coffee.UIEffects;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems; // IPointerEnterHandler, IPointerExitHandler


public class IconShinOverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UIEffect uiEffect;

    private void Awake()
    {
        uiEffect = GetComponent<UIEffect>();
        if (uiEffect == null)
            Debug.LogError("x");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (uiEffect != null)
            uiEffect.enabled = true; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (uiEffect != null)
            uiEffect.enabled = false; 
    }
}
