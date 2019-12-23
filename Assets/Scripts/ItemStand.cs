using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStand : MonoBehaviour {

    [SerializeField]
    GameObject itemPrefab;
    
    public List<Interactable> itemPool;
    
    [SerializeField]
    int poolSize = 8;


    protected Vector3 itemOffset = new Vector3( 0, 0.5f, 0 );

    public bool hasItem = false;
    public bool spawnAtStart = false;

    public float spawnRate = 5.0f;
    public float spawnTimer;

    // Start is called before the first frame update
    void Start() {
        for( int i = 0; i < poolSize; i++ ) {
            GameObject currentItem = Instantiate( itemPrefab, transform );
            currentItem.SetActive( false );
            Interactable currentPickup = currentItem.GetComponent<Interactable>();
            itemPool.Add( currentPickup );
        }

        spawnTimer = spawnRate;

        if( spawnAtStart ) {
            spawnTimer = 0;
        }
    }

    // Update is called once per frame
    void Update() {
        if( !hasItem ) {
            if( spawnTimer <= 0 ) {
                SpawnItem();
                spawnTimer = spawnRate;
            }
            else {
                spawnTimer -= Time.deltaTime;
            }
        }
    }

    protected virtual void SpawnItem() {
        
    }
}
