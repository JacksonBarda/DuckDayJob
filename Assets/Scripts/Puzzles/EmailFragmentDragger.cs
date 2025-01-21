using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EmailFragmentDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private EmailDecrypt DecryptPuzzle;

    [HideInInspector]
    public Vector3 originalPosition;
    private Image fragment;
    [SerializeField]
    public bool isPlaced = false;

    private void Start()
    {
        fragment = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isPlaced)
        {
            originalPosition = fragment.rectTransform.position;
            DecryptPuzzle.selectedFragment = fragment;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DecryptPuzzle.selectedFragment != null && !isPlaced)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(fragment.transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPoint);
            fragment.rectTransform.anchoredPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DecryptPuzzle.selectedFragment != null)
        {
            DecryptPuzzle.CheckSnap(fragment);
            DecryptPuzzle.selectedFragment = null;
        }
    }

    public void FragmentPlaced()
    {
        isPlaced = true;
    }
}