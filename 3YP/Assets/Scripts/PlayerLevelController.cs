using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelController : MonoBehaviour
{
    public int timeBeforeStartingLevelChanges;
    public int changeLevelEvery;

    // layermask bitshifted to detect layer 9
    int layerMask = 1 << 10;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CheckBehind", timeBeforeStartingLevelChanges, changeLevelEvery);
    }

    private void CheckBehind() {
        
        RaycastHit hit;
        
        // detect
        if(Physics.Raycast(transform.position, -transform.forward, out hit, 50, layerMask)){

            // ensure player is out of the room before triggering a regen
            if(hit.distance > 10) {

            hit.collider.GetComponent<ReGenerator>().regenPoint();
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
