using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip mainMenuBGM;
    public AudioClip gameplayBGM;
    public AudioClip buttonClick;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMainMenuBGM()
    {
        if (bgmSource.clip == mainMenuBGM)
            return;

        bgmSource.clip = mainMenuBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayGameplayBGM()
    {
        if (bgmSource.clip == gameplayBGM)
            return;

        bgmSource.clip = gameplayBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}