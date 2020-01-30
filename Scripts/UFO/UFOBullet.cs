using UnityEngine;
using System.Collections;

public class UFOBullet : MonoBehaviour {

    public float bulletSpeed = 250f;
    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        // FacePlayer
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Player")
        {
            Destroy(gameObject);
            player.Death();
        }
    }
}