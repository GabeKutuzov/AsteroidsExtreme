using UnityEngine;
using System.Collections;

public class UFOBossLogic : MonoBehaviour {

    public float moveSpeed = 0.25f;

    public float minNewPosTimer = 2f;
    public float maxNewPosTimer = 4f;
    private float newPosTimer;

    public Color colorA;
    public Color colorB;
    public float colorSmooth = 2f;
    public float colorTransitionTime = 0.25f;
    private float colorTransitionTimeStart;
    private Color newColor;
    private bool currentColorA = true;

    public float shootMinTimer = 2f;
    public float shootMaxTimer = 3f;
    public float shootCurrentTimer;
    public GameObject bulletPrefab;

    public float minUfoSpawnTimer = 5f;
    public float maxUfoSpawnTimer = 10f;
    public float currentUfoSpawnTimer;
    public GameObject ufoPrefab;

    public Collider2D ufoCollider;

    public float health = 100;
    private float startHealth;

    public GameObject hitParticlePrefab;
    public GameObject bossDeathParticlePrefab;
    public GameObject unlockablePrefab;

    private SpriteRenderer spriteRen;

    public AudioClip[] hitClips;
    public AudioClip shootClip;
    public AudioClip deathClip;
    private AudioSource audSource;

    private PlayerController player;
    private ScoreManager scoreManager;
    private BossManager bossManager;

    private Vector2 newPosition;
    private Vector2 damagePosition;

    private bool isDead = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        bossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();

        spriteRen = GetComponent<SpriteRenderer>();
        audSource = GetComponent<AudioSource>();

        colorTransitionTimeStart = colorTransitionTime;

        CreatePosTimer();
        SetNewPosition();

        SetShootTimer();
        SetUFOSpawnTimer();

        startHealth = health;
    }

    private void Update()
    {
        newPosTimer -= Time.deltaTime;

        bossManager.bossHealthBar.fillAmount = health / startHealth;

        if(newPosTimer <= 0)
        {
            SetNewPosition();
            CreatePosTimer();
        }

        if(health <= 0)
        {
            Death();
        }

        colorTransitionTime -= Time.deltaTime;

        if(colorTransitionTime <= 0)
        {
            currentColorA = !currentColorA;
            colorTransitionTime = colorTransitionTimeStart;
        }

        if(currentColorA == true)
        {
            newColor = colorA;
        }
        else if(currentColorA == false)
        {
            newColor = colorB;
        }

        shootCurrentTimer -= Time.deltaTime;

        if (shootCurrentTimer <= 0)
        {
            Shoot();
            SetShootTimer();
        }

        currentUfoSpawnTimer -= Time.deltaTime;

        if(currentUfoSpawnTimer <= 0)
        {
            Instantiate(ufoPrefab, transform.position, Quaternion.identity);
            SetUFOSpawnTimer();
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, newPosition, Time.deltaTime * moveSpeed);
        spriteRen.color = Color.Lerp(spriteRen.color, newColor, Time.deltaTime * colorSmooth);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(isDead == false)
        {
            if (target.tag == "Bullet" || target.tag == "LaserBullet" || target.tag == "IceBullet")
            {
                audSource.PlayOneShot(hitClips[Random.Range(0, hitClips.Length)]);
                damagePosition = target.transform.position;
                TakeDamage(Random.Range(1, 4));
                Destroy(target.gameObject);
            }
        }
    }

    private void CreatePosTimer()
    {
        newPosTimer = Random.Range(minNewPosTimer, maxNewPosTimer);
    }

    private void SetNewPosition()
    {
        newPosition = player.GetUFOPosition(10);
    }

    private void TakeDamage(float damage)
    {
        Debug.Log("UFO Boss Took Damage: " + damage + " " + "Health: " + health);
        health -= damage;

        GameObject particleHitInstance = (GameObject)Instantiate(hitParticlePrefab, damagePosition, Quaternion.identity);
        particleHitInstance.GetComponent<ParticleSystem>().startColor = spriteRen.color;
    }

    private void SetShootTimer()
    {
        shootCurrentTimer = Random.Range(shootMinTimer, shootMaxTimer);
    }

    private void SetUFOSpawnTimer()
    {
        currentUfoSpawnTimer = Random.Range(minUfoSpawnTimer, maxUfoSpawnTimer);
    }

    private void Death()
    {
        if(isDead == false)
        {
            ufoCollider.enabled = false;
            Instantiate(bossDeathParticlePrefab, transform.position, Quaternion.identity);
            Instantiate(unlockablePrefab, transform.position, Quaternion.identity);
            audSource.Stop();
            audSource.PlayOneShot(deathClip);
            isDead = true;
            spriteRen.enabled = false;
            bossManager.BossDeath();
            scoreManager.AddScore(2500);
            Destroy(gameObject, 5f);
        }
    }

    private void Shoot()
    {
        if (isDead == false && spriteRen.isVisible == true)
        {
            audSource.PlayOneShot(shootClip);
            Instantiate(bulletPrefab, transform.position, bulletPrefab.transform.rotation);
        }
    }
}