using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;

public class AgentIntelligenceII : MonoBehaviour
{
    // REFRENCES 
    public GameObject enemy;

    public Animator anim;

    public GameObject grave;

    BoxCollider graveCollider;

    public GameObject head;

    NavMeshAgent navmesh;

    public GameObject agent;

    BoxCollider hearingCollider;

    SphereCollider smellingCollider;

    public GameObject food;

    public GameObject otherAgent;

    //************************ 


    // STATUS 
    public bool resting;

    public float stamina;

    public int staminaCap;

    public bool eating;
    //*************************


    // RESTBAR 
    public RestingBar restBar;

    public GameObject bar;

    public GameObject anchor;

    Transform anchorTransform;

    float barLength = 0f;

    public Camera mainCam;

    Vector3 camTransform;
    //**************************   


    // SENSES 
    bool enemyRaycastHit;

    public bool enemyFound;

    bool trackingTarget = true;
    //***************************

    //PATHFINDING
    Transform startLocation;
    Transform endLocation;
    Transform currentLocation;
    Transform parentNode;
    public GameObject grid;
    Pathfinder pathfinder;

    float G;
    float H;
    float F;

    const int MOVE_DIAGONAL_COST = 14;
    const int MOVE_STRAIGHT_COST = 10;

    List<Transform> nodesList;
    enum nodeState { Untested, Open, Closed }

    nodeState state;
    //***************************

    //MISC
    float value;
    //****************************

    void Start()
    {
        graveCollider = grave.GetComponent<BoxCollider>();
        restBar = GetComponent<RestingBar>();
        anim = GetComponent<Animator>();
        navmesh = GetComponent<NavMeshAgent>();
        hearingCollider = GetComponent<BoxCollider>();
        smellingCollider = GetComponent<SphereCollider>();
        pathfinder = grid.GetComponent<Pathfinder>();
        nodesList = pathfinder.nodes;
        SetBarVisibility(0);
        Eat();

    }

    void FixedUpdate()
    {
        if (enemyFound)
        {
            MoveAgent();
            eating = false;
        }

        if (resting)
        {
            Rest();

            if (stamina == staminaCap)
            {
                ExitRest();
            }
        }

        if(trackingTarget)
            Sight();
    }
    //SENSE 1
    void Sight()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(head.transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {

            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.DrawRay(head.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                enemyRaycastHit = true;
            }
            else
            {
                enemyRaycastHit = false;
            }           

            if (enemyRaycastHit)
            {
                enemyFound = true;
            }
        }
    }
    //SENSE 2 --- SENSE 3 IS IN SMELL SCRIPT
    void Hearing()
    {
        float hearingValue = Random.value;

        if (hearingValue < .009)
            enemyFound = true;

    }
    //COLLISION DETECTION
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Hearing();
            Debug.Log("HEARING");
        }
    }
    //NAVIGATION
    void MoveAgent()
    {
        //As long as the zombie has seen the player once, he will continuously chase
        if (enemyFound)
        {
            anim.SetBool("Eating", false);
            //AGENT RESTING FALSE
            if (resting == false)
            {
                navmesh.destination = enemy.transform.position;
                anim.SetBool("Moving", true);
                Debug.Log("Agent1 resting false");
                stamina -= 1;
            }
            else
            {
                //anim.SetBool("Moving", false);
            }

            // WHERE PATHFINDING.RESTING IS BEING STORED
            if (stamina == 0)
            {
                navmesh.destination = grave.transform.position;
                resting = true;
                trackingTarget = false;
            }
        }
    }
    //NON-MOVEMENT BEHAVIOR 1
    void Eat()
    {
        Debug.Log("EATING");
        navmesh.destination = food.transform.position;
        anim.SetBool("Moving", false);
        anim.SetBool("Eating", true);

        eating = true;
        Quaternion q = new Quaternion();
        agent.transform.localRotation = q;
        q.x += 180;

    }
    //NON-MOVEMENT BEHAVIOR 2
    void Rest()
    {
        trackingTarget = false;
        stamina++;

        Debug.Log("agent no longer tracking target , is resting");
        Debug.Log("agent stamina: " + stamina / 300);


        SetBarVisibility(1);
        if (barLength < .14f)
            barLength += stamina / 750000;

        SetBarLength();
    }

    public void SetBarVisibility(int type)
    {
        if (type == 1)
        {
            bar.transform.position = agent.transform.position + new Vector3(0f, 3f, 0f);
        }
        else
        {
            bar.transform.position = camTransform + new Vector3(0f, -1000f, 0f);
        }
    }

    public void SetBarLength()
    {
        anchor.transform.localScale = new Vector3(barLength, 0.14508f, 0.23634f);
        Debug.Log("Bar being set");
    }

    void ExitRest()
    {
        Debug.Log("agent no longer resting");
        resting = false;
        trackingTarget = true;
        navmesh.destination = enemy.transform.position;
        SetBarVisibility(0);
        barLength = 0f;
    }

    void Communicate()
    {
        if (otherAgent.GetComponent<AgentIntelligence>().enemyFound)
        {
            enemyFound = true;
        }
    }

    void Pathfind()
    {
        float startingF;
        //USE .position WHEN USING THESE
        startLocation = agent.transform;
        endLocation = grave.transform.transform;

        G = 0;
        H = CalculateDistance(startLocation, endLocation);
        F = CalculateFCost();
        startingF = F;
        currentLocation = startLocation;

        for (int i = 0; i < pathfinder.nodes.Count; i++)
        {
            G++;
            currentLocation = agent.transform;
            H = CalculateDistance(currentLocation, endLocation);
            F = CalculateFCost();
            CalculateDistanceToNearestGridPoint();
            //tileScores[i] = F;

        }

    }

    float CalculateFCost()
    {
        F = G + H;
        return F;
    }

    float CalculateDistance(Transform node1, Transform node2)
    {
        float xDistance = Mathf.Abs(node1.position.x - node2.position.x);
        float yDistance = Mathf.Abs(node1.position.y - node2.position.y);
        float remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    void CalculateDistanceToNearestGridPoint()
    {
        float xDistance;
        float yDistance;
        float remaining;
        Transform destination = agent.transform;
        List<float> gridScores = new List<float>();
        for (int i = 0; i < pathfinder.nodes.Count - 1; i++)
        {
            xDistance = Mathf.Abs(currentLocation.position.x - nodesList[i].position.x);
            yDistance = Mathf.Abs(currentLocation.position.y - nodesList[i].position.y);
            remaining = Mathf.Abs(xDistance - yDistance);

            gridScores[i] = remaining;
        }


        for (int i = 0; i < pathfinder.nodes.Count - 1; i++)
        {
            if (gridScores[i] == gridScores.Min())
            {
                destination = nodesList[i];
            }
            Vector3.Lerp(currentLocation.position, destination.position, Time.deltaTime);
        }
    }
}
