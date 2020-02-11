using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarecrowController : MonoBehaviour
{
    private Animator anim;
    public LevelLoader levelLoader;

    private bool attackTimerRunning = false;
    private float attackTimer = 0f;
    private float attackFinishedTime = 2.35f;

    public enum State {
        Wandering,
        Chasing,
        Stopped
    }
    public GameObject player;
    public State state = State.Chasing;
    
    // parameters for navagent
    public float agentRadius = 0.1f;



    private UnityEngine.AI.NavMeshAgent agent;

    void Start() {
        anim = GetComponent<Animator>();
        GetComponent<LevelLoader>();
        
        // start animation walking sequence
        anim.SetBool("isWalking", true);


    }

    public void startNavAgent(Vector3 startPoint) {

        // detect closest hit point
        UnityEngine.AI.NavMeshHit closestHit;

        // check there is a closest hit, start navagent there
        if( UnityEngine.AI.NavMesh.SamplePosition(startPoint, out closestHit, 500, 1 ) ){
            // add navagent 
            transform.position = closestHit.position;
            gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

            // set navagent's parameters
            agent.radius = agentRadius;


            Debug.Log("NavAgent for Scarecrow started successfully!");
            
        }
        else{
            Debug.Log("Error adding NavMeshAgent");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if chasing down the player
        if(state==State.Chasing) {
            // get direction
            Vector3 dir = player.transform.position - transform.position;

            // create quarternion looking at player
            //Quaternion lookRot = Quaternion.LookRotation(dir);

            // set x and z rotations to 0 (freezing rotation in rigidbody does not seem to work)
            //lookRot.x = 0; lookRot.z = 0;

            // slerp new rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Mathf.Clamp01(rotationSpeed * Time.maximumDeltaTime));
     
            // move towards player (BASIC IMPLEMENTATION)
            //transform.position += transform.forward * moveSpeed * Time.deltaTime;

            // move towards player (using navmesh)
            agent.SetDestination(player.transform.position);           
        }   

        // update attack timer if it's running
        if(attackTimerRunning)
            attackTimer += Time.deltaTime;     

    }

    // check for things entering trigger (player only currently)
    void OnTriggerEnter(Collider other) {

        // if player has entered trigger
        if(other.gameObject.CompareTag("Player")) {
            // debug
            Debug.Log("Player is in Scarecrow trigger!");
            
            // start animation
            anim.SetBool("isAttacking", true);
            
            // start timer
            if (!attackTimerRunning)
                attackTimerRunning = true;

        }
    }

    void OnTriggerStay(Collider other) {
        // if player has stayed in trigger
        if(other.gameObject.CompareTag("Player")) {
            if(attackTimer >= attackFinishedTime) {
                Debug.Log("PLAYER KILLED!!!!!!!!!!!!!!!!!!!");
                
                // transition to gameover screen
                levelLoader.GetComponent<LevelLoader>().loadGameOverScreen();
            }

        }
    }

    void OnTriggerExit(Collider other) {
        // if player has exited the trigger
        if(other.gameObject.CompareTag("Player")) {
            // reset attack timer            
            attackTimerRunning = false;
            attackTimer = 0.0f;

        }

    }

    public void stop() {
        state = State.Stopped;
    }


}
