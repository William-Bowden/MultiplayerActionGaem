using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolleyBall : DisableWhenFar {

    ScoreKeeper sk;

    // Start is called before the first frame update
    void Start() {
        if( !sk ) {
            sk = GameObject.Find( "ScoreKeeper" ).GetComponent<ScoreKeeper>();
        }
    }

    // Update is called once per frame
    void Update() {
        if( transform.position.y < 0 ) {
            float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

            if( distFromCenter > maxDistance ) {
                SendInfo();

                gameObject.SetActive( false );
            }
        }
    }

    void SendInfo() {
        if( transform.position.x > 0 ) {
            sk.UpdateScore( 0 );
        }
        else {
            sk.UpdateScore( 1 );
        }
    }
}
