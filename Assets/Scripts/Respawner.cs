using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{

    public float spawnTimer = 1.0f;
    public float stopwatch = 0.0f;

    GameObject go;

    void Start() {
        stopwatch = spawnTimer;
        go = transform.GetChild( 0 ).gameObject;
    }

    // Update is called once per frame
    void Update() {
        if( go.activeInHierarchy == false ) {
            stopwatch -= Time.deltaTime;

            if( stopwatch <= 0 ) {
                go.transform.localPosition = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.SetActive( true );
                stopwatch = spawnTimer;

                Damageable damageable = go.GetComponent<Damageable>();
                damageable?.Replenish();
                damageable?.Revive();
            }
        }
    }
}
