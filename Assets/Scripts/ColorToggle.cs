using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorToggle : MonoBehaviour
{
    public Image[] circleImages; // Assign these in Inspector
    public Color highlightColor = Color.green;
    private Color[] originalColors;

    public float toggleDuration = 1.5f; // Seconds to wait before reverting

    void Start()
    {
        // Store original colors at start
        originalColors = new Color[circleImages.Length];
        for (int i = 0; i < circleImages.Length; i++)
        {
            originalColors[i] = circleImages[i].color;
        }
    }

    // Call this function to trigger toggle
    public void ToggleColor(int index)
    {
        if (index >= 0 && index < circleImages.Length)
        {
            StartCoroutine(ColorToggleRoutine(index));
        }
    }

    private IEnumerator ColorToggleRoutine(int index)
    {
        circleImages[index].color = highlightColor;

        yield return new WaitForSeconds(toggleDuration);

        circleImages[index].color = originalColors[index];
    }
}
