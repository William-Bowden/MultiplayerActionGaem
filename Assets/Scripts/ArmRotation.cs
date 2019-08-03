using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PlayerInput;

public class ArmRotation : MonoBehaviour {

    Vector2 aiming;
    public int rotOffset = 0;
    public bool facingRight = true;
    private Transform gfx;

    private void Start() {
        gfx = transform.root.GetChild( 0 );
    }

    private void Update() {
        Aim();
    }

    void OnAim( InputValue value ) {
        if( value.Get<Vector2>().magnitude > 0.5f ) {
            aiming = value.Get<Vector2>();
        }
    }

    void Aim() {
        Vector3 difference = new Vector3( aiming.x, aiming.y, 0 ).normalized;

        // calculate the rotation angle
        float zRot = Mathf.Atan2( difference.y, difference.x ) * Mathf.Rad2Deg;

        // rotate the arm
        transform.rotation = Quaternion.Euler( 0f, 0f, zRot + rotOffset );

        if( zRot > 90 || zRot < -90 ) {
            transform.localScale = new Vector3( 1, -1, 1 );
        }
        else {
            transform.localScale = new Vector3( 1, 1, 1 );
        }

        zRot = Mathf.Abs( zRot );

        if( zRot > 90 && !facingRight ) {
            Flip();
        }
        else if( zRot < 90 && facingRight ) {
            Flip();
        }
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = gfx.localScale;
        theScale.x *= -1;
        gfx.localScale = theScale;
    }
}
