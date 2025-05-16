using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BingoCardTMPGenerator : MonoBehaviour
{
    public GameObject[] cardPanels; // Assign 16 card panels (each with 15 TMP_Text children)

    void Start()
    {
        GenerateAllCards();
    }

    void GenerateAllCards()
    {
        foreach (GameObject panel in cardPanels)
        {
            GenerateCard(panel);
        }
    }

    void GenerateCard(GameObject panel)
    {
        TMP_Text[] numberTexts = panel.GetComponentsInChildren<TMP_Text>();

        HashSet<int> usedNumbers = new HashSet<int>();
        int[] cardNumbers = new int[15];
        int count = 0;

        while (count < 15)
        {
            int randNum = Random.Range(0, 100); // 0–99
            if (!usedNumbers.Contains(randNum))
            {
                usedNumbers.Add(randNum);
                cardNumbers[count] = randNum;
                count++;
            }
        }

        for (int i = 0; i < numberTexts.Length && i < 15; i++)
        {
            numberTexts[i].text = cardNumbers[i].ToString("D2");
        }
    }
}
