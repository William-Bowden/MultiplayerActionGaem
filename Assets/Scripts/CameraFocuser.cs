using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFocuser : MonoBehaviour {

    CinemachineTargetGroup targetGroup;
    Transform focusTransform;

    // Start is called before the first frame update
    void Start() {
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();
        if( transform.childCount > 0 ) {
            focusTransform = transform.GetChild( 0 );
        }
        else {
            focusTransform = transform;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        targetGroup.AddMember( focusTransform, 1.0f, 3 );
    }
    private void OnTriggerExit2D( Collider2D collision ) {
        targetGroup.RemoveMember( focusTransform );
    }
}
