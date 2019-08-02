using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStand : MonoBehaviour {

    public GameObject weaponPrefab;
    public List<WeaponPickup> weaponPool;
    private int poolSize = 8;


    Vector3 weaponOffset = new Vector3( 0, 0.5f, 0 );

    public bool hasWeapon = false;
    public bool spawnAtStart = false;

    public float spawnRate = 5.0f;
    public float spawnTimer;

    // Start is called before the first frame update
    void Start() {

        for( int i = 0; i < poolSize; i++ ) {
            GameObject currentWeapon = Instantiate( weaponPrefab, transform );
            currentWeapon.SetActive( false );
            WeaponPickup currentPickup = currentWeapon.GetComponent<WeaponPickup>();
            weaponPool.Add( currentPickup );
        }

        spawnTimer = spawnRate;

        if( spawnAtStart ) {
            spawnTimer = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        if( !hasWeapon ) {
            if( spawnTimer <= 0 ) {
                SpawnWeapon();
                spawnTimer = spawnRate;
            }
            else {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    void SpawnWeapon() {
        WeaponPickup weaponToSpawn = null;
        foreach( WeaponPickup weapon in weaponPool ) {
            if( /*weapon.IsAvailable() && */!weapon.gameObject.activeInHierarchy ) {
                weaponToSpawn = weapon;
                break;
            }
        }

        if( weaponToSpawn ) {
            weaponToSpawn.gameObject.SetActive( true );
            weaponToSpawn.MakeAvailable();
            weaponToSpawn.transform.position = transform.position + weaponOffset;
            weaponToSpawn.transform.rotation = Quaternion.identity;
            weaponToSpawn.transform.localScale = Vector3.one;
            hasWeapon = true;
        }
        else {
            hasWeapon = false;
        }
    }
}
