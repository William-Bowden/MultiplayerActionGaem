using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {
    [SerializeField]
    float health;
    float maxHealth;

    public GameObject[] onDeathParticles;
    public AudioClip[] onDeathNoises;

    // Use this for initialization
    void Start() {
        maxHealth = health;
    }

    public void TakeDamage( float amount ) {
        // this method is only for adding damage to health
        // therefore, if the damage is negative, exit the method
        if( amount < 0 ) {
            return;
        }

        health -= amount;

        // after all damage has been applied, check if health is equal to or below 0
        if( health <= 0 ) {
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
    }

    // replenishes health back to maxHealth
    public void Replenish() {
        health = maxHealth;
    }

    void Die() {
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

        // destroy this damageable
        gameObject.SetActive( false );
    }

    void Revive() {
        gameObject.SetActive( true );

        // add revival sounds/effects?
    }
}
