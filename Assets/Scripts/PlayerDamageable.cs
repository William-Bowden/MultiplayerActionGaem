using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable {

    Character character;
    TargetGroupManager tgm;

    protected override void Start() {
        base.Start();
        tgm = GameObject.Find( "PlayerInputManager" ).GetComponent<TargetGroupManager>();

        character = GetComponent<Character>();
    }

    protected override void Die() {
        DeathEffects();
        isDead = true;

        if( tgm ) {
            tgm.Remove( transform );
        }

        character.canInput = false;
    }

}
