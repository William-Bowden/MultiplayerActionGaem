using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.PlayerInput;

public class TargetGroupManager : MonoBehaviour {

    [SerializeField] CinemachineTargetGroup tg = null;

    int playerCount = 0;

    // Start is called before the first frame update
    void Start() {
        if( !tg ) {
            tg = GameObject.Find( "TargetGroup1" ).GetComponent<CinemachineTargetGroup>();
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
