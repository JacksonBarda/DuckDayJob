using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DeskDragger : Interactable, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    


    private Image item;

    private void Start()
    {
        item = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(item.transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);
        item.rectTransform.anchoredPosition = localPoint;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
