using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWhenFar : MonoBehaviour {

    [SerializeField]
    float maxDistance = 50.0f;

    [SerializeField]
    bool kill;

    // Update is called once per frame
    void Update() {
        float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

        if( distFromCenter > maxDistance ) {
            gameObject.SetActive( false );
            if( kill ) {
                gameObject.GetComponent<Damageable>().TakeDamage( 1000 );
            }
        }
    }
}
