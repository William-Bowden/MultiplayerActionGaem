using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrabber : MonoBehaviour {

    Transform weaponsTransform;
    List<WeaponPickup> weapons;

    bool GAMERUNNING = false;

    public Transform weaponHeld = null;

    // Start is called before the first frame update
    void Awake() {
        weapons = new List<WeaponPickup>();

        GAMERUNNING = true;
    }

    public void Interact() {
        if( !weaponHeld ) {
            WeaponPickup closestWeapon = null;
            float shortestDistance = 0.0f;

            foreach( WeaponPickup weapon in weapons ) {
                // if this weapon is not available, skip checking it
                if( !weapon.IsAvailable() ) {
                    continue;
                }

                float dist = Mathf.Abs( ( transform.parent.position - weapon.transform.position ).magnitude );

                // set the line color to green for weapons in reach
                if( dist <= weapon.pickupRadius ) {
                    // if there is no current weapon or the distance to this weapon is shorter than the current closest weapon
                    if( !closestWeapon || shortestDistance > dist ) {
                        // set this weapon as the new closest
                        closestWeapon = weapon;
                        shortestDistance = dist;
                    }
                }
            }

            // if there is a weapon that is the closest and within reach
            if( closestWeapon ) {
                // pick it up
                weaponsTransform = closestWeapon.transform.parent;
                closestWeapon.SetAvailability( false );
                closestWeapon.transform.SetParent( transform );
                closestWeapon.transform.localPosition = Vector3.zero;
                Vector3 scale = closestWeapon.transform.localScale;
                scale.y = 1;
                closestWeapon.transform.localScale = scale;
                closestWeapon.onStand = false;
                weaponHeld = closestWeapon.transform;

                weaponHeld.rotation = transform.parent.localRotation;
            }
        }
        else {
            WeaponPickup currentWeapon = weaponHeld.GetComponent<WeaponPickup>();

            currentWeapon.SetAvailability( true );
            currentWeapon.transform.SetParent( weaponsTransform );
            weaponHeld = null;
        }
    }

    private void OnTriggerEnter2D( Collider2D collision ) {
        weapons.Add( collision.GetComponent<WeaponPickup>() );
    }
    private void OnTriggerExit2D( Collider2D collision ) {
        weapons.Remove( collision.GetComponent<WeaponPickup>() );
    }

    void OnDrawGizmos() {
        if( GAMERUNNING && weapons.Count > 0 ) {
            // draw a line to all weapons
            foreach( WeaponPickup weapon in weapons ) {
                if( weapon == null ) {
                    continue;
                }
                // set the line color to red
                Gizmos.color = Color.red;

                float dist = Mathf.Abs( ( transform.parent.position - weapon.transform.position ).magnitude );

                // set the line color to green for weapons in reach
                if( dist <= weapon.pickupRadius ) {
                    Gizmos.color = Color.green;
                }

                Gizmos.DrawLine( transform.parent.position, weapon.transform.position );
            }
        }
    }

}
