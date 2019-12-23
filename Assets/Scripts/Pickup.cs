using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : Interactable
{
    protected SpriteRenderer sr;
    ItemStand stand;

    public AudioClip pickupSound;
    public GameObject pickupParticles;


    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<CircleCollider2D>();
        stand = transform.parent.GetComponent<ItemStand>();
    }

    public void Enable() {
        gameObject.SetActive( true );
    }

    public void Disable() {
        gameObject.SetActive( false );
    }

    protected virtual void Collect( GameObject collectingObject ) {
        Disable();

        // play sound
        if( pickupSound ) {
            AudioSource.PlayClipAtPoint( pickupSound, transform.position, 1 );
        }

        // create particles
        if( pickupParticles ) {
            GameObject particles = Instantiate( pickupParticles, transform.position, Quaternion.identity );

            Destroy( particles, 1f );
        }

        if( stand ) {
            stand.hasItem = false;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        Collect( collision.gameObject );
    }

}
