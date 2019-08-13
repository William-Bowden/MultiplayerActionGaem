using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PlayerInput;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] List<Vector3> startPositions = new List<Vector3>();

    int playerNumber = 0;

    void OnPlayerJoined( PlayerInput input ) {

        if( input.GetComponent<Character>() ) {
            Vector3 pos = startPositions[ Random.Range( 0, startPositions.Count - 1 ) ];
            input.transform.root.position = pos;

            startPositions.Remove( pos );
        }

    }

    private void OnDrawGizmosSelected() {
        foreach( Vector3 pos in startPositions ) {
            Gizmos.DrawWireSphere( pos, 1.0f );
        }
    }

}
