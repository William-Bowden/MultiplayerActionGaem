using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed = 10.0f;

    public bool onTimer;
    public float timeAlive;
    float timer;

    public bool stopOnTrigger;
    bool triggered;
    float stopTimer = 0.5f;

    // Start is called before the first frame update
    void Start() {
        timer = timeAlive;
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.right * speed * Time.deltaTime;

        if( triggered ) {
            stopTimer -= Time.deltaTime;

            if( stopTimer <= 0 ) {
                speed = 0;
            }
        }
        if( onTimer ) {
            timer -= Time.deltaTime;

            if( timer <= 0 ) {
                Destroy( gameObject );
            }
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        if( stopOnTrigger ) {
            if( collision.gameObject.layer == 8 || collision.gameObject.layer == 9 ) {
                triggered = true;
            }
        }
    }
}
