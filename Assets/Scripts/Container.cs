using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : ObjectDamageable
{

    // if true, a single item will be dropped from the whatToDrop list
    // if false, all items from the whatToDrop list will be dropped
    [SerializeField]
    bool givesRandom = false;
    [SerializeField]
    int numToGive = 1;


    [SerializeField]
    protected List<GameObject> whatToDrop;
    protected List<GameObject> gameObjects;


    [SerializeField]
    Vector3 goRotation;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        gameObjects = new List<GameObject>();
    }

    protected override void Die() {
        if( givesRandom ) {
            for( int i = 0; i < numToGive; i++ ) {
                GameObject go = whatToDrop[ Random.Range( 0, whatToDrop.Count ) ];
                if( go ) {
                    gameObjects.Add( Instantiate( go, transform.position, Quaternion.Euler( goRotation ) ) );
                }
            }
        }
        else {
            for( int i = 0; i < numToGive; i++ ) {
                foreach( GameObject go in whatToDrop ) {
                    if( go ) {
                        GameObject dropped = Instantiate( go, transform.position, Quaternion.Euler( goRotation ) );
                        gameObjects.Add( dropped );

                        Bullet bullet = dropped.GetComponent<Bullet>();
                        if( bullet ) {
                            Destroy( dropped );
                        }
                    }
                }
            }
        }

        base.Die();
    }

}
