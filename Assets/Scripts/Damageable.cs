using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    [SerializeField]
    float health;

    float maxHealth;

    public GameObject[] onDeathParticles;
    public AudioClip[] onDeathNoises;

    [SerializeField]
    bool dieOnStomped;

    [HideInInspector]
    public bool isDead;

    SpriteRenderer sr;

    // Use this for initialization
    protected virtual void Start() {
        maxHealth = health;
        sr = GetComponent<SpriteRenderer>();

        if( !sr ) {
            sr = transform.GetChild( 0 ).GetComponent<SpriteRenderer>();
        }
    }

    void Update() {
        if( sr ) {
            if( sr.color.g < 0.25f + health / maxHealth || sr.color.b < 0.25f + health / maxHealth ) {
                Color temp = sr.color;
                temp.g = Mathf.Min( sr.color.g + Time.deltaTime * 2, 1 );
                temp.b = Mathf.Min( sr.color.b + Time.deltaTime * 2, 1 );
                sr.color = temp;
            }
        }
    }

    public void TakeDamage( float amount ) {
        // this method is only for adding damage to health
        // therefore, if the damage is negative, exit the method
        if( amount < 0 ) {
            return;
        }

        health -= amount;
        if( sr ) {
            Color temp = sr.color;
            temp.g = 0.15f;
            temp.b = 0.15f;
            sr.color = temp;
        }

        // after all damage has been applied, check if health is equal to or below 0
        if( health <= 0 && !isDead ) {
            Die();
        }
    }

    public void Heal( float amount ) {
        // this method is only for subtracting damage from health
        // therefore, if the damage is negative, exit the method
        if( amount < 0 ) {
            return;
        }

        health += amount;

        // after all damage has been applied, check if health is equal to or below 0
        if( health - amount <= 0 ) {
            Revive();
        }
        if( health > maxHealth ) {
            health = maxHealth;
        }
    }

    // replenishes health back to maxHealth
    public virtual void Replenish() {
        health = maxHealth;
        if( sr ) {
            Color temp = sr.color;
            temp.g = 1;
            temp.b = 1;
            sr.color = temp;
        }
        isDead = false;
    }

    protected virtual void Die() {
        DeathEffects();
        isDead = true;

        // destroy this damageable
        gameObject.SetActive( false );
    }

    protected void DeathEffects() {
        // if there are particles to be spawned on death
        if( onDeathParticles.Length != 0 ) {
            for( int i = 0; i < onDeathParticles.Length; i++ ) {
                // create a gameobject for the particles
                GameObject deathParticles = Instantiate( onDeathParticles[ i ], transform.position, Quaternion.identity );
                // grab the duration of the deathParticles (plus the longest lifetime of a single particle)
                float particleTime = onDeathParticles[ i ].GetComponent<ParticleSystem>().main.duration + onDeathParticles[ i ].GetComponent<ParticleSystem>().main.startLifetime.constantMax;

                // destroy the particle system when it is complete
                Destroy( deathParticles, particleTime );
            }
        }

        if( onDeathNoises.Length != 0 ) {
            AudioSource.PlayClipAtPoint( onDeathNoises[ Random.Range( 0, onDeathNoises.Length ) ], transform.position );
        }
    }

    public void Revive() {
        gameObject.SetActive( true );
        isDead = false;
        // add revival sounds/effects?
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();
        Interactable pickup = collision.gameObject.GetComponent<Interactable>();
        Character character = collision.gameObject.GetComponent<Character>();
        if( collision.transform != transform && ( damageable || pickup ) ) {
            if( dieOnStomped ) {
                if( !character ) {
                    float speedOfCollision = collision.rigidbody.velocity.y;
                    if( speedOfCollision > -6.0f ) {
                        return;
                    }
                }

                Vector3 diff = Vector3.Normalize( transform.position - collision.transform.position );
                float dot = Vector3.Dot( diff, Vector3.up );

                if( dot < -0.8f ) {
                    Die();
                }
            }
        }
    }
}
