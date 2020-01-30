using UnityEngine;
using System.Collections;

public class UFOLogic : MonoBehaviour {

    public float moveSpeed = 5f;

    public GameObject bulletPrefab;

    public float newPositionMinTimer = 2f;
    public float newPositionMaxTimer = 4f;
    public float newPositionCurrentTimer;

    public float shootMinTimer = 2f;
    public float shootMaxTimer = 3f;
    public float shootCurrentTimer;

    private bool bulletWasLaser = false;

    public AudioClip[] explosionClips;
    public AudioClip shootClip;
    private AudioSource audSource;

    private BossManager bossManager;

    public GameObject explosionParticlePrefab;
    public GameObject laserExplosionParticlePrefab;

    private GameObject ufoBoss;

    private Vector2 newPosition;

    private PlayerController player;
    private ScoreManager scoreManager;

    private SpriteRenderer spriteRenderer;

    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        bossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audSource = GetComponent<AudioSource>();

        if(bossManager.bossActive == true)
        {
            ufoBoss = GameObject.FindGameObjectWithTag("Boss");
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), ufoBoss.GetComponent<Collider2D>());
        }

        SetNewPosition();
        SetShootTimer();
    }

    private void Update()
    {
        newPositionCurrentTimer -= Time.deltaTime;

        if(newPositionCurrentTimer <= 0)
        {
            SetNewPosition();
        }

        shootCurrentTimer -= Time.deltaTime;

        if(shootCurrentTimer <= 0)
        {
            Shoot();
            SetShootTimer();            
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Bullet")
        {
            bulletWasLaser = false;
            Destroy(target);
            Death();
        }
        else if(target.tag == "LaserBullet")
        {
            bulletWasLaser = true;
            Destroy(target);
            Death();
        }
    }

    private void SetNewPosition()
    {
        newPositionCurrentTimer = Random.Range(newPositionMinTimer, newPositionMaxTimer);
        newPosition = player.GetUFOPosition(5);
    }

    private void Shoot()
    {
        if(isDead == false && spriteRenderer.isVisible == true)
        {
            audSource.PlayOneShot(shootClip);
            Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        }
    }

    private void SetShootTimer()
    {
        shootCurrentTimer = Random.Range(shootMinTimer, shootMaxTimer);
    }

    private void Death()
    {
        if(isDead == false)
        {
            if (bulletWasLaser == true)
            {
                Instantiate(laserExplosionParticlePrefab, transform.position, transform.rotation);
            }
            else if (bulletWasLaser == false)
            {
                Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
            }

            audSource.Stop();
            audSource.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
            spriteRenderer.enabled = false;
            scoreManager.AddScore(200);
            Destroy(gameObject, 5f);
            isDead = true;
        }
    }
}