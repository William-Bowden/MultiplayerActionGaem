using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {

    public int rotOffset = 0;
    public bool facingRight = true;
    private Transform gfx;

    private void Start() {
        gfx = transform.root.GetChild( 0 );
    }

    // Update is called once per frame
    void Update() {

        Vector3 difference = ( Camera.main.ScreenToWorldPoint( Input.mousePosition ) - transform.position ).normalized;

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
