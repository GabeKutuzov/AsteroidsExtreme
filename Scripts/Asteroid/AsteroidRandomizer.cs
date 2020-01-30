using UnityEngine;
using System.Collections;

public class AsteroidRandomizer : MonoBehaviour {

    public Sprite[] asteroidSprites;

    public float minScale = 0.3f;
    public float maxScale = 2f;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        RandomizeAsteroid();
    }

    private void RandomizeAsteroid()
    {
        #region SpriteRandomizer
        spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];
        #endregion SpriteRandomizer

        #region AsteroidScaleRandomizer

        float newScaleAmount = Random.Range(minScale, maxScale);
        Vector3 newScale = new Vector3(newScaleAmount, newScaleAmount, newScaleAmount);
        transform.localScale = newScale;

        #endregion AsteroidScaleRandomizer

        #region AsteroidFlipRandomizer

        int spriteFlipX = Random.Range(0, 2);
        int spriteFlipY = Random.Range(0, 2);

        if (spriteFlipX == 1)
        {
            spriteRenderer.flipX = true;
        }
        else if(spriteFlipX == 0)
        {
            spriteRenderer.flipX = false;
        }

        if(spriteFlipY == 1)
        {
            spriteRenderer.flipY = true;
        }
        else if(spriteFlipY == 0)
        {
            spriteRenderer.flipY = false;
        }

        #endregion AsteroidFlipRandomizer
    }
}