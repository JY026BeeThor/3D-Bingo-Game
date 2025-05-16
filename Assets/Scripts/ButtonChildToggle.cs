using UnityEngine;
using UnityEngine.UI;

public class ButtonChildToggle : MonoBehaviour
{
    public Button targetButton;
    public GameObject object1;
    public GameObject object2;

    private bool isFirstClick = true;

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        if (isFirstClick)
        {
            if (object1 != null) object1.SetActive(false);
            if (object2 != null) object2.SetActive(true);
            Debug.Log("-1");
        }
        else
        {
            if (object1 != null) object1.SetActive(true);
            if (object2 != null) object2.SetActive(false);
            Debug.Log("+1");
        }

        isFirstClick = !isFirstClick; // Toggle the state
    }
}