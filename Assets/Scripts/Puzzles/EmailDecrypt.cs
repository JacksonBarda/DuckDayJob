using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EmailDecrypt : Interactable
{
    public GameObject puzzleUI;
    public GameObject mainUI;

    public Text[] numberColumns;
    public GameObject[] boxColors;
    public RectTransform[] rect;
    private int focusedColumn;
    private int[] correctNumbers;
    private bool[] isAnimating;
    private float[] speeds;
    private bool inPuzzle = false;

    public override void Interact()
    {
        //fade.FadeImageOverTime(0.7f, this);
        player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        InitializePuzzle();
        inPuzzle = true;
        StartCoroutine(FloatingNumbersAnimation());
    }

    public override void Action()
    {

    }
    protected override void Finished()
    {
        inPuzzle = false;
        player.puzzleMode = false;
        puzzleUI.SetActive(false);
        mainUI.SetActive(true);
    }
    void Update()
    {
        HandleInput();
        CheckLocking();
    }

    private void InitializePuzzle()
    {

        foreach (GameObject GO in boxColors)
        {
            GO.GetComponent<Image>().color = new Color(164, 61, 53, 255);

        }
        correctNumbers = new int[numberColumns.Length];
        for (int i = 0; i < numberColumns.Length; i++)
        {
            numberColumns[i].text = Random.Range(0, 10).ToString();
            correctNumbers[i] = i;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && focusedColumn > 0)
        {
            focusedColumn--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && focusedColumn < numberColumns.Length - 1)
        {
            focusedColumn++;
        }
    }

    private void CheckLocking()
    {
        if(inPuzzle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RectTransform columnTransform = numberColumns[focusedColumn].GetComponent<RectTransform>();
                if (rect[focusedColumn].GetComponent<Rect>().Contains(columnTransform.localPosition))
                {
                    int lockedNumber = int.Parse(numberColumns[focusedColumn].text);
                    if (lockedNumber == correctNumbers[focusedColumn])
                    {
                        boxColors[focusedColumn].GetComponent<Image>().color = new Color(79, 154, 53, 255);
                        focusedColumn++;
                    }
                    else
                    {
                        StartCoroutine(ResetPuzzle());
                    }
                }
            }
        }

    }

    private IEnumerator FloatingNumbersAnimation()
    {
        int[] currentNumbers = new int[numberColumns.Length];
        speeds = new float[numberColumns.Length];
        isAnimating = new bool[numberColumns.Length];

        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = Random.Range(0.05f, 0.1f);
            isAnimating[i] = true;
        }

        while (true)
        {
            for (int i = 0; i < numberColumns.Length; i++)
            {
                if (i < focusedColumn) // Stop animating when the number is correctly selected
                {
                    isAnimating[i] = false;
                    continue;
                }

                if (isAnimating[i])
                {
                    RectTransform columnTransform = numberColumns[i].GetComponent<RectTransform>();
                    columnTransform.localPosition += new Vector3(0, -speeds[i], 0);

                    if (columnTransform.localPosition.y < -rect[focusedColumn].localPosition.y)
                    {
                        columnTransform.localPosition = new Vector3(columnTransform.localPosition.x, 0, columnTransform.localPosition.z);
                        currentNumbers[i] = (currentNumbers[i] + 1) % 10;
                        Debug.Log(currentNumbers[i].ToString());
                        numberColumns[i].text = currentNumbers[i].ToString();
                    }
                }
            }
            yield return null;
        }
    }

    private IEnumerator ResetPuzzle()
    {

        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = Random.Range(0.05f, 0.1f);
            isAnimating[i] = true;
        }
        foreach (GameObject GO in boxColors)
        {
            GO.GetComponent<Image>().color = new Color(164, 61, 53, 255);
        }

        yield return new WaitForSeconds(1f);


    }
}

