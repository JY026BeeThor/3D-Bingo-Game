using UnityEngine;

public class CardButtonHandler : MonoBehaviour
{
    public GameObject cardBoardPanel;

    public void ToggleCardBoard()
    {
        if (cardBoardPanel != null)
        {
            bool isActive = cardBoardPanel.activeSelf;
            cardBoardPanel.SetActive(!isActive);
        }
    }
}