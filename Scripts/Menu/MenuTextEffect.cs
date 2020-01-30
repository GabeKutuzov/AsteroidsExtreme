using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTextEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public Vector3 normalScale = Vector3.one;
    public Vector3 newScale = Vector3.one;
    public float scaleSpeed = 5f;

    private bool isStretch = false;

    private void FixedUpdate()
    {
        if(isStretch == true)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, newScale, Time.deltaTime * scaleSpeed);
        }
        else if(isStretch == false)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, normalScale, Time.deltaTime * scaleSpeed);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isStretch = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isStretch = false;        
    }
}