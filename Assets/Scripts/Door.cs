using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    public bool open = false;

    [SerializeField]
    AudioClip[] doorOpenSounds;
    [SerializeField]
    AudioClip[] doorCloseSounds;

    GameObject closedChild;
    GameObject openChild;

    // Start is called before the first frame update
    void Start() {
        closedChild = transform.GetChild( 0 ).gameObject;
        openChild = transform.GetChild( 1 ).gameObject;
    }

    public override void Interact( Transform interactor ) {
        open = !open;

        if( open ) {
            openChild.SetActive( true );
            closedChild.SetActive( false );
            AudioSource.PlayClipAtPoint( doorOpenSounds[ Random.Range( 0, doorOpenSounds.Length ) ], transform.position, 1f );
        }
        else {
            closedChild.SetActive( true );
            openChild.SetActive( false );
            AudioSource.PlayClipAtPoint( doorCloseSounds[ Random.Range( 0, doorCloseSounds.Length ) ], transform.position, 1f );
        }

        if( ( transform.position - interactor.position ).x < 0 ) {
            Vector3 temp = transform.localScale;
            temp.x = -1;
            transform.localScale = temp;
        }
        else {
            Vector3 temp = transform.localScale;
            temp.x = 1;
            transform.localScale = temp;
        }

    }
}
