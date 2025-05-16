using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [System.Serializable]
    public class CardSlot
    {
        public Image leftDigit;
        public Image rightDigit;
    }

    [System.Serializable]
    public class Card
    {
        public GameObject cardObject;
        public CardSlot[] slots = new CardSlot[15];
        public bool isSelected = false;
    }

    public List<Card> allCards = new List<Card>();
    public Sprite[] digitSprites;
    public GameObject cardPrefab;
    public Transform cardParent;

    private void Start()
    {
        CreateCards(5); // Create 5 cards by default
    }

    public void CreateCards(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            Card card = new Card
            {
                cardObject = newCard,
                slots = new CardSlot[15],
                isSelected = false
            };

            Image[] digitImages = newCard.GetComponentsInChildren<Image>();
            int digitIndex = 0;

            for (int j = 0; j < 15; j++)
            {
                card.slots[j] = new CardSlot();

                if (digitIndex < digitImages.Length)
                    card.slots[j].leftDigit = digitImages[digitIndex++];

                if (digitIndex < digitImages.Length)
                    card.slots[j].rightDigit = digitImages[digitIndex++];
            }

            allCards.Add(card);
        }
    }

    public void UpdateCardVisual(Card card, List<int> numbers)
    {
        for (int i = 0; i < card.slots.Length && i < numbers.Count; i++)
        {
            int number = numbers[i];
            int leftDigit = number / 10;
            int rightDigit = number % 10;

            if (leftDigit >= 0 && leftDigit < digitSprites.Length)
                card.slots[i].leftDigit.sprite = digitSprites[leftDigit];

            if (rightDigit >= 0 && rightDigit < digitSprites.Length)
                card.slots[i].rightDigit.sprite = digitSprites[rightDigit];
        }
    }

    public void MarkDrawnNumbers(List<int> drawnNumbers)
    {
        foreach (Card card in allCards)
        {
            if (!card.isSelected) continue;

            for (int i = 0; i < card.slots.Length; i++)
            {
                int leftDigitValue = GetDigitValue(card.slots[i].leftDigit.sprite);
                int rightDigitValue = GetDigitValue(card.slots[i].rightDigit.sprite);

                int number = leftDigitValue * 10 + rightDigitValue;
                bool isDrawn = drawnNumbers.Contains(number);

                card.slots[i].leftDigit.color = isDrawn ? Color.green : Color.white;
                card.slots[i].rightDigit.color = isDrawn ? Color.green : Color.white;
            }
        }
    }

    public void ResetCardMarks()
    {
        foreach (Card card in allCards)
        {
            for (int i = 0; i < card.slots.Length; i++)
            {
                card.slots[i].leftDigit.color = Color.white;
                card.slots[i].rightDigit.color = Color.white;
            }
        }
    }

    private int GetDigitValue(Sprite digitSprite)
    {
        for (int i = 0; i < digitSprites.Length; i++)
        {
            if (digitSprites[i] == digitSprite)
                return i;
        }
        return -1;
    }

    // ✅ Public property to get the number of selected cards
    public int selectedCardsCount
    {
        get
        {
            int count = 0;
            foreach (Card card in allCards)
            {
                if (card.isSelected)
                    count++;
            }
            return count;
        }
    }
}
