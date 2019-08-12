using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    Collider2D col;

    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    protected void Interact() {
        Debug.Log( gameObject + " was interacted with");
    }

    protected void OnTriggerEnter2D( Collider2D collision ) {

    }

}
