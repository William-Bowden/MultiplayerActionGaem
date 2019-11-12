using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWhenFar : MonoBehaviour
{

    [SerializeField]
    protected float maxDistance = 50.0f;

    [SerializeField]
    bool kill;

    // Update is called once per frame
    void Update() {
        float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

        if( distFromCenter > maxDistance ) {
            if( kill ) {
                Damageable damageable = gameObject.GetComponent<Damageable>();
                damageable.TakeDamage( 1000 );
            }
            else {
                gameObject.SetActive( false );
            }
        }
    }
}
