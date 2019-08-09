using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoomer : MonoBehaviour {

    CinemachineVirtualCamera cam;

    [SerializeField] CinemachineTargetGroup tg = null;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<CinemachineVirtualCamera>();

        if( !tg ) {
            tg = GameObject.Find( "TargetGroup1" ).GetComponent<CinemachineTargetGroup>();
        }
    }

    // Update is called once per frame
    void Update() {
        float furthest = 1;

        if( tg.m_Targets.Length > 1 ) {
            foreach( CinemachineTargetGroup.Target target1 in tg.m_Targets ) {
                foreach( CinemachineTargetGroup.Target target2 in tg.m_Targets ) {
                    float dist = Mathf.Abs( ( target1.target.position - target2.target.position ).magnitude );

                    if( dist > furthest ) {
                        furthest = dist;
                    }
                }
            }


        }

        cam.m_Lens.OrthographicSize = Mathf.Lerp( cam.m_Lens.OrthographicSize, ( furthest + 7 ) / 2, 0.1f );
    }
}
