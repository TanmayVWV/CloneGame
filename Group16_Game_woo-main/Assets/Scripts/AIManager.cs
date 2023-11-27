using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{

    private GameObject[] obstacles;
    // Start is called before the first frame update
    void Start()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Prop");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<GameObject> FindNearbyProps(Vector3 pos, float radius)
    {
        List<GameObject> toReturn = new List<GameObject>();
        foreach (GameObject obstacle in obstacles)
        {
            float distanceToObstacle = Vector3.Distance(pos, obstacle.transform.position);

            if (distanceToObstacle < radius)
            {
                toReturn.Add(obstacle);
            }
        }
        return toReturn;
    }
}
