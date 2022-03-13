using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public List<Transform> nodes;

    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitGrid()
    {
        foreach (Transform child in transform)
        {
            nodes.Add(child);
        }
    }

    public List<Transform> GetNodeList()
    {
        return nodes;
    }
}
