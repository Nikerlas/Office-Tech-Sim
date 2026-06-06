using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PortraitSlot : MonoBehaviour
{
    Coroutine currentAnimation;
    Coroutine colorRoutine;

    public Image portraitImage;

    void Awake()
    {
        portraitImage.sprite = null;

        gameObject.SetActive(false);
    }

    public void SetPortrait(Sprite sprite)
    {
        portraitImage.sprite = sprite;

        gameObject.SetActive(
            sprite != null
        );
    }

    public void Highlight()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        FadeToColor(
            Color.white
        );
    }

    public void Unhighlight()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        FadeToColor(
            new Color(
                0.6f,
                0.6f,
                0.6f,
                1f
            )
        );
    }

    public void ClearPortrait()
    {
        portraitImage.sprite = null;

        gameObject.SetActive(false);
    }

    public bool HasPortrait()
    {
        return portraitImage.sprite != null;
    }

    public void PlaySpawnAnimation()
    {
        if (currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
        }

        currentAnimation =
            StartCoroutine(
                SpawnAnimation()
            );
    }

    void FadeToColor(Color target)
    {
        if (colorRoutine != null)
        {
            StopCoroutine(
                colorRoutine
            );
        }

        colorRoutine =
            StartCoroutine(
                FadeRoutine(target)
            );
    }

    IEnumerator SpawnAnimation()
    {
        transform.localScale =
            Vector3.one * 0.9f;

        float timer = 0f;

        while (timer < 0.08f)
        {
            timer += Time.deltaTime;

            transform.localScale =
                Vector3.Lerp(
                    Vector3.one * 0.9f,
                    Vector3.one * 1.05f,
                    timer / 0.08f
                );

            yield return null;
        }

        timer = 0f;

        while (timer < 0.08f)
        {
            timer += Time.deltaTime;

            transform.localScale =
                Vector3.Lerp(
                    Vector3.one * 1.05f,
                    Vector3.one,
                    timer / 0.08f
                );

            yield return null;
        }

        transform.localScale =
            Vector3.one;
    }

    IEnumerator FadeRoutine(Color target)
    {
        Color start =
            portraitImage.color;

        float timer = 0f;

        while (timer < 0.15f)
        {
            timer += Time.deltaTime;

            portraitImage.color =
                Color.Lerp(
                    start,
                    target,
                    timer / 0.15f
                );

            yield return null;
        }

        portraitImage.color =
            target;
    }
}