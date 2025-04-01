using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargeOnMouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float enlargeSize = 2f;

    void Start()
    {
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)          //allows for fragments to be rotated without mouse drag
    {
        Debug.Log("EOMH: pointer enter");
        gameObject.transform.localScale = new Vector3(enlargeSize, enlargeSize, enlargeSize);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("EOMH: mouse exit");
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
