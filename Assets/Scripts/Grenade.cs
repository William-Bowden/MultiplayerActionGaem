using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    float damage = 150f;
    float explosionRadius = 1.5f;


    [Range( 100f, 1000f ), SerializeField]
    float throwForce = 3;

    [Range( 0, 3f ), SerializeField]
    float fuseLength = 3;

    [SerializeField]
    bool onContact = false;
    public ParticleSystem explosion;

    Rigidbody2D rb;

    SpriteRenderer sr;
    bool tickColorOn = false;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        explosion = GetComponent<ParticleSystem>();

        float torque = Random.Range( 2.5f, 5.0f );

        rb.AddTorque( torque );
        rb.AddForce( transform.right * throwForce );

        StartCoroutine( Tick( fuseLength ) );
        StartCoroutine( Fuse( fuseLength ) );
    }

    void Boom() {
        sr.enabled = false;
        explosion.Play();

        RaycastHit2D[] hits = Physics2D.CircleCastAll( transform.position, explosionRadius, Vector2.zero );

        for( int i = 0; i < hits.Length; i++ ) {
            Damageable damageable = hits[ i ].transform.GetComponent<Damageable>();

            if( damageable ) {
                float distance = Mathf.Abs( ( hits[ i ].centroid - hits[ i ].point ).magnitude );

                float totalDmg = ( 1.3f - distance / explosionRadius ) * damage;
                // deal damage depending on how close target is to explosion
                damageable.TakeDamage( totalDmg );
            }
        }

        Destroy( gameObject, 0.5f );
    }

    void ToggleColor( bool stop = false ) {
        Color temp = sr.color;
        tickColorOn = !tickColorOn;

        if( stop ) {
            tickColorOn = false;
        }

        if( tickColorOn ) {
            temp.r = 1.0f;
            temp.g = 0.2f;
            temp.b = 0.2f;
        }
        else {
            temp.r = 1.0f;
            temp.g = 1.0f;
            temp.b = 1.0f;
        }

        sr.color = temp;
    }

    // Wait for the first 75% of the fuse, then blink for 25%, then blow up
    IEnumerator Tick( float waitTime ) {
        float newTime = waitTime / 2;
        yield return new WaitForSeconds( newTime * 0.75f );

        // the "timer" that controls color ticking of the grenade
        float ticker = newTime * 0.25f;

        while( true ) {
            yield return new WaitForSeconds( ticker );
            // reduce ticker
            ticker *= 0.75f;

            // turn red
            ToggleColor();

            yield return new WaitForSeconds( 0.05f );

            // return to normal color
            ToggleColor();
        }
    }

    IEnumerator Fuse( float waitTime ) {
        yield return new WaitForSeconds( waitTime );
        Boom();
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        // if the grenade collides with a player, have it explode
        if( onContact && collision.gameObject.layer == 10 ) {
            Boom();
        }
    }

}
