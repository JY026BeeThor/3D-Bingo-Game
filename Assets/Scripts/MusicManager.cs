using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSource;

    void Start()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }
}