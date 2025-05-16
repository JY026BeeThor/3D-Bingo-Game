using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Cardscontroller : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject cardPanel;

    [Header("Card Grids")]
    public GameObject card1Grid;
    public GameObject card2Grid;
    public GameObject card4Grid;
    public GameObject card6Grid;
    public GameObject card8Grid;
    public GameObject card12Grid;
    public GameObject card16Grid;

    [Header("Card Covers")]
    public GameObject[] card1Covers;
    public GameObject[] card2Covers;
    public GameObject[] card4Covers;
    public GameObject[] card6Covers;
    public GameObject[] card8Covers;
    public GameObject[] card12Covers;
    public GameObject[] card16Covers;

    [Header("Card Count Display")]
    public TextMeshProUGUI cardCountText;

    public int selectedCardsCount = 0;

    private Dictionary<int, GameObject> cardGrids;
    private Dictionary<int, GameObject[]> cardCovers;

    void Awake()
    {
        // Initialize the dictionaries for easy access
        cardGrids = new Dictionary<int, GameObject>()
        {
            { 1, card1Grid },
            { 2, card2Grid },
            { 4, card4Grid },
            { 6, card6Grid },
            { 8, card8Grid },
            { 12, card12Grid },
            { 16, card16Grid },
        };

        cardCovers = new Dictionary<int, GameObject[]>()
        {
            { 1, card1Covers },
            { 2, card2Covers },
            { 4, card4Covers },
            { 6, card6Covers },
            { 8, card8Covers },
            { 12, card12Covers },
            { 16, card16Covers },
        };
    }

    void Start()
    {
        ShowCards(4); // Default to 4 cards
    }

    public void OpenCardPanel()
    {
        cardPanel.SetActive(true);
    }

    public void ShowCards(int cardCount)
    {
        // Hide all grids
        foreach (var grid in cardGrids.Values)
            grid.SetActive(false);

        // Hide all covers
        SetAllCovers(true);

        // Show selected grid and set covers
        if (cardGrids.ContainsKey(cardCount) && cardCovers.ContainsKey(cardCount))
        {
            var grid = cardGrids[cardCount];
            var covers = cardCovers[cardCount];

            grid.SetActive(true);

            for (int i = 0; i < covers.Length; i++)
            {
                covers[i].SetActive(i >= cardCount); // Show cover only if index >= cardCount
            }
        }
        else
        {
            Debug.LogWarning($"Unsupported card count: {cardCount}");
        }

        // Update text
        selectedCardsCount = cardCount;
        if (cardCountText != null)
        {
            cardCountText.text = $" {cardCount}";
        }

        cardPanel.SetActive(false);
    }

    private void SetAllCovers(bool state)
    {
        foreach (var coverArray in cardCovers.Values)
        {
            foreach (var cover in coverArray)
            {
                cover.SetActive(state);
            }
        }
    }
}
