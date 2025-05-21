using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardGridController : MonoBehaviour
{
    [System.Serializable]
    public class GridInfo
    {
        public int cardCount;
        public GameObject gridObject;
        public List<GameObject> cards;
    }

    [Header("Grids Setup")]
    public List<GridInfo> grids = new List<GridInfo>();

    [Header("UI Reference")]
    public TMP_Text oddEvenText; // This should contain just a number like: "6"

    void Start()
    {
        UpdateFromText(); // On start, show cards based on display text
    }

    public void UpdateFromText()
    {
        if (oddEvenText == null)
        {
            Debug.LogWarning("OddEvenText not assigned.");
            return;
        }

        string content = oddEvenText.text.Trim();

        if (int.TryParse(content, out int cardCount))
        {
            ApplyGridDisplay(cardCount);
        }
        else
        {
            Debug.LogWarning("Invalid number in oddEvenText: " + content);
        }
    }

    void ApplyGridDisplay(int cardCount)
    {
        // Hide all grids first
        foreach (var grid in grids)
        {
            grid.gridObject.SetActive(false);
        }

        // Find the smallest grid that can support the requested cards
        GridInfo bestFit = null;
        foreach (var grid in grids)
        {
            if (grid.cardCount >= cardCount)
            {
                if (bestFit == null || grid.cardCount < bestFit.cardCount)
                    bestFit = grid;
            }
        }

        if (bestFit == null)
        {
            Debug.LogWarning("No grid can support " + cardCount + " cards.");
            return;
        }

        bestFit.gridObject.SetActive(true);

        for (int i = 0; i < bestFit.cards.Count; i++)
        {
            bestFit.cards[i].SetActive(i < cardCount);
        }
    }

    // Optional: Call this method from UI button
    public void OnUpdateButtonClick()
    {
        UpdateFromText();
    }
}
