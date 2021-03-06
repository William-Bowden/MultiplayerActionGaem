﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] List<Vector3> startPositions = new List<Vector3>();
    PlayerIdentifier identifier;

    List<Vector3> spawnPositions = new List<Vector3>();

    int playerNumber = 0;

    [SerializeField]
    bool noReuse = true;

    private void Start() {
        spawnPositions = startPositions;
    }

    void OnPlayerJoined( PlayerInput input ) {

        Character c = input.GetComponent<Character>();

        if( c ) {
            c.SetPlayerHandler( this );
            identifier = c.GetComponentInChildren<PlayerIdentifier>();

            Vector3 pos = Vector3.zero;

            //if( noReuse ) {
            //    pos = startPositions[ Random.Range( 0, startPositions.Count - 1 ) ];
            //    startPositions.Remove( pos );
            //}
            //else {
            //pos = startPositions[ playerNumber % startPositions.Count ];
            pos = startPositions[ playerNumber++ % startPositions.Count ];
            //}

            input.transform.root.position = pos;
        }


        identifier.SetColor( playerNumber );
    }

    // TODO: if called twice(simultaneously), currently possible to return the same position
    public Vector3 GetRandSpawnPos() {
        return spawnPositions[ Random.Range( 0, spawnPositions.Count - 1 ) ];
    }

    private void OnDrawGizmosSelected() {
        foreach( Vector3 pos in startPositions ) {
            Gizmos.DrawWireSphere( pos, 1.0f );
        }
    }

}
