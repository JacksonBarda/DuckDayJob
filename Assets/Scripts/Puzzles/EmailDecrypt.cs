using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EmailDecrypt : Interactable
{
    public GameObject puzzleBarUI;
    public List<Image> fragments;
    public List<Image> outlines;
    public float rotationSpeed = 75f;
    public float snapThreshold = 0.1f;
    public float snapThresholdRot = 5.0f;
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float sizeChangeSpeed = 5f;
    public float rotDistance;
    private Vector2 targetRedBoxPosition;
    private Vector2 targetWhiteBoxPosition;
    private float targetRedBoxWidth;
    private float targetWhiteBoxWidth;
    [SerializeField]
    private Slider SLDR_Progress;

    [HideInInspector]
    public Image selectedFragment;

    public Image progressBar; 
    public RectTransform redBox; 
    public RectTransform whiteBox; 

    public float decryptionSpeed = 0.5f;
    private bool partOneComplete = false;

    private int count = 0;

    private void Update()
    {
        HandleInput();
        if (partOneComplete)
        {
            HandleMouseInteraction();
            UpdateBoxMovements();
        }
        
    }

    private void HandleInput()
    {
        if (selectedFragment != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                selectedFragment.rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                selectedFragment.rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            }
        }
    }

    public void CheckSnap(Image fragment)
    {

        int index = fragments.IndexOf(fragment);
        float distance = Vector3.Distance(fragment.rectTransform.position, outlines[index].rectTransform.position);
        rotDistance = Mathf.Abs((fragment.rectTransform.rotation.eulerAngles.z - outlines[index].rectTransform.rotation.eulerAngles.z));
        if (distance <= snapThreshold && (360f - snapThresholdRot) <= rotDistance || rotDistance <= snapThresholdRot)
        {
            fragment.rectTransform.position = outlines[index].rectTransform.position;
            fragment.rectTransform.rotation = outlines[index].rectTransform.rotation;
            count++;
            AudioManager.Instance.PlaySFX("SFX_VendingButton");
        }
        else
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        if (count >= fragments.Count)
        {
            //Complete();
            puzzleBarUI.SetActive(true);
            partOneComplete = true;

        }
    }
    private void Start()
    {
        targetWhiteBoxPosition = new Vector2(Random.Range(progressBar.GetComponent<RectTransform>().rect.xMin, progressBar.GetComponent<RectTransform>().rect.xMax), whiteBox.rect.y - 123f);
        targetRedBoxPosition = new Vector2(Random.Range(whiteBox.rect.xMin, whiteBox.rect.xMax), redBox.rect.position.y +50);
        targetWhiteBoxWidth = Random.Range(progressBar.GetComponent<RectTransform>().rect.width * 0.1f, progressBar.GetComponent<RectTransform>().rect.width * 0.30f);
        targetRedBoxWidth = Random.Range(whiteBox.rect.width * 0.4f, whiteBox.rect.width * 1.0f);
    }
    
    public override void Interact()
    {
        
        PlayerMove.puzzleMode = true;
        puzzleUI.SetActive(true);
        mainUI.SetActive(false);
        //puzzleBarUI.SetActive(false);
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        progressBar.fillAmount = 0;
    }

    public override void Action()
    {
        throw new System.NotImplementedException();
    }

    public override void Complete()
    {
        base.Complete();

        count = 0;
        AudioManager.Instance.PlaySFX("SFX_Complete");
        foreach (Image fragment in fragments)
        {
            fragment.rectTransform.position = fragment.GetComponent<FragmentDragger>().originalPosition;
        }
        SLDR_Progress.value++;
    }
    private void HandleMouseInteraction()
    {
        // Check if the mouse is within the red box
        if (RectTransformUtility.RectangleContainsScreenPoint(redBox, Input.mousePosition))
        {
            // Update the progress bar based on decryption speed
            progressBar.fillAmount += Time.deltaTime * decryptionSpeed;

            // Update the position and size of the red and white boxes periodically
            
        }
        else
        {
            // Reset progress if the mouse is outside the red box
            progressBar.fillAmount = 0f;
        }

        // Check if the progress bar is filled
        if (progressBar.fillAmount >= 1.0f)
        {
            Complete();
        }
    }
    void UpdateBoxMovements()
    {
        // Check if the red box has reached its target position
        if ((Vector2)redBox.anchoredPosition != targetRedBoxPosition)
        {
            // Smoothly move the red box towards the target position
            redBox.anchoredPosition = Vector2.MoveTowards(redBox.anchoredPosition, targetRedBoxPosition, Time.deltaTime * movementSpeed);
        }
        else
        {
            // Red box reached the target, pick a new target position
            targetRedBoxPosition = new Vector2(Random.Range(whiteBox.rect.xMin, whiteBox.rect.xMax), redBox.rect.position.y + 50);
            targetRedBoxWidth = Random.Range(whiteBox.rect.width * 0.4f, whiteBox.rect.width * 1.0f);
        }
        if(redBox.rect.width !> targetRedBoxWidth-1 || redBox.rect.width !> targetRedBoxWidth + 1)
        {
            float redBoxWidth = Mathf.Lerp(redBox.rect.width, targetRedBoxWidth, Time.deltaTime * sizeChangeSpeed);
            redBox.sizeDelta = new Vector2(redBoxWidth, redBox.rect.height);
        }
        else
        {
            targetRedBoxWidth = Random.Range(whiteBox.rect.width * 0.3f, whiteBox.rect.width * 1.0f);
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
            targetWhiteBoxPosition = new Vector2(Random.Range(progressBar.GetComponent<RectTransform>().rect.xMin, progressBar.GetComponent<RectTransform>().rect.xMax), whiteBox.rect.y - 123f);
        }
        /*
        if (whiteBox.rect.width !> targetWhiteBoxWidth-1 || whiteBox.rect.width !< targetWhiteBoxWidth + 1)
        {
            //float whiteBoxWidth = Mathf.Lerp(whiteBox.rect.width, targetWhiteBoxWidth, Time.deltaTime * sizeChangeSpeed);
            //whiteBox.sizeDelta = new Vector2(whiteBoxWidth, whiteBox.rect.height);
        }
        else
        {
            targetWhiteBoxWidth = Random.Range(progressBar.GetComponent<RectTransform>().rect.width * 0.1f, progressBar.GetComponent<RectTransform>().rect.width * 0.60f);
        }
        */
        // Smoothly change the size of the white box
       

    }
}
