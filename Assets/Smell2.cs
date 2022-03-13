using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smell2 : MonoBehaviour
{
    public GameObject agent;
    AgentIntelligenceII aiRef;

    // Start is called before the first frame update
    void Start()
    {
        aiRef = agent.GetComponent<AgentIntelligenceII>();
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
