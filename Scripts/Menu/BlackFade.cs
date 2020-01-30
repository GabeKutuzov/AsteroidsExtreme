using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlackFade : MonoBehaviour {

    public float fadeSpeed = 5f;

    public Color colorA;
    public Color colorB;

    private Image blackImage;

    private void Start()
    {
        blackImage = GetComponent<Image>();
        blackImage.color = colorA;
    }

    private void FixedUpdate()
    {
        blackImage.color = Color.Lerp(blackImage.color, colorB, Time.deltaTime * fadeSpeed);
    }

}
