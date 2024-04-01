using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public CanvasRenderer image;
    public PrototypePuzzle prototypePuzzle;

    // Update is called once per frame
    void Update()
    {
        image.GetMaterial().mainTextureOffset += new Vector2(prototypePuzzle.scrollSpeed * Time.deltaTime, 0);
    }
}
