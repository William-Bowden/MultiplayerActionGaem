using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

    [SerializeField]
    float resetDistance = -118.5f;
    [SerializeField]
    float scrollSpeed = -2.0f;

    // Update is called once per frame
    void Update() {
        for( int i = 0; i < transform.childCount; i++ ) {
            Transform child = transform.GetChild( i );
            child.position += transform.right * scrollSpeed * Time.deltaTime;

            // absolute value so that this script works nicely for either direction scrolling
            if( Mathf.Abs( child.position.x ) > Mathf.Abs( resetDistance ) ) {
                Vector3 temp = child.position;
                temp.x -= resetDistance * 2;
                child.position = temp;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawLine( transform.position, transform.position + ( transform.right * resetDistance ) );
        Gizmos.DrawWireSphere( ( transform.right * resetDistance ), 3.0f );
    }
}
