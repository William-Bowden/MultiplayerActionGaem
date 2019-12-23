using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    protected Collider2D col;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    public virtual void Interact( Transform interactor ) {
        
    }

}
