using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueTyper : MonoBehaviour
{
    [SerializeField]
    TMP_Text dialogueText;

    [SerializeField]
    float typingSpeed = 0.03f;

    [SerializeField]
    GameObject continueIndicator;

    Coroutine typingRoutine;
    Coroutine blinkRoutine;

    bool isTyping;

    string fullText;

    public void StartTyping(
        string text
    )
    {
        continueIndicator.SetActive(false);

        if (blinkRoutine != null)
        {
            StopCoroutine(
                blinkRoutine
            );
        }

        fullText = text;

        if (typingRoutine != null)
        {
            StopCoroutine(
                typingRoutine
            );
        }

        typingRoutine =
            StartCoroutine(
                TypeRoutine()
            );
    }

    public bool TryCompleteTyping()
    {
        if (!isTyping)
        {
            return false;
        }

        StopCoroutine(
            typingRoutine
        );

        dialogueText.text =
            fullText;

        continueIndicator.SetActive(true);

        StartBlink();

        isTyping = false;

        return true;
    }

    // void Update()
    // {
    //     continueIndicator.SetActive(!continueIndicator.activeSelf);
    // }
    void StartBlink()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(
                blinkRoutine
            );
        }

        blinkRoutine =
            StartCoroutine(
                BlinkRoutine()
            );
    }

    IEnumerator TypeRoutine()
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char c in fullText)
        {
            dialogueText.text += c;

            yield return new WaitForSeconds(
                typingSpeed
            );
        }

        isTyping = false;

        continueIndicator.SetActive(true);

        StartBlink();
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            continueIndicator.SetActive(
                !continueIndicator.activeSelf
            );

            yield return new WaitForSeconds(
                0.5f
            );
        }
    }
}