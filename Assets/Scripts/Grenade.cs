using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    float damage = 150f;

    [Range( 0, 3f ), SerializeField]
    float explosionRadius = 1f;

    [Range( 100f, 1000f ), SerializeField]
    float throwForce = 400f;

    [Range( 0, 3f ), SerializeField]
    float fuseLength = 3;

    bool live = false;

    [SerializeField]
    bool onContact = false;
    [SerializeField]
    bool isSticky = false;
    bool hasStuck = true;
    public ParticleSystem explosion;

    Rigidbody2D rb;

    SpriteRenderer sr;
    bool tickColorOn = false;
    [SerializeField]
    AudioClip tickSound;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        explosion = GetComponent<ParticleSystem>();

        float torque = Random.Range( 2.5f, 5.0f );

        rb.AddTorque( torque );
        rb.AddForce( transform.right * throwForce );

        StartCoroutine( Fuse( fuseLength ) );
        StartCoroutine( Tick( fuseLength ) );
        StartCoroutine( ResetStickyness() );
    }

    void Boom() {
        sr.enabled = false;
        live = false;
        explosion.Play();

        transform.parent = null;
        rb.simulated = false;

        // change layers to something that won't collide (5 is UI)
        gameObject.layer = 5;

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

        Destroy( gameObject, explosion.main.duration );
    }

    public void Unstick() {
        StartCoroutine( ResetStickyness() );
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
            AudioSource.PlayClipAtPoint( tickSound, transform.position );
        }
        else {
            temp.r = 1.0f;
            temp.g = 1.0f;
            temp.b = 1.0f;
        }

        sr.color = temp;
    }

    IEnumerator Tick( float waitTime ) {
        float newTime = waitTime / 2;
        yield return new WaitForSeconds( newTime * 0.75f );

        // the "timer" that controls color ticking of the grenade
        float ticker = newTime * 0.25f;

        while( live ) {
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
        live = true;
        yield return new WaitForSeconds( waitTime );
        Boom();
    }

    IEnumerator ResetStickyness() {
        transform.parent = null;
        rb.simulated = true;

        // wait 0.075 seconds then re-enable stickyness
        yield return new WaitForSeconds( 0.075f );
        hasStuck = false;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // if a sticky grenade collides with something, have it stick
        if( isSticky && !hasStuck ) {
            rb.simulated = false;
            transform.parent = collision.transform;
            hasStuck = true;
        }

        // if an on-contact grenade collides with a player, have it explode
        else if( onContact && damageable ) {
            Boom();
        }
    }

}
