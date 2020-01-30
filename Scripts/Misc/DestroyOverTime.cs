using UnityEngine;
using System.Collections;

public class DestroyOverTime : MonoBehaviour {

    public float timeToDestroy = 10f;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }

}