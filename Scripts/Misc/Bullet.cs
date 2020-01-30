using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public float bulletForce = 200f;

    private PlayerController player;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D bulletRigidbody;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bulletRigidbody = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>());

        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach(var bullet in allBullets)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), bullet.GetComponent<Collider2D>());
        }
    }

    private void Update()
    {
        bulletRigidbody.AddForce(transform.up * bulletForce);

        if(spriteRenderer.isVisible == false)
        {
            Destroy(gameObject);
        }
    }
}