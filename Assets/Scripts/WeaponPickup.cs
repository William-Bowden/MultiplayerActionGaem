using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

    public float pickupRadius = 1.0f;

    Gun gun;

    float maxRemovalTimer = 5.0f;
    float removalTimer = 5.0f;

    public bool onStand = false;

    [SerializeField]
    bool Available = true;
    public bool IsAvailable() {
        return Available;
    }
    public void MakeAvailable() {
        Available = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public void SetAvailability( bool newAvailability ) {
        if( !newAvailability && onStand ) {
            WeaponStand ws = transform.parent.GetComponent<WeaponStand>();
            ws.hasWeapon = false;
        }

        Available = newAvailability;
        TogglePhysics( newAvailability );
    }

    Rigidbody2D rb;
    Collider2D[] colliders;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();
        gun = GetComponent<Gun>();
        removalTimer = maxRemovalTimer;
    }

    private void Update() {
        if( Available ) {
            if( gun.currentAmmo <= 0 ) {
                removalTimer -= Time.deltaTime;
            }
            if( removalTimer <= 0 ) {
                RemovePickup();
            }

            float distFromCenter = Mathf.Abs( ( transform.position - Vector3.zero ).magnitude );

            if( distFromCenter > 30.0f ) {
                RemovePickup();
            }
        }
    }

    void TogglePhysics( bool physics ) {
        rb.bodyType = RigidbodyType2D.Dynamic;
        if( !physics ) {
            rb.velocity = Vector2.zero;
        }
        gun.enabled = !physics;
        rb.simulated = physics;
        if( physics ) {
            Vector3 dir = ( transform.parent.position - transform.parent.parent.position ).normalized;
            rb.AddForce( dir * 500.0f );
            gun.muzzleFlash.enabled = false;
        }
        foreach( Collider2D col in colliders ) {
            col.enabled = physics;
        }
    }

    void RemovePickup() {
        gameObject.SetActive( false );
        gun.Reload();
        removalTimer = maxRemovalTimer;
    }

    private void OnCollisionEnter2D( Collision2D collision ) {
        if( Mathf.Min( Random.Range( ( rb.velocity.magnitude ) / 6.0f, 1.0f ), 1.0f ) >= 0.75f ) {
            gun.enabled = true;
            gun.HitSurface();
            gun.muzzleFlash.enabled = false;
            gun.enabled = false;
        }
    }

}
