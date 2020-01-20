using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    // store reference to level generator script
    public GameObject LevelGenerator;
    private NavMeshBuilder navMeshBuilder;

    public int timeBeforeStartingLevelChanges;
    public int changeLevelEvery;

    // layermask bitshifted to detect layer 9
    int layerMask = 1 << 10;


    // Start is called before the first frame update
    void Start()
    {
        navMeshBuilder = LevelGenerator.GetComponent<NavMeshBuilder>();
        InvokeRepeating("CheckBehind", timeBeforeStartingLevelChanges, changeLevelEvery);
    }

    private void CheckBehind() {
        
        RaycastHit hit;
        
        // detect
        if(Physics.Raycast(transform.position, -transform.forward, out hit, 50, layerMask)){

            // ensure player is out of the room before triggering a regen
            if(hit.distance > 10) {
            
            // regenerate point
            hit.collider.GetComponent<ReGenerator>().regenPoint();

            // rebuild nav mesh whenever level is changed
            //navMeshBuilder.buildNavMesh();

            Debug.Log("REGEN OCCURRED");


            }
            
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
