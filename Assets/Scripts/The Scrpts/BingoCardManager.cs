using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class BingoCardManager : MonoBehaviour
{
    public List<Transform> allCards; // Assign all 26 cards in Inspector
    public Button changeButton;

    private int numberRange = 90;
    private int numbersPerCard = 15;

    private List<List<int>> uniqueNumberSets = new List<List<int>>();

    void Start()
    {
        changeButton.onClick.AddListener(GenerateCards);
        GenerateCards();
    }

    public void GenerateCards()
    {
        uniqueNumberSets.Clear();

        // Step 1: Generate 16 unique number sets
        for (int i = 0; i < 16; i++)
        {
            List<int> numbers = GenerateUniqueNumbers(numbersPerCard, numberRange);
            uniqueNumberSets.Add(numbers);
        }

        // Step 2: Assign numbers to 26 cards
        for (int i = 0; i < allCards.Count; i++)
        {
            List<int> cardNumbers;

            if (i < 4)
            {
                // Card 0–3 → random
                cardNumbers = uniqueNumberSets[i];
            }
            else if (i >= 4 && i < 8)
            {
                // Card 4–7 → copy from 0–3
                cardNumbers = new List<int>(uniqueNumberSets[i - 4]);
            }
            else if (i >= 8 && i < 10)
            {
                // Card 8–9 → new random
                cardNumbers = uniqueNumberSets[i - 4]; // i=8 → index 4, i=9 → index 5
            }
            else if (i >= 10 && i < 20)
            {
                // Card 10–19 → copy from 0–9
                cardNumbers = new List<int>(uniqueNumberSets[i - 10]);
            }
            else
            {
                // Card 20–25 → new random
                cardNumbers = uniqueNumberSets[i - 10]; // i=20 → index 10, ..., i=25 → index 15
            }

            AssignNumbersToCard(allCards[i], cardNumbers, i + 1);
        }
    }

    List<int> GenerateUniqueNumbers(int count, int max)
    {
        List<int> pool = new List<int>();
        for (int i = 1; i <= max; i++)
            pool.Add(i);

        List<int> result = new List<int>();
        while (result.Count < count && pool.Count > 0)
        {
            int index = Random.Range(0, pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result;
    }

    void AssignNumbersToCard(Transform card, List<int> numbers, int cardIndex)
    {
        int numberSlotIndex = 0;

        foreach (Transform child in card)
        {
            TMP_Text txt = child.GetComponentInChildren<TMP_Text>();
            if (txt != null && numberSlotIndex < numbers.Count)
            {
                txt.text = numbers[numberSlotIndex].ToString();
                txt.color = Color.black; // Reset color
                numberSlotIndex++;
            }
        }

        // Set card label
        Transform labelObj = card.Find("CardLabel");
        if (labelObj != null)
        {
            TMP_Text label = labelObj.GetComponent<TMP_Text>();
            if (label != null)
                label.text = "Card " + cardIndex;
        }
    }

    // ✅ Highlight matched numbers
    public void MarkDrawnNumbers(List<int> drawnNumbers)
    {
        foreach (Transform card in allCards)
        {
            foreach (Transform child in card)
            {
                TMP_Text txt = child.GetComponentInChildren<TMP_Text>();
                Image img = child.GetComponent<Image>();

                if (txt != null && int.TryParse(txt.text, out int number))
                {
                    if (drawnNumbers.Contains(number))
                    {
                        txt.color = Color.green;

                        if (img != null)
                            StartCoroutine(BackgroundPopEffect(img)); // ✅ Add animated pop
                    }
                }
            }
        }
    }



    // 🔁 Reset markings
    public void ResetCardMarks()
    {
        foreach (Transform card in allCards)
        {
            foreach (Transform child in card)
            {
                TMP_Text txt = child.GetComponentInChildren<TMP_Text>();
                Image img = child.GetComponent<Image>();

                if (txt != null)
                    txt.color = Color.black;

                if (img != null)
                    img.color = new Color(1f, 1f, 1f, 0f); ; //default background color   
            }
        }
    }
    IEnumerator BackgroundPopEffect(Image img)
    {
        Color brightGreen = new Color(0.6f, 1f, 0.6f); // bright green
        Color dark = new Color(0.1f, 0.1f, 0.1f); // dark gray (near black)

        for (int i = 0; i < 3; i++)
        {
            img.color = brightGreen;
            yield return new WaitForSeconds(0.1f);
            img.color = dark;
            yield return new WaitForSeconds(0.1f);
        }

        img.color = dark; // Ensure final color stays dark green
    }

}