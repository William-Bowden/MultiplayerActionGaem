using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public bool open = false;

    GameObject closedChild;
    GameObject openChild;

    // Start is called before the first frame update
    void Start() {
        closedChild = transform.GetChild( 0 ).gameObject;
        openChild = transform.GetChild( 1 ).gameObject;
    }

    public override void Interact() {
        Debug.Log( "A DOOR was interacted with" );
        open = !open;

        if( open ) {
            openChild.SetActive( true );
            closedChild.SetActive( false );
        }
        else {
            closedChild.SetActive( true );
            openChild.SetActive( false );
        }
    }
}
