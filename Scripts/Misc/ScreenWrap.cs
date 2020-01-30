using UnityEngine;
using System.Collections;

public class ScreenWrap : MonoBehaviour {

    public bool wrapWidth = true;
    public bool wrapHeight = true;

    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector2 viewportPosition;
    private bool isWrappingWidth;
    private bool isWrappingHeight;
    private Vector2 newPosition;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        viewportPosition = Vector2.zero;
        isWrappingWidth = false;
        isWrappingHeight = false;
        newPosition = transform.position;
    }

    private void LateUpdate()
    {
        Wrap();
    }

    private void Wrap()
    {
        bool isVisible = IsBeingRendered();

        if(isVisible == true)
        {
            isWrappingWidth = false;
            isWrappingHeight = false;
        }

        newPosition = transform.position;
        viewportPosition = mainCamera.WorldToViewportPoint(newPosition);

        if(wrapWidth == true)
        {
            if(isWrappingWidth == false)
            {
                if(viewportPosition.x > 1)
                {
                    newPosition.x = mainCamera.ViewportToWorldPoint(Vector2.zero).x;
                    isWrappingWidth = true;
                }
                else if(viewportPosition.x < 0)
                {
                    newPosition.x = mainCamera.ViewportToWorldPoint(Vector2.one).x;
                    isWrappingWidth = true;
                }
            }
        }

        if(wrapHeight == true)
        {
            if (isWrappingHeight == false)
            {
                if (viewportPosition.y > 1)
                {
                    newPosition.y = mainCamera.ViewportToWorldPoint(Vector2.zero).y;
                    isWrappingHeight = true;
                }
                else if (viewportPosition.y < 0)
                {
                    newPosition.y = mainCamera.ViewportToWorldPoint(Vector2.one).y;
                    isWrappingHeight = true;
                }
            }
        }

        transform.position = newPosition;
    }

    private bool IsBeingRendered()
    {
        if(spriteRenderer.isVisible == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}