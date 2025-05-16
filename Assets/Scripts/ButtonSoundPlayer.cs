using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioSource buttonAudioSource;

    // Call this method from the Button's OnClick event
    public void PlaySound()
    {
        if (buttonAudioSource != null && buttonAudioSource.clip != null)
        {
            buttonAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("Button AudioSource or Clip is not assigned.");
        }
    }
}
