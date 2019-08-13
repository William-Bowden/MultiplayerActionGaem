using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Interactable {

    [SerializeField]
    float pushForce = 10000.0f;

    [SerializeField]
    bool dampenFirst = false;

    [SerializeField]
    float interactRate = 1.0f;
    float interactTimer;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();

        if( !rb ) {
            rb = transform.parent.GetComponent<Rigidbody2D>();
        }
    }

    private void Update() {
        if( interactTimer > 0 ) {
            interactTimer -= Time.deltaTime;
        }
    }

    public override void Interact( Transform interactor ) {
        if( interactTimer <= 0 ) {
            Vector3 pushDir = Vector3.Normalize( interactor.position - interactor.parent.position );
            float dot = Vector2.Dot( rb.velocity.normalized, pushDir );

            // if it should dampenFirst and it's currnetly moving against new force, dampen it
            if( dampenFirst && dot < 0 ) {
                rb.velocity /= 10.0f;
            }

            rb.AddForce( pushForce * pushDir );

            interactTimer = interactRate;
        }
    }


}
