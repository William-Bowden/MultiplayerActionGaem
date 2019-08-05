using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.PlayerInput;

public class PlayerHandler : MonoBehaviour {
    [SerializeField] List<Vector3> startPositions = new List<Vector3>();

    int playerNumber = 0;

    void OnPlayerJoined( PlayerInput input ) {
        Vector3 pos = startPositions[ Random.Range( 0, startPositions.Count ) ];
        input.transform.root.position = pos;

        startPositions.Remove( pos );
    }

}
