using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float thrustForce = 250f;
    public float rotationSpeed = 250f;

    public bulletTypes currentBulletType = new bulletTypes();
    public enum bulletTypes { Normal, Laser, Ice};
    public bool hasPowerup = false;

    public float minUpgradeTimer = 45f;
    public float maxUpgradeTimer = 60f;
    public float currentUpgradeTime;

    public float minAutomaticTimer = 45f;
    public float maxAutomaticTimer = 60f;
    public float currentAutomaticTime;
    public bool isAutomatic = false;

    public float automaticFireRate = 0.15f;
    private float automaticFireRateStart;

    public bool hasFreeLife = false;
    private bool wasHit = false;
    public float flickerSpeed = 0.5f;
    private float flickerSpeedStart;
    public float flickerTimer = 3f;
    private float flickerTimerStart;

    public Image freeLifeImage;

    [HideInInspector]
    public bool isDead = false;

    public GameObject bulletPrefab;
    public GameObject laserBulletPrefab;
    public GameObject iceBulletPrefab;
    public GameObject shootPosition;

    public ParticleSystem thrustParticle;

    private Rigidbody2D rigidBody2D;

    public GameObject thrustSoundManager;
    private AudioSource thrustAudSource;
    private SpriteRenderer spriteRen;

    public AudioClip shootClip;
    public AudioClip laserShootClip;
    public AudioClip iceShootClip;
    public AudioClip deathClip;
    public AudioClip hitFreeLifeClip;
    private AudioSource audSource;

    private GameOverManager gameOverManager;

    private ScoreManager scoreManager;
    private UFOManager ufoManager;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        gameOverManager = GameObject.FindGameObjectWithTag("GameOverManager").GetComponent<GameOverManager>();
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        ufoManager = GameObject.FindGameObjectWithTag("UFOManager").GetComponent<UFOManager>();

        thrustAudSource = thrustSoundManager.GetComponent<AudioSource>();
        audSource = GetComponent<AudioSource>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRen = GetComponent<SpriteRenderer>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        SetUpgradeTimer();
        SetAutomaticTimer();

        automaticFireRateStart = automaticFireRate;
        flickerSpeedStart = flickerSpeed;
        flickerTimerStart = flickerTimer;
    }

    private void Update()
    {
        if(hasPowerup == true)
        {
            currentUpgradeTime -= Time.deltaTime;
        }

        if(currentUpgradeTime <= 0 && hasPowerup == true)
        {
            currentBulletType = bulletTypes.Normal;
            SetUpgradeTimer();
            hasPowerup = false;
        }

        if(isAutomatic == true)
        {
            currentAutomaticTime -= Time.deltaTime;
        }

        if(currentAutomaticTime <= 0 && isAutomatic)
        {
            SetAutomaticTimer();
            isAutomatic = false;
        }

        if(wasHit == true)
        {
            flickerTimer -= Time.deltaTime;
            flickerSpeed -= Time.deltaTime;

            if(flickerSpeed <= 0)
            {
                spriteRen.enabled = !spriteRen.enabled;
                flickerSpeed = flickerSpeedStart;
            }                         
        }

        if (flickerTimer <= 0)
        {
            flickerTimer = flickerTimerStart;
            hasFreeLife = false;
            wasHit = false;
            spriteRen.enabled = true;
        }

        if(hasFreeLife == true)
        {
            freeLifeImage.enabled = true;
        }
        else if(hasFreeLife == false)
        {
            freeLifeImage.enabled = false;
        }

        if (isDead == false)
        {
            if(isAutomatic == true)
            {
                if (Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Space))
                {
                    automaticFireRate -= Time.deltaTime;

                    if(automaticFireRate <= 0)
                    {
                        Shoot();
                        automaticFireRate = automaticFireRateStart;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }

            if(Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Space))
            {
                automaticFireRate = automaticFireRateStart;
            }
        }
        else if(isDead == true)
        {
            if (gameOverManager.canContinue == true)
            {
                if (Input.anyKey)
                {
                    gameOverManager.GameOverSet(false);
                    Respawn();
                }
            }
        }

        #region Cheats


        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            hasFreeLife = !hasFreeLife;
        }

        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            currentBulletType = bulletTypes.Normal;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            currentBulletType = bulletTypes.Laser;
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            currentBulletType = bulletTypes.Ice;
        }

        if(Input.GetKeyDown(KeyCode.Keypad4))
        {
            scoreManager.AddScore(5000);
        }


        #endregion Cheats

    }

    private void FixedUpdate()
    {
        if(isDead == true)
        {
            thrustAudSource.mute = true;
            thrustParticle.Stop();
        }

        else if(isDead == false)
        {
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                rigidBody2D.AddForce(transform.up * thrustForce);
                thrustAudSource.mute = false;
                thrustParticle.Play();
            }
            else if(!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.UpArrow))
            {
                thrustAudSource.mute = true;
                thrustParticle.Stop();
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rigidBody2D.angularVelocity = rotationSpeed;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                rigidBody2D.angularVelocity = -rotationSpeed;
            }
            else
            {
                rigidBody2D.angularVelocity = 0f;
            }
        }
    }

    private void Shoot()
    {
        if (currentBulletType == bulletTypes.Laser)
        {
            audSource.PlayOneShot(laserShootClip);
            Instantiate(laserBulletPrefab, shootPosition.transform.position, shootPosition.transform.rotation);
        }
        else if(currentBulletType == bulletTypes.Ice)
        {
            audSource.PlayOneShot(iceShootClip);
            Instantiate(iceBulletPrefab, shootPosition.transform.position, shootPosition.transform.rotation);
        }
        else if (currentBulletType == bulletTypes.Normal)
        {
            audSource.PlayOneShot(shootClip);
            Instantiate(bulletPrefab, shootPosition.transform.position, shootPosition.transform.rotation);
        }
    }

    public void Death()
    {
        if(hasFreeLife == true)
        {
            if(wasHit == false)
            {
                audSource.PlayOneShot(hitFreeLifeClip);
            }

            wasHit = true;
        }
        else if(hasFreeLife == false)
        {
            if (isDead == false)
            {
                isDead = true;
                gameOverManager.GameOverSet(true);
                audSource.PlayOneShot(deathClip);
            }
        }
    }

    public void Respawn()
    {
        rigidBody2D.velocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = startRotation;
        scoreManager.ResetScore();
        ufoManager.SetSpawnScore();
        currentBulletType = bulletTypes.Normal;
        SetUpgradeTimer();
        SetAutomaticTimer();
        hasPowerup = false;
        isAutomatic = false;
        hasFreeLife = false;
        isDead = false;

        GameObject[] allAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject[] allUnlockables = GameObject.FindGameObjectsWithTag("Unlockable");
        GameObject[] allUFOs = GameObject.FindGameObjectsWithTag("UFO");
        GameObject[] allUFOBullets = GameObject.FindGameObjectsWithTag("UFOBullet");
        GameObject[] allParticleEffects = GameObject.FindGameObjectsWithTag("ParticleEffect");
        GameObject[] allUnlockableSpawners = GameObject.FindGameObjectsWithTag("UnlockableSpawner");
        GameObject[] allBosses = GameObject.FindGameObjectsWithTag("Boss");

        BossManager bossManager = GameObject.FindGameObjectWithTag("BossManager").GetComponent<BossManager>();
        bossManager.SetSpawnScore();
        bossManager.BossDeath();

        foreach (var asteroid in allAsteroids)
        {
            Destroy(asteroid);
        }

        foreach (var unlockable in allUnlockables)
        {
            Destroy(unlockable);
        }

        foreach (var unlockableSpawn in allUnlockableSpawners)
        {
            UnlockableSpawner unlockableSpawner = unlockableSpawn.GetComponent<UnlockableSpawner>();
            unlockableSpawner.SetSpawnScore();
        }

        foreach (var ufo in allUFOs)
        {
            Destroy(ufo);
        }

        foreach(var bullet in allUFOBullets)
        {
            Destroy(bullet);
        }

        foreach(var particleEffect in allParticleEffects)
        {
            Destroy(particleEffect);
        }

        foreach (var boss in allBosses)
        {
            Destroy(boss);
        }
    }

    public void UpgradeToLaser()
    {
        Debug.Log("UpgradeToLaser");
        currentBulletType = bulletTypes.Laser;
        hasPowerup = true;
    }

    public void UpgradeToIce()
    {
        Debug.Log("UpgradeToIce");
        currentBulletType = bulletTypes.Ice;
        hasPowerup = true;
    }

    public void UpgradeToAutomatic()
    {
        Debug.Log("UpgradeToAuto");
        isAutomatic = true;
    }

    private void SetUpgradeTimer()
    {
        currentUpgradeTime = Random.Range(minUpgradeTimer, maxUpgradeTimer);
    }

    private void SetAutomaticTimer()
    {
        currentAutomaticTime = Random.Range(minAutomaticTimer, maxAutomaticTimer);
    }

    public Vector2 GetUFOPosition(float range)
    {
        Vector2 UFOPosition = Random.insideUnitCircle * range;
        return UFOPosition;
    }
}