using UnityEngine;
using System.Collections;

public class FreezeDeath : MonoBehaviour {

    public float minFreezeTimer = 3f;
    public float maxFreezeTimer = 6f;
    public float currentFreezeTimer;

    [SerializeField]
    public Behaviour[] scriptsToDisable;

    public GameObject iceExplosionParticlePrefab;
    public GameObject frostParticlePrefab;

    public Color freezeColor;

    private ScoreManager scoreManager;

    public bool hasRigidbody = false;
    private Rigidbody2D objectRigidbody;

    public AudioClip[] explosionClips;
    private AudioSource audSource;

    private SpriteRenderer spriteRen;
    public bool isFrozen = false;

    private GameObject frostInstance;

    private void Start()
    {
        audSource = GetComponent<AudioSource>();
        spriteRen = GetComponent<SpriteRenderer>();

        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        if (hasRigidbody == true)
        {
            objectRigidbody = GetComponent<Rigidbody2D>();
        }

        CreateFreezeTimer();
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(isFrozen == false)
        {
            if (target.tag == "IceBullet")
            {
                Destroy(target.gameObject);
                Freeze();
            }
        }
    }

    private void CreateFreezeTimer()
    {
        currentFreezeTimer = Random.Range(minFreezeTimer, maxFreezeTimer);
    }

    private void Update()
    {
        if(isFrozen == true)
        {
            currentFreezeTimer -= Time.deltaTime;
        }

        if (currentFreezeTimer <= 0)
        {
            spriteRen.enabled = false;
            Instantiate(iceExplosionParticlePrefab, transform.position, transform.rotation);
            audSource.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
            Destroy(frostInstance);
            Destroy(gameObject, 5f);
            currentFreezeTimer = 100; // Set too high
        }
    }

    private void Freeze()
    {
        if(isFrozen == false)
        {
            for (int i = 0; i < scriptsToDisable.Length; i++)
            {
                scriptsToDisable[i].enabled = false;
            }

            if (hasRigidbody == true)
            {
                objectRigidbody.velocity = Vector2.zero;
            }

            frostInstance = (GameObject)Instantiate(frostParticlePrefab, transform.position, Quaternion.identity);

            audSource.Stop();
            spriteRen.color = freezeColor;
            isFrozen = true;
            scoreManager.AddScore(150);
        }

    }
}