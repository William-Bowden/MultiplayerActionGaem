using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStand : ItemStand
{

    protected override void SpawnItem() {
        WeaponPickup weaponToSpawn = null;

        foreach( Interactable weapon in itemPool ) {
            if( !weapon.gameObject.activeInHierarchy ) {
                weaponToSpawn = weapon.GetComponent<WeaponPickup>();
                break;
            }
        }

        if( weaponToSpawn ) {
            weaponToSpawn.gameObject.SetActive( true );
            weaponToSpawn.MakeAvailable();
            weaponToSpawn.transform.position = transform.position + itemOffset;
            weaponToSpawn.transform.rotation = Quaternion.identity;
            weaponToSpawn.transform.localScale = Vector3.one;
            hasItem = true;
        }
        else {
            hasItem = false;
        }
    }

}
