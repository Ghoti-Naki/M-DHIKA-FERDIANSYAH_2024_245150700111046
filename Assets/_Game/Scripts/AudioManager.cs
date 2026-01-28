using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Clips")]
    [SerializeField] private AudioClip moveClip;
    [SerializeField] private AudioClip pushClip;
    [SerializeField] private AudioClip winClip;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlayMove()
    {
        if (moveClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(moveClip);
        }
    }

    public void PlayPush()
    {
        if (pushClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(pushClip);
        }
    }

    public void PlayWin()
    {
        if (winClip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(winClip);
        }
    }
}