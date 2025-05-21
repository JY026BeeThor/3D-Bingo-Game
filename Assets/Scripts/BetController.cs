using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BetController : MonoBehaviour
{
    public Button myButton;                // Assign your button in the Inspector
    public TextMeshProUGUI numberText;     // Assign your TMP Text object in the Inspector
    public int numberToPrint = 5;          // Change this to any number you want to display

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
            numberText.text = $"${numberToPrint:N0}"; // Formats with commas, like $1,000
        }
    }
}
