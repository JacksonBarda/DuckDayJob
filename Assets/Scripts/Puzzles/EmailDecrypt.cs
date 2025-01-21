using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EmailDecrypt : Interactable
{
    public GameObject puzzleBarUI;
    public List<Image> fragments;
    public List<Image> outlines;

    public float snapThreshold = 10;

    [HideInInspector]
    public Image selectedFragment;

    private bool partOneComplete = false;

    // part 2 variables below -----------------------

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float sizeChangeSpeed;
 
    private Vector2 targetRedBoxPosition;
    private Vector2 targetWhiteBoxPosition;
    private float targetRedBoxWidth;
    private float targetWhiteBoxWidth;

    private float redboxMinX;
    private float redboxMaxX;
    private float whiteboxMinX;
    private float whiteboxMaxX;

    [SerializeField]
    private Slider SLDR_Progress;

    public GameObject progressBarGroup;
    public Text progressText;
    public Slider progressBar; 
    public RectTransform redBox; 
    public RectTransform whiteBox; 

    public float decryptionSpeed;
    public float decaySpeed;


    public int count = 0;

    private void Start()
    {   decryptionSpeed = 3;
        decaySpeed = 3;
        progressBar.minValue = 0;
        progressBar.maxValue = 100;
        //movementSpeed = 25;
        progressBarGroup.SetActive(false);
    }

    private void Update()
    {
        if (partOneComplete)
        {
            HandleMouseInteraction();
            UpdateBoxMovements();
            UpdateBoxSize();

            progressText.text = "Progress: " + progressBar.value.ToString("#.##") + "%";
            progressText.color = new Color(Mathf.Lerp(0, 1, 1 - progressBar.value / 100), Mathf.Lerp(0, 1, progressBar.value / 100), 0);
        }
    }

    public override void Interact()
    {
        PlayerMove.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        progressBar.value = 0;

        //InitializeProgressBar();
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();

        StopCoroutine(PeriodicRecalculation());

        count = 0;
        AudioManager.Instance.PlaySFX("SFX_Complete");
        foreach (Image fragment in fragments)
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        SLDR_Progress.value++;
    }

    public void CheckSnap(Image fragment)
    {
        if (fragment.gameObject.GetComponent<EmailFragmentDragger>().isPlaced == false)
        {
            
            int index = fragments.IndexOf(fragment);
            float distance = Vector3.Distance(fragment.rectTransform.position, outlines[index].rectTransform.position);

            if (distance <= snapThreshold)
            {
                fragment.rectTransform.position = outlines[index].rectTransform.position;
                fragment.gameObject.GetComponent<EmailFragmentDragger>().FragmentPlaced();
                count++;
                AudioManager.Instance.PlaySFX("SFX_VendingButton");
                fragment.gameObject.transform.SetAsFirstSibling();
            }

            if (count >= fragments.Count)
            {
                //Complete();
                puzzleBarUI.SetActive(true);
                partOneComplete = true;
                InitializeProgressBar();
            }
        }
        
    }

    // --------------- PART TWO CODE -------------------------------------------------------------------------------------

    private void InitializeProgressBar()
    {
        progressBarGroup.SetActive(true);
        Debug.Log("redbox width: " + redBox.sizeDelta.x);
        Debug.Log("whitebox x: " + whiteBox.sizeDelta.x);
        CalculateBoxLimits();
        StartCoroutine(PeriodicRecalculation());

        whiteBox.sizeDelta = new Vector2(12, redBox.rect.height);
        whiteBox.anchoredPosition = new Vector2(-169, 0);
        redBox.sizeDelta = new Vector2(12, redBox.rect.height);
        redBox.anchoredPosition = new Vector2(0, 0);
    }

    private void HandleMouseInteraction()
    {
        // Check if the mouse is within the red box
        if (RectTransformUtility.RectangleContainsScreenPoint(redBox, Input.mousePosition))
        {
            // Update the progress bar based on decryption speed
            progressBar.value += Time.deltaTime * decryptionSpeed;

            // Update the position and size of the red and white boxes periodically
            
        }
        else
        {
            // Reset progress if the mouse is outside the red box
            progressBar.value -= Time.deltaTime * decaySpeed;
        }

        // Check if the progress bar is filled
        if (progressBar.value >= progressBar.maxValue)
        {
            Complete();
        }
    }

    private IEnumerator PeriodicRecalculation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CalculateBoxLimits();
        }
    }

    private void CalculateBoxLimits()
    {
        if (progressBar.value >= 10)
        {
            Debug.Log("Recalculating box limits...");
            targetWhiteBoxWidth = Random.Range(Mathf.Lerp(0, 300, progressBar.value / 100) * 0.1f, Mathf.Lerp(0, 300, progressBar.value / 100) * 0.6f);
            if (targetWhiteBoxWidth < 12) targetWhiteBoxWidth = 12;


            whiteboxMinX = -169 + (targetWhiteBoxWidth / 2);
            whiteboxMaxX = Mathf.Lerp(-175, 175, progressBar.value / 100) - (targetWhiteBoxWidth / 2);
            targetWhiteBoxPosition = new Vector2(Random.Range(whiteboxMinX, whiteboxMaxX), 0);


            targetRedBoxWidth = Random.Range(whiteBox.sizeDelta.x * 0.3f, whiteBox.sizeDelta.x * 0.6f);
            if (targetRedBoxWidth < 12) targetRedBoxWidth = 12;

            redboxMinX = (-targetWhiteBoxWidth / 2) + (targetRedBoxWidth / 2);
            redboxMaxX = (targetWhiteBoxWidth / 2) - (targetRedBoxWidth / 2);

            targetRedBoxPosition = new Vector2(Random.Range(redboxMinX, redboxMaxX), 0);
        }
        else
        {
            targetWhiteBoxWidth = 12;
            targetWhiteBoxPosition = new Vector2(-169, 0);
            targetRedBoxWidth = 12;
            targetRedBoxPosition = new Vector2(0,0);
        }
    }

    void UpdateBoxMovements()
    {
        //movementSpeed = Mathf.Lerp(17, 40, progressBar.value / 100);
        movementSpeed = Random.Range(17,40);
        // Check if the red box has reached its target position
        if ((Vector2)redBox.anchoredPosition != targetRedBoxPosition)
        {
            // Smoothly move the red box towards the target position
            redBox.anchoredPosition = Vector2.MoveTowards(redBox.anchoredPosition, targetRedBoxPosition, Time.deltaTime * movementSpeed);
        }
        else
        {
            // Red box reached the target, pick a new target position
            //CalculateBoxLimits();
            //targetRedBoxPosition = new Vector2(Random.Range(redboxMinX, redboxMaxX), 0);
            //targetRedBoxWidth = Random.Range(whiteBox.rect.width * 0.4f, whiteBox.rect.width * 1.0f);
        }
        
        if ((Vector2)whiteBox.anchoredPosition != targetWhiteBoxPosition)
        {
            // Smoothly move the white box towards the target position
            whiteBox.anchoredPosition = Vector2.MoveTowards(whiteBox.anchoredPosition, targetWhiteBoxPosition, Time.deltaTime * movementSpeed);
        }

        else
        {
            // White box reached the target, pick a new target position
            //targetWhiteBoxWidth = Random.Range(progressBar.GetComponent<RectTransform>().rect.width * 0.1f, progressBar.GetComponent<RectTransform>().rect.width * 0.60f);
            //CalculateBoxLimits();
            //targetWhiteBoxPosition = new Vector2(Random.Range(whiteboxMinX, whiteboxMaxX), 0);
        }
    }

    private void UpdateBoxSize()
    {
        if (redBox.sizeDelta.x != targetRedBoxWidth)
        {
            float redBoxWidth = Mathf.Lerp(redBox.sizeDelta.x, targetRedBoxWidth, Time.deltaTime * sizeChangeSpeed);
            redBox.sizeDelta = new Vector2(redBoxWidth, redBox.rect.height);
        }
        else
        {
            //CalculateBoxLimits();
            //targetRedBoxWidth = Random.Range(whiteBox.sizeDelta.x * 0.3f, whiteBox.sizeDelta.x * 0.6f);
        }

        if (whiteBox.sizeDelta.x < targetWhiteBoxWidth-1 || whiteBox.sizeDelta.x > targetWhiteBoxWidth+1)
        {
            float whiteBoxWidth = Mathf.Lerp(whiteBox.sizeDelta.x, targetWhiteBoxWidth, Time.deltaTime * sizeChangeSpeed);
            whiteBox.sizeDelta = new Vector2(whiteBoxWidth, whiteBox.rect.height);
        }
        else
        {
            //CalculateBoxLimits();
            //targetWhiteBoxWidth = Random.Range(progressBar.GetComponent<RectTransform>().rect.width * 0.1f, progressBar.GetComponent<RectTransform>().rect.width * 0.60f);
            //targetWhiteBoxWidth = Random.Range(Mathf.Lerp(0,300,progressBar.value/100) * 0.1f, Mathf.Lerp(0, 300, progressBar.value / 100) * 0.6f);

        }

        // Smoothly change the size of the white box
    }
}
