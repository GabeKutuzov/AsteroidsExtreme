using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextFade : MonoBehaviour {

    public float moveSpeed;
    public float colorTransitionSpeed;

    private float colorTransition;

    public Color startColor;
    public Color endColor;

    private Text scoreAddText;

    private void Start()
    {
        scoreAddText = GetComponent<Text>();
        Destroy(gameObject, 3f);
    }

    private void Update()
    {
        transform.Translate(Vector2.up * Time.deltaTime * moveSpeed);
    }

    private void FixedUpdate()
    {
        colorTransition += Time.deltaTime * colorTransitionSpeed;
        scoreAddText.color = Color.Lerp(startColor, endColor, Time.deltaTime * colorTransition);
    }
}