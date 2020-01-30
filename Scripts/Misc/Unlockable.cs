using UnityEngine;
using System.Collections;

public class Unlockable : MonoBehaviour {

    public float rotateSpeed = 1f;
    public float forceAmount = 20f;

    public int upgradeType = 1; // 1 = Laser, 2 = Ice, 3 = Automatic 4 = Score +1000 5 = Free Life 6 = KillAll
    private int[] upgradeTypes = { 1, 2, 3, 4, 5, 6 };

    public GameObject explosionParticlePrefab;
    public GameObject laserExplosionParticlePrefab;

    public float chanceOfGoldUnlockable = 7f;
    public Color normalColor = new Color(255,255, 0);
    public Color goldColor = new Color(255, 185, 0);
    public GameObject goldFrostParticleChild;
    public bool isGold = false;

    public AudioClip[] explosionSounds;
    public AudioClip killAllClip;
    public AudioClip upgradeClip;
    private AudioSource audSource;

    public GameObject boomTextPrefab;
    private GameObject UICanvas;

    private Transform playerTransform;
    private Rigidbody2D spriteRigidbody;

    private SpriteRenderer spriteRenderer;

    private PlayerController player;
    private ScoreManager scoreManager;

    private bool isDead = false;
    private bool wasLaser = false;

    private void Start()
    {
        SetUpgradeType();

        audSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        UICanvas = GameObject.FindGameObjectWithTag("UICanvas");

        Vector3 dir = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        spriteRigidbody = GetComponent<Rigidbody2D>();
        spriteRigidbody.AddForce(transform.up * forceAmount);

        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        float chance = Random.Range(0, 100);

        if(chance <= chanceOfGoldUnlockable)
        {
            isGold = true;
        }
        else
        {
            isGold = false;
        }

        if(isGold == true)
        {
            goldFrostParticleChild.SetActive(true);
            spriteRenderer.color = goldColor;
        }
        else if(isGold == false)
        {
            goldFrostParticleChild.SetActive(false);
            spriteRenderer.color = normalColor;
        }

    }

    private void Update()
    {
        transform.Rotate(0, 0, 50 * Time.deltaTime * rotateSpeed);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Player")
        {
            player = target.GetComponent<PlayerController>();
            Upgrade();
        }
        else if(target.tag == "Bullet")
        {
            wasLaser = false;
            Death();
        }
        else if(target.tag == "LaserBullet")
        {
            wasLaser = true;
            Death();
        }
    }

    private void Upgrade()
    {
        if(isDead == false)
        {
            audSource.PlayOneShot(upgradeClip);

            if(upgradeType == 1)
            {
                player.UpgradeToLaser();
            }
            else if(upgradeType == 2)
            {
                player.UpgradeToIce();
            }
            else if(upgradeType == 3)
            {
                player.UpgradeToAutomatic();
            }
            else if(upgradeType == 4)
            {
                Debug.Log("+1000 Score!");
                scoreManager.AddScore(1000);
            }
            else if(upgradeType == 5)
            {
                Debug.Log("+Free Life!");
                player.hasFreeLife = true;
            }
            else if(upgradeType == 6)
            {
                KillAll();
            }

            spriteRenderer.enabled = false;
            Destroy(gameObject, 5f);
            isDead = true;
        }
    }

    private void Death()
    {
        if(isDead == false)
        {
            if (wasLaser == true)
            {
                Instantiate(laserExplosionParticlePrefab, transform.position, transform.rotation);
            }
            else if (wasLaser == false)
            {
                Instantiate(explosionParticlePrefab, transform.position, transform.rotation);
            }

            audSource.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
            spriteRenderer.enabled = false;
            Destroy(gameObject, 5f);
            isDead = true;
        }
    }

    private void SetUpgradeType()
    {
        upgradeType = upgradeTypes[Random.Range(0, upgradeTypes.Length)];
    }

    private void KillAll()
    {
        GameObject boomTextInstance = (GameObject)Instantiate(boomTextPrefab, UICanvas.transform.position, Quaternion.identity);
        boomTextInstance.transform.SetParent(UICanvas.transform);

        GameObject[] allAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] allUFOs = GameObject.FindGameObjectsWithTag("UFO");

        foreach(var asteroid in allAsteroids)
        {
            Instantiate(laserExplosionParticlePrefab, asteroid.transform.position, asteroid.transform.rotation);
            Destroy(asteroid);
            scoreManager.AddScore(100);
        }

        foreach(var ufo in allUFOs)
        {
            Instantiate(laserExplosionParticlePrefab, ufo.transform.position, ufo.transform.rotation);
            Destroy(ufo);
            scoreManager.AddScore(100);
        }

        audSource.PlayOneShot(killAllClip);
    }

}