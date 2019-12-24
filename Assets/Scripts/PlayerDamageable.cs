using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    public GameObject starsPrefab;

    Character character;
    TargetGroupManager tgm;

    LayerMask origLayer;
    LayerMask immune = 20;

    protected override void Start() {
        base.Start();
        tgm = GameObject.Find( "PlayerInputManager" ).GetComponent<TargetGroupManager>();

        character = GetComponent<Character>();
        origLayer = character.gameObject.layer;
    }

    public override void Replenish() {
        base.Replenish();

        if( tgm ) {
            tgm.Add( transform );
        }

        character.gameObject.layer = origLayer;
    }

    protected override void Die() {
        base.Die();

        character.gameObject.layer = immune;

        if( tgm ) {
            tgm.Remove( transform );
        }

        character.canInput = false;
    }

    protected override void OnCollisionEnter2D( Collision2D collision ) {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        Character character = collision.gameObject.GetComponent<Character>();

        if( collision.transform != transform && ( damageable ) ) {
            // if another player "stomps" on this player, this player takes damage
            if( dieOnStomped ) {
                if( !character ) {
                    float speedOfCollision = collision.rigidbody.velocity.y;
                    if( speedOfCollision > -3.0f ) {
                        return;
                    }
                }

                if( character.GetComponent<Rigidbody2D>().velocity.y < -0.1f ) {
                    Vector3 diff = Vector3.Normalize( transform.position - collision.transform.position );
                    float dot = Vector3.Dot( diff, Vector3.up );

                    if( dot < -0.8f ) {
                        if( !isDead ) {
                            GameObject stars = Instantiate( starsPrefab, transform.position + new Vector3( 0.02f, 0, 0 ), Quaternion.identity, transform );
                        }

                        TakeDamage( maxHealth * 0.5f );
                    }
                }
            }
        }
    }

}
