using UnityEngine;
using TMPro;

public class ButtonTMPTextLogger : MonoBehaviour
{
    public TMP_Text buttonText;

    public void ReadButtonText(int number)
    {
        Debug.Log("Button Text: " + number);
    }
}
