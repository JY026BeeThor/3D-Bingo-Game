using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackgroundSlideDown : MonoBehaviour
{
    public Slider slider;                 // Assign your UI Slider here
    public string nextSceneName = "Game"; // Scene to load
    public float slideDuration = 30f;     // Time to go from full to empty

    private float elapsedTime = 0f;

    void Start()
    {
        if (slider != null)
            slider.value = 1f; // Start full
    }

    void Update()
    {
        if (slider == null) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / slideDuration);

        slider.value = 1f - t; // Decrease value over time

        if (slider.value <= 0f)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}