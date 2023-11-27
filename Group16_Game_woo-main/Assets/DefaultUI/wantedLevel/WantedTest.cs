using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WantedTest : MonoBehaviour
{

    [SerializeField] WantedLevel wantedLevel;

    [SerializeField] int maxWanted;

    private int currentWanted;

    // Start is called before the first frame update
    void Start()
    {
        currentWanted = 0;
        wantedLevel.UpdateWanted(currentWanted, maxWanted);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentWanted -= 10;
            wantedLevel.UpdateWanted(currentWanted, maxWanted);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            currentWanted += 10;
            wantedLevel.UpdateWanted(currentWanted, maxWanted);
        }
    }
}
