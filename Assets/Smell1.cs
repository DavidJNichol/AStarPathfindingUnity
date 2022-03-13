using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell1 : MonoBehaviour
{
    public GameObject agent;
    AgentIntelligence aiRef;
    
    // Start is called before the first frame update
    void Start()
    {
        aiRef = agent.GetComponent<AgentIntelligence>();
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            SmellTarget();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void SmellTarget()
    {
        Debug.Log("SMELL");
        aiRef.enemyFound = true;
    }
}
