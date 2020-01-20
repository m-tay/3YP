using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // GENERATION DEBUG PARAMETERS
    bool genCritialPath = true;
    bool genFillerTiles = true;

    // enum to hold all the types of rooms (directions of entry/exit) that are possible
    public enum roomType{
        All,
        BL,
        BR,
        LR,
        TB,
        TL,
        TR,
        TLR,
        BLR
    }

    public enum Direction{
        Left,
        Right, 
        Down
    }

    public GameObject[] tilesL;
    public GameObject[] tilesR;
    public GameObject[] tilesD;
    public GameObject[] tilesDD;

    public GameObject helperDD;
    public GameObject helperD;
    public GameObject helperL;
    public GameObject helperR;


    public GameObject startTile;
    public GameObject[] startpoints;    // holds all the players possible start points
    public GameObject[] fillpoints;     // holds all the fillpoints, to spawn rooms on if empty
    public GameObject player;
    public GameObject scarecrow;
    public GameObject[] interiors;
    public GameObject endpoint;

    private float moveAmount = 27; // how to move spawning position around - size of tile
    private Direction direction;      // direction to move random walk level generator

    int r = 0;
    private int downCount = 0;
    private bool keepGenerating = true; // checks if bottom bound has been reached when generating

    // array of directions, weighted to make left/right movement more likely
    private Direction[] possDirections = {Direction.Left, Direction.Left, Direction.Right, Direction.Right, Direction.Down};

    // bounding for the random walk
    private int minX = -0;
    private int maxX = 130;
    private int minZ = -160;



    // Start is called before the first frame update
    void Start()
    {   
      

        int rand = 0;

        // init spawn room with BLR room (tiles[3]) at random location
        rand = Random.Range(0, startpoints.Length); // get random position
        var room = Instantiate(startTile, startpoints[rand].transform.position, Quaternion.identity); // spawn room

        // move player and enemy to starting positions
        player.transform.position = startpoints[rand].transform.position;
        

        // set first direction for random walk level generation
        direction = getRandDirection();
        Debug.Log("First direction is " + direction + " , location is " + transform.position);

        if(direction == Direction.Down) {
            downCount++;
        }

        // set transform position to match starting position
        transform.position = startpoints[rand].transform.position;

        // run level generation
        if(genCritialPath)
            Generate();

        // run room filler
        if(genFillerTiles)
            Fill();

        // build dynamic navmesh
        this.transform.parent.GetComponent<NavMeshBuilder>().buildNavMesh();

        if(genFillerTiles)
            AddDoors();

        // move scarecrow to start point
        Vector3 scarecrowStart = new Vector3(player.transform.position.x + 3, player.transform.position.y, player.transform.position.z);
        scarecrow.transform.position = scarecrowStart;
        
        // add navagent to scarecrow dynamically
        var sc = scarecrow.GetComponent<ScarecrowController>();
        sc.startNavAgent(scarecrowStart);
    }

    private void Generate() {

        // keep running level generation moves until the generator reaches the bottom level (on z axis)
        while(keepGenerating) {
            Move();
        }

    }

    private void Move() {
        if(direction == Direction.Right) { // move right

            if(transform.position.x < maxX)  {// check for bounding

                // reset downcount
                downCount = 0;
                
                // create new position and move transform there
                Vector3 newPos = new Vector3(transform.position.x + moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;
                Debug.Log("Going right, spawning at " + newPos);

                // generate next move, do not allow left moves
                direction = getRandDirection();
                while(direction == Direction.Left) {
                    direction = getRandDirection();
                }

                if(transform.position.x + moveAmount + 2 > maxX)  {
                    direction = Direction.Down; // if reached bound, go down
                    downCount++;
                    Debug.Log("Right bound UP NEXT reached, going down");
                
                }

            }
            else {
                direction = Direction.Down; // if reached bound, go down
                downCount++;
                Debug.Log("Right bound reached, going down");
            }

            // spawn tile at new location - all rooms have R exits so randomise from tiles[]
            r = Random.Range(0, tilesR.Length);

            // check if next direction is going down - must generate room with B opening
            if(direction == Direction.Down && downCount < 2) {
                r = Random.Range(0, tilesD.Length);
                var room = Instantiate(tilesD[r], transform.position, Quaternion.identity);



            }
            if (direction == Direction.Down && downCount >= 2) {
                r = Random.Range(0, tilesDD.Length);
                var room = Instantiate(tilesDD[r], transform.position, Quaternion.identity);


                

            }
            if (direction == Direction.Right) {
                var room = Instantiate(tilesR[r], transform.position, Quaternion.identity);


                


                //Debug.Log("Spawning tile at " + transform.position);
            }

        }

        if(direction == Direction.Left) { // move left
            if(transform.position.x > minX) {   // check for bounding

                // reset downcount
                downCount = 0;
    
                // create new position and move transform there
                Vector3 newPos = new Vector3(transform.position.x - moveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;
                Debug.Log("Going left, spawning at " + newPos);
                
                // generate next move, do not allow right moves
                direction = getRandDirection();
                while(direction == Direction.Right) {
                    direction = getRandDirection();
                }

                if(transform.position.x - moveAmount - 2 < minX)  {
                    direction = Direction.Down; // if reached bound, go down
                    downCount++;
                    Debug.Log("Left bound UP NEXT reached, going down");
                }

            }
            else {
                direction = Direction.Down;
                downCount++;
                Debug.Log("Left bound reached, going down");

            }
            
            // spawn tile at new location - all rooms have L exits so randomise from tiles[]
            r = Random.Range(0, tilesL.Length);

            // check if next direction is going down - must generate room with B opening
            if(direction == Direction.Down && downCount < 2) {
                r = Random.Range(0, tilesD.Length);
                var room = Instantiate(tilesD[r], transform.position, Quaternion.identity);

                
            } 
            if(direction == Direction.Down && downCount >= 2) {
                r = Random.Range(0, tilesDD.Length);
                var room = Instantiate(tilesDD[r], transform.position, Quaternion.identity);

                
            }
            if(direction == Direction.Left) {
                var room = Instantiate(tilesL[r], transform.position, Quaternion.identity);

                
                //Debug.Log("Spawning tile at " + transform.position);
            }
        }

        if(direction == Direction.Down ) { // move down

            // downcounter tracks how many times moved down, to ensure valid tile (all 4 exits) placed
            downCount++;

            if(transform.position.z > minZ) { // check for bounding
                
                // create new position and move transform there
                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - moveAmount);
                transform.position = newPos;
                Debug.Log("Going down, spawning at " + newPos);

                // check if moved down more than once - spawn all 4 opening room
                if(downCount >= 2) {
                    r = Random.Range(0, tilesDD.Length);
                    var room = Instantiate(tilesDD[r], transform.position, Quaternion.identity);

                    

                }
                else {
                    // spawn tile at new location
                    r = Random.Range(0, tilesD.Length);
                    var room = Instantiate(tilesD[r], transform.position, Quaternion.identity);


                    //Debug.Log("Spawning tile at " + transform.position);
                }
            }
            else {
                // bottom bound reached, so stop generating
                keepGenerating = false;

                // move level endpoint to this tile
                endpoint.transform.position = transform.position;
            }

            // generate next move, can go any direction
            direction = getRandDirection();

        }

        Debug.Log("Downcount: " + downCount);


    }

    private Direction getRandDirection() {
        int dirIndex = Random.Range(1, possDirections.Length);
        return possDirections[dirIndex];
    }

    // fills empty gaps with rooms
    private void Fill() {
        // for all fillpoints
        for(int i = 0; i < fillpoints.Length; i++) {
            fillpoints[i].GetComponent<RoomAdder>().CheckAndFill();
        }
    }

    public void AddDoors() {
        // for all fillpoints
        for(int i = 0; i < fillpoints.Length; i++) {
            fillpoints[i].GetComponent<RoomAdder>().AddDoors();
        }
    }


}
