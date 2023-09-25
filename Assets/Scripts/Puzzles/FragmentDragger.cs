using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FragmentDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CADPuzzle CadPuzzle;

    [HideInInspector]
    public Vector3 originalPosition;
    private Image fragment;

    private void Start()
    {
        fragment = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CadPuzzle.selectedFragment = fragment;
        originalPosition = fragment.rectTransform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CadPuzzle.selectedFragment != null)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(fragment.transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);
            fragment.rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (CadPuzzle.selectedFragment != null)
        {
            CadPuzzle.CheckSnap(fragment);
            CadPuzzle.selectedFragment = null;
        }
    }
}
