using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable {

    Character character;
    TargetGroupManager tgm;

    LayerMask origLayer;
    LayerMask immune = 20;

    protected override void Start() {
        base.Start();
        tgm = GameObject.Find( "PlayerInputManager" ).GetComponent<TargetGroupManager>();

        character = GetComponent<Character>();
        origLayer = character.gameObject.layer;
    }

    public override void Replenish() {
        base.Replenish();

        if( tgm ) {
            tgm.Add( transform );
        }

        character.gameObject.layer = origLayer;
    }

    protected override void Die() {
        DeathEffects();
        isDead = true;

        character.gameObject.layer = immune;

        if( tgm ) {
            tgm.Remove( transform );
        }

        character.canInput = false;
    }

}
