using UnityEngine;
using System.Collections;

public class MenuAsteroid : MonoBehaviour {

    public float moveSpeed = 0.025f;
    public float rotateSpeed = -0.25f;

    public GameObject asteroidChild;

    private void Start()
    {
        Destroy(gameObject, 60f);
    }

    private void Update()
    {
        transform.Translate(Vector2.right * moveSpeed);
        asteroidChild.transform.Rotate(0, 0, 50 * Time.deltaTime * rotateSpeed);
    }

}