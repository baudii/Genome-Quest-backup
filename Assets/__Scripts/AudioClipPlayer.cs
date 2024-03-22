using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    public void Play()
    {
        if (audioSource.enabled)
            audioSource.PlayOneShot(audioClip);
    }
}
