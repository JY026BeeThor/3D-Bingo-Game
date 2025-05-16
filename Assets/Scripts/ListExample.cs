using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToggle : MonoBehaviour
{
    public List<Sprite> sprites;              // 2 sprites: Off and On
    public List<GameObject> objects;          // 10 objects to toggle
    public float toggleInterval = 0.5f;       // Blinking speed in seconds

    void Start()
    {
        if (sprites.Count < 2)
        {
            Debug.LogError("Please assign 2 sprites in the Inspector.");
            return;
        }

        foreach (GameObject obj in objects)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = sprites[0]; // Start with Off sprite
                StartCoroutine(ToggleSprite(sr));
            }
        }
    }

    IEnumerator ToggleSprite(SpriteRenderer sr)
    {
        int currentIndex = 0;
        while (true)
        {
            yield return new WaitForSeconds(toggleInterval);
            currentIndex = 1 - currentIndex; // Toggle index between 0 and 1
            sr.sprite = sprites[currentIndex];
        }
    }
}
