using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIdentifier : MonoBehaviour
{
    Image image;

    Color[] colors = {
        new Color( 1, 0.2f, 0.2f, 1 ),
        new Color( 0.2f, 0.2f, 1, 1 ),
        new Color( 0.2f, 1, 0.2f, 1 ),
        new Color( 1, 1, 0.2f, 1 ),
        new Color( 1, 0.2f, 1, 1 ),
        new Color( 0.2f, 1, 1, 1 ),
        new Color( 1, 1, 1, 1 ),
    };

    [SerializeField]
    Transform player;

    [SerializeField]
    Camera cam;

    RectTransform rt;

    public float pointerDist = -0.075f;
    Vector3 offset;

    // Start is called before the first frame update
    void Start() {
        image = GetComponent<Image>();
        rt = GetComponent<RectTransform>();

        if( !player ) {
            player = transform.root;
        }
        if( !cam ) {
            cam = Camera.main;
        }
    }

    public void SetColor(int playerNumber ) {
        if(!image) {
            image = GetComponent<Image>();
        }

        image.color = colors[ playerNumber ] * 0.9f;
    }

    private void OnGUI() {
        offset = new Vector3( 0, pointerDist * cam.orthographicSize, 0 );

        rt.position = cam.WorldToScreenPoint( player.position + offset );
    }
}
