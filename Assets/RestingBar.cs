using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestingBar : MonoBehaviour
{
    GameObject[] bars = new GameObject[2];
    GameObject[] anchors = new GameObject[2];
    
    public Camera mainCam;
    Vector3 camTransform;

    public GameObject bar1;
    public GameObject bar2;

    public GameObject anchor1;
    public GameObject anchor2;

    Transform anchorTransform;
    float barLength = 0f;

    // Start is called before the first frame update
    void Start()
    {
        

        //BARS ARRAY IS NULL AFTER START??? ***********
       

        // SetBarVisibility(0);
        //SetBarVisibility(0 , agent1);
        

       // SetBarLength();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //int type 1 == ON, type 0 == OFF
    public void SetBarVisibility(int type, GameObject agent)
    {
        bars[0] = bar1;
        bars[1] = bar2;

        if (type == 1)
        {           
            if(agent.tag == "Agent1")
                bars[0].transform.position = agent.transform.position + new Vector3(0f,3f,0f);    
            else if(agent.tag == "Agent2")
                bars[1].transform.position = agent.transform.position + new Vector3(0f, 3f, 0f);

        }
        else
        {
            for (int i = 0; i < bars.Length; i++)
            {
                if (agent.tag == "Agent1")
                    bars[0].transform.position = camTransform + new Vector3(0f, -1000f, 0f);
                else if (agent.tag == "Agent2")
                    bars[1].transform.position = camTransform + new Vector3(0f, -1000f, 0f);
            }
        }
    }

    //ACTOR NUM MUST BE 1 OR 0
    public void SetBarLength(float length, int actorNum)
    {
        if(actorNum == 0)
        {
            anchor1.transform.localScale = new Vector3(length, 0.14508f, 0.23634f);
        }
        else
        {
            anchor2.transform.localScale = new Vector3(length, 0.14508f, 0.23634f);
        }
       
        
            
              
        
    }
}
