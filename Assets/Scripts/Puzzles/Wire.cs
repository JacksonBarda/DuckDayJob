using UnityEngine;
using UnityEngine.UI;

public class Wire : MonoBehaviour
{
    public WirePuzzle WirePuzzle;
    public int wireIndex;

    private Button button;
     
    /* 
    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;
   */

    [SerializeField]
    private Color sprite1;
    [SerializeField]
    private Color sprite2;



    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => WirePuzzle.CutWire(wireIndex));
    }
    private void Update()
    {
        ChangeImages();
    }
    public void ChangeImages()
    {
        if (WirePuzzle.initialStates[wireIndex])
        {
            this.GetComponent<Image>().color = sprite1;
        }else
            this.GetComponent<Image>().color = sprite2;
    }

}
