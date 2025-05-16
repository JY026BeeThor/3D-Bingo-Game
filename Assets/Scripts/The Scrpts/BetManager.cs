using UnityEngine;
using TMPro;

public class BetManager : MonoBehaviour
{
    public float totalBalance = 200f;  // Player's balance
    public float selectedBetAmount = 1f;  // Amount bet per card
    public float totalBetAmount = 0f;    // Total bet (selectedBetAmount * selectedCardsCount)

    // Reference to CardManager to access selectedCardsCount
    public CardManager cardManager;

    // UI Elements
    public TextMeshProUGUI balanceText;  // Displays total balance
    public TextMeshProUGUI betAmountText; // Displays selected bet amount per card
    public TextMeshProUGUI totalBetText; // Displays total bet (betAmount * cards)
    public GameObject betGridPanel;


    // Method to update the total bet based on selected bet amount and number of cards
    public void UpdateTotalBet()
    {
        // Get the number of selected cards from the CardManager
        int selectedCardsCount = cardManager.selectedCardsCount;

        // Calculate the total bet amount (Bet Amount * Selected Cards Count)
        totalBetAmount = selectedBetAmount * selectedCardsCount;

        // Update the UI
        totalBetText.text = $" ${totalBetAmount}";  // Update the total bet text
    }

    // Method to handle bet amount selection (per card)
    public void OnBetAmountSelected(float betAmount)
    {
        selectedBetAmount = betAmount;
        betAmountText.text = $" ${selectedBetAmount}";  // Update the bet amount text
        UpdateTotalBet();  // Update total bet when bet amount changes
    }

    // Method to open the bet panel
    public void OnBetButtonClick()
    {
        // Toggle the bet grid panel active state
        betGridPanel.SetActive(!betGridPanel.activeSelf);

        // Update the balance display only if the panel is shown
        if (betGridPanel.activeSelf)
        {
            UpdateBalanceDisplay();
        }
    }

    // Method to update the balance display
    private void UpdateBalanceDisplay()
    {
        balanceText.text = $" ${totalBalance}";
    }
}