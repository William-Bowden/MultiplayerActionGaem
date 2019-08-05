using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.PlayerInput;

public class TargetGroupManager : MonoBehaviour {

    [SerializeField] CinemachineTargetGroup tg = null;

    int playerCount = 0;
    float camTimer = 0;
    int camPriority = 0;
    int prevPriority = 0;

    // Start is called before the first frame update
    void Start() {
        if( !tg ) {
            tg = GameObject.Find( "TargetGroup1" ).GetComponent<CinemachineTargetGroup>();
        }
    }

    private void Update() {
        if( playerCount <= 0 ) {
            camTimer += Time.deltaTime;

            if( camTimer >= 5.0f ) {
                //tg.m_Targets[ camPriority ].weight = 1f;

                prevPriority = camPriority++;

                if( camPriority > tg.m_Targets.Length - 1 ) {
                    camPriority = 0;
                }
                camTimer = 0;
            }

            //tg.m_Targets[ camPriority ].weight += Time.deltaTime;
            tg.m_Targets[ camPriority ].weight = Mathf.Lerp( tg.m_Targets[ camPriority ].weight, 5, 0.01f );

            if( prevPriority != camPriority ) {
                tg.m_Targets[ prevPriority ].weight = Mathf.Lerp( tg.m_Targets[ prevPriority ].weight, 1, 0.1f );
            }

            //foreach( CinemachineTargetGroup.Target target in tg.m_Targets ) {

            //}
        }
    }

    void OnPlayerJoined( PlayerInput input ) {
        Transform member = input.transform.root;

        foreach( CinemachineTargetGroup.Target target in tg.m_Targets ) {
            if( playerCount <= 0 ) {
                tg.RemoveMember( target.target.transform );
            }
            if( target.target.transform == member ) {
                return;
            }
        }

        tg.AddMember( member, 1, 5 );
        playerCount++;
    }
}
