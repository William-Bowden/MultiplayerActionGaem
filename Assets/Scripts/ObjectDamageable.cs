using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamageable : Damageable
{
    protected override void Die() {
        base.Die();

        // deactivate this damageable
        gameObject.SetActive( false );
    }

    protected override void OnCollisionEnter2D( Collision2D collision ) {
        Character character = collision.gameObject.GetComponent<Character>();
        if( collision.transform != transform && character ) {
            if( dieOnStomped ) {
                Vector3 diff = Vector3.Normalize( transform.position - collision.transform.position );
                float dot = Vector3.Dot( diff, Vector3.up );

                if( dot < -0.8f || dot > 0.8f ) {
                    Die();
                }
            }
        }
    }

}
