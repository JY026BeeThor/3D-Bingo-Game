using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardComponent : MonoBehaviour
{
    public int SlotIndex;
    public int FullNumber; // 0 to 99
    public int LeftDigit => FullNumber / 10;
    public int RightDigit => FullNumber % 10;

    public CardComponent(int index, int number)
    {
        SlotIndex = index;
        FullNumber = number;
    }
}