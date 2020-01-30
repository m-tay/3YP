using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RoomAdder : MonoBehaviour
{
    public GameObject[] tiles;  // holds tiles that can be used to fill map
    public GameObject[] interiors;
    public GameObject[] doors;

    public int percDoorWillSpawn = 50;

    private GameObject target; 

    public void CheckAndFill() {

        // create a up vector
        Vector3 up = transform.TransformDirection(Vector3.up);

        // cast a ray up to see if rooms are detected
        if(Physics.Raycast(transform.position, up, 10)) {
            //Debug.Log("ROOM DETECTED AT" + transform.position);
        }
        else {
            // if no room detected, spawn a room
            //Debug.Log("ROOM NOT DETECTED AT" + transform.position);
            Instantiate(tiles[0], transform.position, Quaternion.identity);
            
        }

        // generate random interior (this happens in all rooms)
        int r = Random.Range(0, interiors.Length);
        Instantiate(interiors[r], transform.position, Quaternion.identity);
         
    }

    public void AddDoors() {
        // DOOR SPAWNING

        // setup raycasting
        RaycastHit hit;
        Ray ray = new Ray();    
        ray.origin = transform.position;
        
        // setup layer mask for doors
        int layermask = 1 << 11;

        // // setup layer mask for doors when they spawned
        // int layermaskSpawned = 1 << 12;

        // forward check        
        if(Physics.Raycast(transform.position, Vector3.forward, out hit, 20, layermask)) {
            target = hit.transform.gameObject;

            if(validDoorPosition(target.transform) && Random.Range(0, 100) < percDoorWillSpawn) {                
                Instantiate(doors[0], target.transform.position, Quaternion.identity, target.transform);
                target.layer = 12;
            }
        }    

        // backwards check        
        if(Physics.Raycast(transform.position, Vector3.back, out hit, 20, layermask)) {
            target = hit.transform.gameObject;

            if(validDoorPosition(target.transform) && Random.Range(0, 100) < percDoorWillSpawn) {
                Instantiate(doors[0], target.transform.position, Quaternion.identity, target.transform);
                target.layer = 12;
            }
        }    

        // left check        
        var rotation = Quaternion.identity * Quaternion.Euler(0, 90, 0);
        if(Physics.Raycast(transform.position, Vector3.left, out hit, 20, layermask)) {
            target = hit.transform.gameObject;

            if(validDoorPosition(target.transform) && Random.Range(0, 100) < percDoorWillSpawn) {
                Instantiate(doors[0], target.transform.position, Quaternion.identity * rotation, target.transform);
                target.layer = 12;
            }
        }            
         
        // right check        
        if(Physics.Raycast(transform.position, Vector3.right, out hit, 20, layermask)) {
            target = hit.transform.gameObject;

            if(validDoorPosition(target.transform) && Random.Range(0, 100) < percDoorWillSpawn) {

                Instantiate(doors[0], target.transform.position, Quaternion.identity * rotation, target.transform);
                target.layer = 12;
            }
        }       

    }

    public bool validDoorPosition(Transform pos) {
        // copy door spanwer position       
        Vector3 doorPos = pos.position;

        // move in and out
        float zFor = doorPos.z + 1;
        float zBack = doorPos.z - 1;
        float xLeft = doorPos.x + 1;
        float xRight = doorPos.x - 1;

        // create new vectors in new positions
        Vector3 forwardPos = new Vector3(doorPos.x, 1, zFor);
        Vector3 backPos = new Vector3(doorPos.x, 1, zBack);
        Vector3 leftPos = new Vector3(xLeft, 1, doorPos.z);
        Vector3 rightPos = new Vector3(xRight, 1, doorPos.z);
            
        // setup raycasting
        RaycastHit hit;
        Ray ray = new Ray();    
                
        // debug drawing lines
        // Debug.DrawRay(forwardPos, (Vector3.down * 2), Color.green, 1000000000, false);
        // Debug.DrawRay(backPos, (Vector3.down * 2), Color.red, 1000000000, false);
        // Debug.DrawRay(leftPos, (Vector3.down * 2), Color.blue, 1000000000, false);
        // Debug.DrawRay(rightPos, (Vector3.down * 2), Color.yellow, 1000000000, false);

        // counter to check if right number of floors hit
        int counter = 0;

        ray.origin = forwardPos;
        if(Physics.Raycast(forwardPos, Vector3.down, out hit, 5))
            counter++;

        if(Physics.Raycast(backPos, Vector3.down, out hit, 5))
            counter++;
            
        if(Physics.Raycast(leftPos, Vector3.down, out hit, 5))
            counter++;

        if(Physics.Raycast(rightPos, Vector3.down, out hit, 5))
            counter++;

        // allow the door to spawn if there is a floor on either side
        if(counter == 4) {
            return true;
        }

        
        // delete the wall and replace with the one next to it if not valid
        if(counter < 4) {
            // tile edge walls are on layer 13
            int wallLayer = 1 << 13;

            // create new point to raycast from, close to ceiling and near doorframe, to get doorframe tile
            Vector3 checkPos = new Vector3((pos.position.x - 0.5f), 2.9f, (pos.position.z - 0.5f));
            
            // debut rays
            Debug.DrawRay(checkPos, Vector3.forward, Color.green, 1000000000, false);
            Debug.DrawRay(checkPos, Vector3.right, Color.yellow, 1000000000, false);

            // check for wall layer stuff
            if((Physics.Raycast(checkPos, Vector3.forward, out hit, 1)) || (Physics.Raycast(checkPos, Vector3.right, out hit, 1))) {                // get hit object                
                // get reference to hit object
                GameObject obj = hit.transform.gameObject;

                // save position of wall
                Vector3 wallPosition = obj.transform.position;
                wallPosition = new Vector3(wallPosition.x, wallPosition.y, wallPosition.z);

                
                // get wall to left
                Vector3 wallNborPos = new Vector3(pos.position.x, 1.0f, pos.position.z);

                // copy neighbour 
                if(Physics.Raycast(wallNborPos, Vector3.left, out hit, 3, wallLayer)) {
                    Quaternion wallRotation = Quaternion.Euler(0, 90, 0);                     
                    GameObject newWall = Instantiate(hit.transform.gameObject, wallPosition, wallRotation);
                }

                if(Physics.Raycast(wallNborPos, Vector3.forward, out hit, 3, wallLayer)) {
                    Quaternion wallRotation = Quaternion.Euler(0, 0, 0);                     
                    GameObject newWall = Instantiate(hit.transform.gameObject, wallPosition, wallRotation);
                }



                // destroy doorframe
                Destroy(obj);

            }
            
            return false;
        }



        return false;
    }

}
