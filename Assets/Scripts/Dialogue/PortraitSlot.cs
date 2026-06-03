using UnityEngine;
using UnityEngine.UI;

public class PortraitSlot : MonoBehaviour
{
    public Image portraitImage;

    public void SetPortrait(
        Sprite sprite
    )
    {
        portraitImage.sprite =
            sprite;

        gameObject.SetActive(
            sprite != null
        );
    }

    public void Highlight()
    {
        portraitImage.color =
            Color.white;
    }

    public void Unhighlight()
    {
        portraitImage.color =
            new Color(
                0.5f,
                0.5f,
                0.5f,
                1f
            );
    }
}