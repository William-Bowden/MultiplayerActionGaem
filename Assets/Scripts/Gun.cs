using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour {

    [Header( "Gun Attributes" )]
    [SerializeField]
    int maxAmmo = 0;
    public int currentAmmo = 0;

    [Range( 0.2f, 1 ), SerializeField]
    float weaponAccuracy = 1f;
    [Range( 0, 1.5f ), SerializeField]
    float fireRate = 0.5f;
    [SerializeField]
    float shootTimer = 0;
    float recoilTimer = 0;

    [Header( "'Anatomy'" )]
    public Transform muzzle;
    public SpriteRenderer muzzleFlash;
    public GameObject bulletPrefab;
    public GameObject muzzleSmokePrefab;
    public Sprite[] muzzleEffects;
    LineRenderer laserSight;

    [Header( "Audio" )]
    public AudioClip[] fireSounds;
    public AudioClip[] emptySounds;

    [Range( 0, 1 ), SerializeField]
    float fireVolume = 1;

    void Awake() {
        shootTimer = 0;
        recoilTimer = 0;
        currentAmmo = maxAmmo;
        if( muzzle.childCount > 0 ) {
            muzzleFlash = muzzle.GetChild( 0 ).GetComponent<SpriteRenderer>();
        }

        laserSight = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update() {
        shootTimer -= Time.deltaTime;
        recoilTimer += Time.deltaTime;

        if( fireRate - shootTimer > 0.05 || shootTimer <= 0 ) {
            muzzleFlash.enabled = false;
        }
    }

    private void OnEnable() {
        if( laserSight ) {
            laserSight.enabled = true;
        }
    }

    private void OnDisable() {
        if( laserSight ) {
            laserSight.enabled = false;
        }
    }

    public void Shoot() {
        // shoot if the gun is able to at this time
        if( shootTimer < 0 && currentAmmo >= 0 ) {
            int index = 0;

            if( currentAmmo > 0 ) {
                // create the bullet
                GameObject bulletGO = Instantiate( bulletPrefab, muzzle.position + new Vector3( 0, 0, 1 ), transform.rotation );

                // assign the owner (shooter) to the bullet(s)
                // if the bullet has children, do so for all children
                int childCount = bulletGO.transform.childCount;
                if( bulletGO.transform.childCount > 0 ) {
                    for( int i = 0; i < childCount; i++ ) {
                        Transform currentChild = bulletGO.transform.GetChild( i );
                        currentChild.GetComponent<Bullet>().owner = transform.root;
                        currentChild.position += new Vector3( 0, 0.01f * ( i - Mathf.Min( childCount / 2 ) ), 0 );
                        // rotate children bullets?
                        currentChild.Rotate( 0, 0, Random.Range( -10.0f, 10.0f ) );
                    }
                    Destroy( bulletGO, 0.25f );
                }
                else {
                    Bullet bullet = bulletGO.GetComponent<Bullet>();

                    if( bullet ) {
                        bulletGO.GetComponent<Bullet>().owner = transform.root;
                    }
                }

                // rotate the bullet to emulate weapon accuracy
                // the longer the wait between shots, the more accurate the shot is
                float timePast = 0;
                if( recoilTimer > fireRate * 1.1f ) {
                    timePast = Mathf.Min( recoilTimer / ( fireRate * 6 ), 1.0f );
                }
                float accuracy = Mathf.Max( 1 - timePast - weaponAccuracy, 0 );
                float rotation = accuracy * Random.Range( -30.0f, 30.0f );

                bulletGO.transform.Rotate( 0, 0, rotation );


                if( muzzleEffects.Length > 0 ) {
                    muzzleFlash.enabled = true;
                    muzzleFlash.sprite = muzzleEffects[ Random.Range( 0, muzzleEffects.Length - 1 ) ];
                }

                currentAmmo--;

                index = Random.Range( 0, fireSounds.Length );

                // play the shooting sound
                AudioSource.PlayClipAtPoint( fireSounds[ index ], muzzle.position, fireVolume );
            }
            else {
                index = Random.Range( 0, emptySounds.Length );

                // play the shooting sound
                AudioSource.PlayClipAtPoint( emptySounds[ index ], muzzle.position, fireVolume );
            }

            // reset the shoot timer
            shootTimer = fireRate;
            recoilTimer = 0;

            // create smoke
            GameObject smoke = Instantiate( muzzleSmokePrefab, muzzle.position + new Vector3( 0.02f, 0, 0 ), transform.rotation, muzzle );

            Destroy( smoke, 1f );
        }
    }

    public void HitSurface() {
        Shoot();
    }

    public void Reload() {
        currentAmmo = maxAmmo;
    }

}
