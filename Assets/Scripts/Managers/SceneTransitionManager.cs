using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private float fadeDuration = 0.5f;

    bool isTransitioning;

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
            return;
        }

        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;
    }

    public void FadeToScene(string sceneName)
    {
        if (isTransitioning)
            return;

        StartCoroutine(FadeRoutine(sceneName));
    }

    IEnumerator FadeRoutine(string sceneName)
    {
        isTransitioning = true;

        // Fade Out
        yield return StartCoroutine(
            Fade(0f, 1f)
        );

        // Load Scene
        SceneManager.LoadScene(sceneName);

        // Tunggu 1 frame setelah scene selesai load
        yield return null;


        // Paksa tetap hitam
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        // Sedikit jeda supaya tidak terasa blink
        yield return new WaitForSeconds(.1f);

        // Fade In
        yield return StartCoroutine(
            Fade(1f, 0f)
        );

        isTransitioning = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;

        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            color.a =
                Mathf.Lerp(
                    startAlpha,
                    endAlpha,
                    t
                );

            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }
}