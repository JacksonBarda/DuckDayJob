using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum ClutterType
{
    Pencil,
    Paper,
    Paperclip,
    Other
}

public class DeskDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private Image item;
    [SerializeField]
    private ClutterType type;

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
        if (type == ClutterType.Pencil) AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_PencilNoise");
        else if (type == ClutterType.Paper) AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_PaperRustle");
        else if (type == ClutterType.Paperclip) AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_PaperclipNoise");
        else AudioManager.PlaySoundOnce(AudioManager.Instance.sourceList[3], SoundType.InteractableSFX, "ISFX_GenericClutterNoise");

    }
}
