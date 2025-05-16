using UnityEngine;
using UnityEngine.UI;

public class AssignPrefabSpriteToImage : MonoBehaviour
{
    public GameObject prefab; // assign in inspector
    public Image targetImage; // assign in inspector

    void Start()
    {
        if (prefab != null && targetImage != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                targetImage.sprite = sr.sprite;
            }
            else
            {
                Debug.LogWarning("Prefab does not have a SpriteRenderer.");
            }
        }
    }
}

