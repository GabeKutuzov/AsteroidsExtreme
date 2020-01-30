using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public bool smooth = true;
    public float smoothSpeed = 4f;
    public Vector3 offset = new Vector3(0f, 0f, -6.5f);

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.transform.position + offset;

        if(smooth == true)
        {
            transform.localPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed); 
        }
        else if(smooth == false)
        {
            transform.position = desiredPosition;
        }
    }
}