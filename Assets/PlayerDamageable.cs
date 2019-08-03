using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable {

    Character character;

    protected override void Start() {
        base.Start();

        character = GetComponent<Character>();
    }

    protected override void Die() {
        DeathEffects();

        character.canInput = false;
    }

}
