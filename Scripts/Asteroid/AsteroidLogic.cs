using UnityEngine;
using System.Collections;

public class AsteroidLogic : MonoBehaviour {

    public float minMoveSpeed = 80f;
    public float maxMoveSpeed = 100f;
    private float moveSpeed;

    public int deathTime = 60;

    public bool isParent = false;
    private bool isDead = false;

    private bool deathWasBoss = false;

    public GameObject childAsteroidPrefab;

    public GameObject laserExplosionParticlePrefab;
    public GameObject explosionParticlePrefab;

    public AudioClip[] explosionSounds;
    private AudioSource audSource;

    private Transform playerTransform;
    //private Vector2 newPosition;

    private SpriteRenderer spriteRenderer;
    private ScoreManager scoreManager;
    //private FreezeDeath freezeDeath;

    private Rigidbody2D asteroidRigidbody;

    private bool createChildAsteroids = false;
    private bool wasLaserBullet = false;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //newPosition = playerTransform.TransformDirection(playerTransform.transform.position);
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        audSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        //freezeDeath = GetComponent<FreezeDeath>();
        isDead = false;

        Destroy(gameObject, deathTime);

        Vector3 dir = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        asteroidRigidbody = GetComponent<Rigidbody2D>();
        asteroidRigidbody.AddForce(transform.up * moveSpeed);
    }

    private void Update()
    {
        //transform.position = Vector2.MoveTowards(transform.position, newPosition, Time.deltaTime * moveSpeed);
        //Vector2 positionToGo = new Vector2(0, transform.position.y);
        //transform.Translate(positionToGo * Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(isDead == false)
        {
            if (target.tag == "Player")
            {
                PlayerController player = target.GetComponent<PlayerController>();
                player.Death();
            }
            else if(target.tag == "Bullet")
            {
                createChildAsteroids = true;
                Destroy(target.gameObject);
                Death();
            }
            else if(target.tag == "LaserBullet")
            {
                createChildAsteroids = false;
                wasLaserBullet = true;
                Destroy(target.gameObject);
                Death();
            }
            else if(target.tag == "Boss")
            {
                createChildAsteroids = false;
                deathWasBoss = true;
                Death();
            }
        }
    }

    public void Death()
    {
        if(isDead == false)
        {
            if(isParent == true && createChildAsteroids == true)
            {
                Instantiate(childAsteroidPrefab, transform.position, Quaternion.identity);
                Instantiate(childAsteroidPrefab, transform.position, Quaternion.identity);
            }

            if(wasLaserBullet == true)
            {
                Instantiate(laserExplosionParticlePrefab, transform.position, Quaternion.identity);

                if(deathWasBoss == false)
                {
                    scoreManager.AddScore(200);
                }                    
            }
            else if(wasLaserBullet == false)
            {
                Instantiate(explosionParticlePrefab, transform.position, Quaternion.identity);

                if (deathWasBoss == false)
                {
                    scoreManager.AddScore(100);
                }
            }

            audSource.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
            spriteRenderer.enabled = false;
            Destroy(gameObject, 5f);
            isDead = true;
        }
    }
}