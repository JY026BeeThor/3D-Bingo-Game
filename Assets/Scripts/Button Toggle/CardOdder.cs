using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardOdder : MonoBehaviour
{
    public Button myButton;
    public TextMeshProUGUI numberText;
    public int numberToPrint = 5;

    public CardGridController gridController; // Assign this in Inspector

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (numberText != null)
        {
            numberText.text = numberToPrint.ToString();
        }

        // Trigger the grid update
        if (gridController != null)
        {
            gridController.UpdateFromText();
        }
    }
}
