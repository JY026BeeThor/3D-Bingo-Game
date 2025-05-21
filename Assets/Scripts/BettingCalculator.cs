using UnityEngine;
using TMPro;

public class BettingCalculator : MonoBehaviour
{
    public TextMeshProUGUI betText;           // e.g., "$1"
    public TextMeshProUGUI cardsOdderText;    // e.g., "4"
    public TextMeshProUGUI totalBetText;      // e.g., "$4"
    public TextMeshProUGUI balanceText;       // e.g., "$1996"

    private string previousBetValue = "";
    private string previousCardValue = "";

    void Update()
    {
        if (betText.text != previousBetValue || cardsOdderText.text != previousCardValue)
        {
            CalculateBetAndBalance();

            previousBetValue = betText.text;
            previousCardValue = cardsOdderText.text;
        }
    }

    void CalculateBetAndBalance()
    {
        Debug.Log("=== Calculating Bet and Balance ===");
        Debug.Log("Raw Bet Text: " + betText.text);
        Debug.Log("Raw Card Text: " + cardsOdderText.text);
        Debug.Log("Raw Balance Text: " + balanceText.text);

        // Try to parse all values safely
        if (float.TryParse(betText.text.Replace("$", ""), out float betAmount) &&
            int.TryParse(cardsOdderText.text, out int cardCount) &&
            float.TryParse(balanceText.text.Replace("$", ""), out float currentBalance))
        {
            Debug.Log("Parsed Bet Amount: " + betAmount);
            Debug.Log("Parsed Card Count: " + cardCount);
            Debug.Log("Parsed Current Balance: " + currentBalance);

            float totalBet = betAmount * cardCount;
            float updatedBalance = currentBalance - totalBet;

            totalBetText.text = "$" + totalBet.ToString("F0");
            balanceText.text = "$" + updatedBalance.ToString("F0");

            Debug.Log("Calculated Total Bet: $" + totalBet);
            Debug.Log("Updated Balance: $" + updatedBalance);
        }
        else
        {
            Debug.LogWarning("Parsing failed. Make sure all text values are valid numeric strings.");
        }
    }
}
