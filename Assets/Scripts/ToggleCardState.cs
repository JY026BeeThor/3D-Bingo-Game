using UnityEngine;
using UnityEngine.UI;

public class ToggleCardState : MonoBehaviour
{
    public GameObject card;             // Card GameObject with Image
    public GameObject children;         // Children GameObject to toggle
    public Sprite newSprite;            // Sprite to show on first press

    private Sprite originalSprite;      // Original sprite to revert to
    private bool isFirstPress = true;   // Toggle state
    private Image cardImage;            // Image component of card

    void Start()
    {
        if (card != null)
            cardImage = card.GetComponent<Image>();

        if (cardImage != null)
            originalSprite = cardImage.sprite;
    }

    public void OnButtonClick()
    {
        if (isFirstPress)
        {
            // First press: Show sprite, hide children
            if (cardImage != null && newSprite != null)
                cardImage.sprite = newSprite;

            if (children != null)
                children.SetActive(false);

            Debug.Log("-1");
        }
        else
        {
            // Second press: Hide sprite, show children
            if (cardImage != null)
                cardImage.sprite = originalSprite;

            if (children != null)
                children.SetActive(true);

            Debug.Log("+1");
        }

        isFirstPress = !isFirstPress;
    }
}
