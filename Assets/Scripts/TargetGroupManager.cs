using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem.PlayerInput;

public class TargetGroupManager : MonoBehaviour
{

    [SerializeField] CinemachineTargetGroup tg = null;
    [SerializeField] Transform ball;
    bool trackingBall = false;

    [SerializeField]
    int playerCount = 0;
    float camTimer = 0;
    float maxTimer = 3.0f;
    int camPriority = 0;
    int prevPriority = 0;

    // Start is called before the first frame update
    void Start() {
        if( !tg ) {
            tg = GameObject.Find( "TargetGroup1" ).GetComponent<CinemachineTargetGroup>();
        }

        if( !ball ) {
            GameObject go = GameObject.Find( "BeachBall" );
            if( go ) {
                ball = go.transform;
            }
        }

        maxTimer = 6.0f / ( tg.m_Targets.Length - 1 );
    }

    private void Update() {
        if( playerCount <= 0 && !trackingBall ) {
            camTimer += Time.deltaTime;

            if( camTimer >= maxTimer ) {
                prevPriority = camPriority++;

                if( camPriority > tg.m_Targets.Length - 1 ) {
                    camPriority = 0;
                }
                camTimer = 0;
            }

            if( tg.m_Targets.Length > 0 ) {
                tg.m_Targets[ camPriority ].weight = Mathf.Lerp( tg.m_Targets[ camPriority ].weight, 10, maxTimer / 10.0f );

                if( prevPriority != camPriority ) {
                    tg.m_Targets[ prevPriority ].weight = Mathf.Lerp( tg.m_Targets[ prevPriority ].weight, 1, maxTimer / 50.0f );
                }
            }
        }
    }

    public void Add( Transform transformToAdd ) {
        tg.AddMember( transformToAdd, 1, 5 );
        playerCount++;
    }

    public void Remove( Transform transformToRemove ) {
        if( tg.FindMember( transformToRemove ) >= 0 )
            playerCount--;
        tg.RemoveMember( transformToRemove );
    }

    void OnPlayerJoined( PlayerInput input ) {

        if( input.GetComponent<Character>() ) {
            Transform member = input.transform.root;

            if( playerCount <= 0 && !trackingBall ) {
                foreach( CinemachineTargetGroup.Target target in tg.m_Targets ) {
                    tg.RemoveMember( target.target.transform );
                    if( target.target.transform == member ) {
                        return;
                    }
                }
            }

            if( !trackingBall && ball ) {
                tg.AddMember( ball, 1, 5 );
                trackingBall = true;
            }

            tg.AddMember( member, 1, 5 );
            playerCount++;
        }
    }
}
