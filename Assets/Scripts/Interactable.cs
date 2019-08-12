using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    Collider2D col;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    public virtual void Interact() {
        Debug.Log( gameObject.name + " was interacted with" );
    }

    protected virtual void OnTriggerEnter2D( Collider2D collision ) {

    }

}
