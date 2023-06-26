using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class PurchaseToggle : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Image toggleImage;
    private bool isOn;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOn)
        {
            toggleImage.sprite = offSprite;
            isOn = false;
            OnToggleOff();
        }
        else
        {
            toggleImage.sprite = onSprite;
            isOn = true;
            OnToggleOn();
        }
    }

    protected abstract void OnToggleOn();
    protected abstract void OnToggleOff();
}
