using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorOpener : MonoBehaviour
{
    public float doorOpenSpeed = 0.6f;

    private bool playerInRange = false;
    private bool doorOpening = false;

    private bool doorOpened = false;

    private float angleToOpen = 90;


    void Update() {
        if(playerInRange && !doorOpening) {
            if(Input.GetKeyDown(KeyCode.E)) {
                Debug.Log("Opening door with angle" + angleToOpen);
                StartCoroutine( openDoor(Vector3.up, angleToOpen, doorOpenSpeed) );    
                 
            }
        }        
    }

// triggers to detect being in range of door
void OnTriggerEnter(Collider other) {
    if (other.tag == "Player") {
        playerInRange = true;
    }
}

void OnTriggerExit(Collider other) {
    if (other.tag == "Player") {
        playerInRange = false;
    }
}

// to be run as coroutine to open door
IEnumerator openDoor( Vector3 axis, float angle, float duration = 1.0f)
   {      
       doorOpening = true;
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler( axis * angle );
            
        float elapsed = 0.0f;
        while( elapsed < duration ) {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration );
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;

        Debug.Log("Door opening complete");
        flipOpenAngle(); 
        doorOpening = false;
   }

// helper to flip door opening angles
void flipOpenAngle() {
    if(angleToOpen == 90) {
        angleToOpen = -90;
        Debug.Log("Now 0");
    }
    else if(angleToOpen == -90) {
        angleToOpen = 90;
        Debug.Log("Now 90");


    }
}


}
