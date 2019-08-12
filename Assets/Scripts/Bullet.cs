using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    float damage = 1;

    [SerializeField]
    bool explosive = false;
    [SerializeField]
    float explosionRadius = 1.0f;

    [SerializeField]
    bool randomBetweenForce = false;
    [SerializeField]
    float initialForceMin = 3.5f;
    [SerializeField]
    float initialForce = 3.5f;


    [SerializeField]
    bool randomBetweenLife = false;
    [SerializeField]
    float MinLife = 2f;
    [SerializeField]
    float maxLife = 2f;
    float lifeTimer;

    // should this bullet be removed on trigger enter?
    public bool removeOnTrigger;
    // should this bullet be removed on collision enter?
    public bool removeOnCollision;

    public GameObject puffPrefab;
    public GameObject explosionPrefab;

    private const float FORCE_MULTIPLIER = 100;
    private Rigidbody2D rb;

    [HideInInspector]
    public Transform owner;

    int groundLayer = 1 << 8;
    int platformLayer = 1 << 9;
    float collideDistCheck = 0.35f;

    // Use this for initialization
    void Start() {
        if( randomBetweenForce ) {
            initialForce = Random.Range( initialForceMin, initialForce );
        }
        if( randomBetweenLife ) {
            maxLife = Random.Range( MinLife, maxLife );
        }

        rb = GetComponent<Rigidbody2D>();
        lifeTimer = maxLife;

        rb.AddForce( ( initialForce * FORCE_MULTIPLIER ) * transform.right );

        bool shouldBeDestroyed = false;

        RaycastHit2D hitLocation = Physics2D.Raycast( transform.position, transform.right, 50.0f, groundLayer );

        RaycastHit2D bulletInGround = Physics2D.Raycast( transform.position, transform.right, 0.25f, groundLayer );

        if( bulletInGround ) {
            Destroy( gameObject );
        }

        RaycastHit2D gunInGround = Physics2D.Raycast( transform.position, -transform.right, 0.5f, platformLayer );

        shouldBeDestroyed = bulletInGround || gunInGround;

        if( shouldBeDestroyed ) {
            Destroy( gameObject );
        }
    }

    // Update is called once per frame
    void Update() {
        lifeTimer -= Time.deltaTime;

        // if the bullet's life is up, remove it
        if( lifeTimer <= 0 ) {
            Destroy( gameObject );
        }
    }

    private void LateUpdate() {
        CheckForPossibleCollision();
    }

    private bool CheckForPossibleCollision() {
        // check ground
        RaycastHit2D right = Physics2D.Raycast( transform.position, transform.right, collideDistCheck, groundLayer );

        RaycastHit2D left = Physics2D.Raycast( transform.position, -transform.right, collideDistCheck, groundLayer );

        if( right.collider || left.collider ) {
            Destroy( gameObject );
            return true;
        }

        // check platforms
        right = Physics2D.Raycast( transform.position, transform.right, collideDistCheck, platformLayer );

        left = Physics2D.Raycast( transform.position, -transform.right, collideDistCheck, platformLayer );

        if( right.collider || left.collider ) {
            Destroy( gameObject );
            return true;
        }
        return false;
    }

    private void OnDestroy() {
        if( explosive ) {
            RaycastHit2D[] hits = Physics2D.CircleCastAll( transform.position, explosionRadius, Vector2.zero );

            for( int i = 0; i < hits.Length; i++ ) {
                Damageable damageable = hits[ i ].transform.GetComponent<Damageable>();

                if( damageable ) {
                    float distance = Mathf.Abs( ( hits[ i ].centroid - hits[ i ].point ).magnitude );

                    float totalDmg = ( 1.3f - distance / explosionRadius ) * damage;
                    // take damage depending on how close target is to explosion
                    damageable.TakeDamage( totalDmg );
                }
            }

            GameObject explosion = Instantiate( explosionPrefab, transform.position, Quaternion.identity );
            Destroy( explosion, 2.0f );
        }

        GameObject smokePuff = Instantiate( puffPrefab, transform.position, Quaternion.identity );
        Destroy( smokePuff, 1.0f );
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if( removeOnCollision ) {
            Destroy( gameObject );
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        bool collisionCheck = CheckForPossibleCollision();

        if( !collisionCheck ) {
            if( owner ) {
                if( collision.transform == owner || collision.transform.IsChildOf( owner ) ) {
                    return;
                }
            }
            if( removeOnTrigger ) {
                Destroy( gameObject );
            }
            if( !removeOnCollision ) {
                Damageable targetDamagable = collision.gameObject.GetComponent<Damageable>();

                if( targetDamagable ) {
                    targetDamagable.TakeDamage( damage );
                }
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if( explosive ) {
            Gizmos.DrawWireSphere( transform.position, explosionRadius );
        }
        Gizmos.DrawLine( transform.position, -transform.right * 0.75f + transform.position );
    }
}
