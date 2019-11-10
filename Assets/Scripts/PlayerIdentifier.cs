using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIdentifier : MonoBehaviour
{

    [SerializeField]
    Transform player;

    [SerializeField]
    Camera cam;

    RectTransform rt;

    Vector3 offset = new Vector3( 0, -0.75f, 0 );

    // Start is called before the first frame update
    void Start() {
        rt = GetComponent<RectTransform>();

        if( !player ) {
            player = transform.root;
        }
        if( !cam ) {
            cam = Camera.main;
        }
    }

    private void OnGUI() {
        rt.position = cam.WorldToScreenPoint( player.position + offset );
    }
}
