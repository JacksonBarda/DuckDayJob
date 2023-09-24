using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EmailDecrypt : Interactable
{
    public GameObject puzzleUI;
    public GameObject mainUI;
    public RectTransform greenBox;
    public Text[] numberColumns;
    private int focusedColumn;
    private int[] correctNumbers;

    public override void Interact()
    {
        //fade.FadeImageOverTime(0.7f, this);
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        InitializePuzzle();
        StartCoroutine(FloatingNumbersAnimation());
    }

    public override void Action()
    {

    }
    protected override void Finished()
    {

    }
    void Update()
    {
        HandleInput();
        CheckLocking();
    }

    private void InitializePuzzle()
    {
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RectTransform columnTransform = numberColumns[focusedColumn].GetComponent<RectTransform>();
            if (greenBox.rect.Contains(columnTransform.localPosition))
            {
                int lockedNumber = int.Parse(numberColumns[focusedColumn].text);
                if (lockedNumber == correctNumbers[focusedColumn])
                {
                    numberColumns[focusedColumn].color = Color.green;
                    focusedColumn++;
                }
                else
                {
                    StartCoroutine(ResetPuzzle());
                }
            }
        }
    }

    private IEnumerator FloatingNumbersAnimation()
    {
        int[] currentNumbers = new int[numberColumns.Length];
        float[] speeds = new float[numberColumns.Length];
        bool[] isAnimating = new bool[numberColumns.Length];

        for (int i = 0; i < speeds.Length; i++)
        {
            speeds[i] = Random.Range(0.03f, 0.06f);
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

                    if (columnTransform.localPosition.y < -greenBox.rect.height)
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
        greenBox.GetComponent<Image>().color = Color.red;
        for (int i = 0; i < focusedColumn; i++)
        {
            numberColumns[i].color = Color.red;
        }
        yield return new WaitForSeconds(1f);

        greenBox.GetComponent<Image>().color = Color.green;
        for (int i = 0; i < focusedColumn; i++)
        {
            numberColumns[i].color = Color.white;
        }
        focusedColumn = 0;
    }
}

