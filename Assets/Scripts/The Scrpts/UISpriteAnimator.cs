using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimator : MonoBehaviour
{
    public Image targetImage;              // UI Image component to animate
    public Sprite[] animationSprites;      // Array of 3 sprites
    public float frameRate = 1.0f;         // Time between frames in seconds

    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();
    }

    void Update()
    {
        if (animationSprites.Length == 0 || targetImage == null)
            return;

        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer -= frameRate;

            // Change to the next sprite
            currentFrame = (currentFrame + 1) % animationSprites.Length;
            Sprite currentSprite = animationSprites[currentFrame];
            targetImage.sprite = currentSprite;

            // Print sprite name
            Debug.Log("Current Sprite: " + currentSprite.name);
        }
    }
}
