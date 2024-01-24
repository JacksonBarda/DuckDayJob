using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmailDecrypt : Interactable
{

    [SerializeField]
    private List<Text> numberColumns;
    [SerializeField]
    private List<GameObject> boxColors;
    [SerializeField]
    private List<RectTransform> rect;

    private int focusedColumn;
    private List<int> correctNumbers;
    private List<bool> isAnimating;
    private List<float> speeds;
    private bool inPuzzle = false;
    private List<float> timers;

    public override void Interact()
    {
        //fade.FadeImageOverTime(0.7f, this);
        player.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        InitializePuzzle();
        inPuzzle = true;
        StartCoroutine(FloatingNumbersAnimation());
        AudioManager.Instance.PlayMusic("Hacking");
    }

    public override void Action()
    {

    }
    public override void Complete()
    {
        base.Complete();
        inPuzzle = false;

        AudioManager.Instance.PlaySFX("SFX_Complete");
        //location of puzzle
        AudioManager.Instance.PlayMusic("Lobby");
    }
    void Update()
    {
        if(player.puzzleMode)
        {
            CheckLocking();
        }

    }

    private void InitializePuzzle()
    {
        timers = new List<float>(numberColumns.Count);

        Debug.Log(boxColors[1]);
        foreach (GameObject GO in boxColors)
        {
            GO.GetComponent<Image>().color = new Color32(164, 61, 53, 255);
            Debug.Log(GO);
        }
        correctNumbers = new List<int>(numberColumns.Count);
        for (int i = 0; i < numberColumns.Count; i++)
        {
            timers.Add(0f); // Initialize timers with zeros
            numberColumns[i].text = Random.Range(0, 10).ToString();
            correctNumbers.Add(i); // Use Add method to add elements to the list
        }
    }



    private void CheckLocking()
    {
        if (inPuzzle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               
                int lockedNumber = int.Parse(numberColumns[focusedColumn].text);
                
                if (lockedNumber == correctNumbers[focusedColumn])
                {
                    AudioManager.Instance.PlaySFX("SFX_Decrypt");
                    Debug.Log(focusedColumn);
                    boxColors[focusedColumn].GetComponent<Image>().color = new Color32(79, 154, 53, 255);
                    if (focusedColumn == 9)
                    {
                        Debug.Log("Finished");
                        Complete();
                        return;
                    }

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
        List<int> currentNumbers = new List<int>(numberColumns.Count);
        speeds = new List<float>(numberColumns.Count);
        isAnimating = new List<bool>(numberColumns.Count);

        for (int i = 0; i < numberColumns.Count; i++)
        {
            speeds.Add(Random.Range(0.5f, 0.6f)); // Use Add method to add elements to the list
            isAnimating.Add(true); // Use Add method to add elements to the list
           
        }
        for (int i = 0; i < numberColumns.Count; i++)
        {
            int number;
            if (int.TryParse(numberColumns[i].text, out number))
            {
                currentNumbers.Add(number);
            }
            else
            {
                Debug.LogError("Failed to parse number from text: " + numberColumns[i].text);
            }
        }



        while (true)
        {
            for (int i = 0; i < numberColumns.Count; i++)
            {     
                float duration = speeds[i]; // Set desired duration (in seconds) for changing numbers
                if (i < focusedColumn) // Stop updating timer when the number is correctly selected
                {
                    continue;
                }

                timers[i] += Time.deltaTime;

                if (timers[i] >= duration)
                {
                    timers[i] = 0f;
                    currentNumbers[i] = (currentNumbers[i] + 1) % 10;
                    numberColumns[i].text = currentNumbers[i].ToString();
                }
            }
            yield return null;
        }
    }

    private IEnumerator ResetPuzzle()
    {

        AudioManager.Instance.PlaySFX("SFX_Error");
        for (int i = 0; i < numberColumns.Count; i++)
        {

            speeds[i] = Random.Range(0.5f, 0.7f);
            isAnimating[i] = true;
        }
        foreach (GameObject GO in boxColors)
        {
            GO.GetComponent<Image>().color = new Color32(164, 61, 53, 255);
        }
        focusedColumn = 0;
        yield return new WaitForSeconds(1f);


    }


}
