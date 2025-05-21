using UnityEngine;
using UnityEngine.UI;

public class ToggleImagePanel : MonoBehaviour
{
    public Button toggleButton;     // Your UI Button
    public GameObject imagePanel;   // Your panel GameObject with the Image

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(TogglePanel);
        }
    }

    public void TogglePanel()
    {
        if (imagePanel != null)
        {
            imagePanel.SetActive(!imagePanel.activeSelf);  // Toggle visibility
        }
    }
}
