using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRange : MonoBehaviour {

    public GameObject lanePrefab;
    public GameObject[] gunPrefabs;

    // Start is called before the first frame update
    void Start() {
        for( int i = 0; i < gunPrefabs.Length; i++ ) {
            GameObject lane = Instantiate( lanePrefab, transform.position + new Vector3( 0, 2.5f * ( i + 1 ), 0 ), Quaternion.identity, transform );
            GameObject stand = Instantiate( gunPrefabs[ i ], transform.position + new Vector3( -1.0f, 2.5f * ( i + 1 ) + 0.15f ), Quaternion.identity, lane.transform );

            gameObject.name += stand.name;

            WeaponStand weaponStand = stand.GetComponent<WeaponStand>();
            weaponStand.spawnRate = 1.0f;
            weaponStand.spawnAtStart = true;
        }
    }
}
