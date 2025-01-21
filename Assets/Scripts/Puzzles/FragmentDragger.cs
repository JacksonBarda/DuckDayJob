using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FragmentDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    [SerializeField]
    private CADPuzzle CadPuzzle;

    [HideInInspector]
    public Vector3 originalPosition;
    private Image fragment;
    private bool isPlaced = false;

    private void Start()
    {
        fragment = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = fragment.rectTransform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)          //allows for fragments to be rotated without mouse drag
    {
        if (eventData.pointerCurrentRaycast.gameObject != null && !isPlaced)
        {
            CadPuzzle.selectedFragment = fragment;
        }
    }
    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (eventData.pointerCurrentRaycast.gameObject == null)
    //    {
    //        CadPuzzle.selectedFragment = null;
    //    }
    //}

    public void OnDrag(PointerEventData eventData)
    {
        if (CadPuzzle.selectedFragment != null && !isPlaced)
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

    public void FragmentPlaced()
    {
        isPlaced = true;
    }
}
